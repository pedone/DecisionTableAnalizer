using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FunctionalTreeLibrary;

namespace DockingLibrary
{
    internal class TabReorderAndFlyoutManager
    {

        /// <summary>
        /// Maximum distance, reordering is not stopped when leaving the item to the sides.
        /// When passing this distance, reordering will be aborted.
        /// </summary>
        private const int MaxOutsideReorderDistance = 50;

        #region Variables

        private bool _IsActive;
        public bool IsActive
        {
            get
            {
                if (_IsActive && Mouse.LeftButton != MouseButtonState.Pressed)
                    _IsActive = false;

                return _IsActive;
            }
            private set { _IsActive = value; }
        }

        public DockingGroupTabItem ManagedItem { get; private set; }

        private bool _mouseLeftWhileReordering;
        /// <summary>
        /// True, if the item was at least once moved to a new position
        /// </summary>
        private bool _itemMoved;

        /// <summary>
        /// This value determines how far the mouse pointer will be positioned to the right on the floating window.
        /// The value is determined after mouse down and after an item has been reordered.
        /// </summary>
        private double _mouseXForFloatingWindow = 0d;

        #endregion

        #region Properties

        public View ManagedView
        {
            get
            {
                return ManagedItem.View;
            }
        }

        #endregion

        public TabReorderAndFlyoutManager(DockingGroupTabItem owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner", "owner is null.");

            ManagedItem = owner;
            InitHandler();
        }

        public void StartReordering()
        {
            IsActive = true;
            _mouseLeftWhileReordering = false;
            _itemMoved = false;
        }

        public void StopReordering()
        {
            IsActive = false;
            _mouseLeftWhileReordering = false;
            _itemMoved = false;

            if (Mouse.Captured == ManagedItem)
                Mouse.Capture(null);
        }

        private void InitHandler()
        {
            ManagedItem.AddHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(ManagedItem_MouseDown), true);
            ManagedItem.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(ManagedItem_MouseUp), true);
            ManagedItem.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(ManagedItem_MouseLeave), true);
            ManagedItem.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(ManagedItem_MouseMove), true);
            ManagedItem.AddHandler(Mouse.MouseEnterEvent, new MouseEventHandler(ManagedItem_MouseEnter), true);
        }

        private void ManagedItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StopReordering();
        }

        private void ManagedItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DisableAllReorderingSiblings();
                StartReordering();

                if (ManagedView.ParentContent is IInputElement)
                    _mouseXForFloatingWindow = e.GetPosition(ManagedView.ParentContent as IInputElement).X;
            }
        }

        private void MoveTabToPosition(DockingGroupTabItem item, DockingGroupTabItem targetPosition, TabGroup parentTabGroup)
        {
            ManagedView.DockManager.LockActiveView();
            parentTabGroup.Move(item.View, parentTabGroup.Items.IndexOf(targetPosition.View));
            ManagedView.DockManager.UnlockActiveView();

            item.ReorderManager._itemMoved = true;

            if (ManagedView.ParentContent is IInputElement)
                _mouseXForFloatingWindow = Mouse.GetPosition(ManagedView.ParentContent as IInputElement).X;
        }

        private void ManagedItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            
            TabGroup viewParentAsTabGroup = ManagedView.ParentContent as TabGroup;
            if (viewParentAsTabGroup == null)
                return;

            DockingGroupTabItem reorderingItem = GetReorderingTabItem(viewParentAsTabGroup);
            if (reorderingItem != null && reorderingItem.View != null && reorderingItem.View != ManagedView)
                MoveTabToPosition(reorderingItem, ManagedItem, viewParentAsTabGroup);
        }

        /// <summary>
        /// TODO think of a better name
        /// After an item has switched places with a larger item, the mouse will be in the larger item.
        /// When that happens, and the mouse is moved in the opposite direction until it reaches the space
        /// where the original item was placed, then the items should be switched back.
        /// </summary>
        private void HandleMouseMoveAfterSmallItemSwitched(Rect bounds, Point mousePosition)
        {
            if (ManagedView.ParentContent == null)
                return;

            TabGroup viewParentAsTabGroup = ManagedView.ParentContent as TabGroup;
            if (viewParentAsTabGroup == null)
                return;

            DockingGroupTabItem reorderingItem = GetReorderingTabItem(viewParentAsTabGroup);
            if (reorderingItem != null && reorderingItem.View != ManagedView)
            {
                Rect reorderingItemBounds = VisualTreeHelper.GetDescendantBounds(reorderingItem);
                int reorderingItemIndex = viewParentAsTabGroup.Items.IndexOf(reorderingItem.View);
                int thisIndex = viewParentAsTabGroup.Items.IndexOf(ManagedView);

                //Reordering item is right to this
                if (thisIndex < reorderingItemIndex)
                {
                    //While the indexes have been switched already, the visual items have not yet switched, let them finish
                    if (ManagedItem.TranslatePoint(new Point(), reorderingItem).X > 0)
                        return;

                    if (mousePosition.X < reorderingItemBounds.Width)
                        MoveTabToPosition(reorderingItem, ManagedItem, viewParentAsTabGroup);
                }
                else
                {
                    //While the indexes have been switched already, the visual items have not yet switched, let them finish
                    if (ManagedItem.TranslatePoint(new Point(), reorderingItem).X < 0)
                        return;

                    if (mousePosition.X > (bounds.Width - reorderingItemBounds.Width))
                        MoveTabToPosition(reorderingItem, ManagedItem, viewParentAsTabGroup);
                }
            }
        }

        private void ManagedItem_MouseMove(object sender, MouseEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(ManagedItem);
            Point mousePosition = e.GetPosition(ManagedItem);

            HandleMouseMoveAfterSmallItemSwitched(bounds, mousePosition);
            HandleMouseMoveOutsideReorderingItem(bounds, mousePosition);
        }

        private void HandleMouseMoveOutsideReorderingItem(Rect thisBounds, Point mousePosition)
        {
            if (!(IsActive && _mouseLeftWhileReordering))
                return;

            if (thisBounds.Contains(mousePosition))
            {
                //If mouse again in item continue reordering
                _mouseLeftWhileReordering = false;
                Mouse.Capture(null);
            }
            else if (mousePosition.X < -MaxOutsideReorderDistance || (mousePosition.X - thisBounds.Right) > MaxOutsideReorderDistance ||
                mousePosition.Y < -MaxOutsideReorderDistance || (mousePosition.Y - thisBounds.Bottom) > MaxOutsideReorderDistance)
            {
                //If mouse too far outside item, stop reordering
                StopReordering();
                //InitiateFloating();
            }
        }

        private void ManagedItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsActive)
                return;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(ManagedItem);
            Point mousePos = e.GetPosition(ManagedItem);

            TabGroup viewParentAsTabGroup = ManagedView.ParentContent as TabGroup;
            if (viewParentAsTabGroup == null)
                return;

            bool isFirstItem = viewParentAsTabGroup.Items.First() == ManagedView;
            bool isLastItem = viewParentAsTabGroup.Items.Last() == ManagedView;

            bool leftBoundsToLeftAndFirstItem = mousePos.X < bounds.X && isFirstItem;
            bool leftBoundsToRightAndLastItem = mousePos.X > bounds.Right && isLastItem;
            bool leftBoundsToTopOrBottom = mousePos.Y < bounds.Top || mousePos.Y > bounds.Bottom;

            if (leftBoundsToLeftAndFirstItem || leftBoundsToRightAndLastItem || leftBoundsToTopOrBottom)
            {
                if (_itemMoved)
                {
                    _mouseLeftWhileReordering = true;
                    Mouse.Capture(ManagedItem);
                }
                //else
                //{
                //    StopReordering();
                //    InitiateFloating();
                //}
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void InitiateFloating()
        {
            if (ManagedView.ParentContent == null)
                return;

            FloatingWindow floatingWindow = new FloatingWindow(_mouseXForFloatingWindow, ManagedView.ParentContent as FrameworkElement)
            {
                Owner = Window.GetWindow(ManagedItem)
            };
            
            //store the view or it will get destroyed
            View floatingView = ManagedView;

            floatingView.DockManager.AddFunctionalChild(floatingWindow);
            floatingView.ViewGroup.Remove(floatingView);
            
            //Initialize the floating window content
            TabGroup floatingTabGroup = new TabGroup();
            floatingTabGroup.Add(floatingView);
            floatingWindow.Content = floatingTabGroup;

            floatingWindow.Show();
            //After the window has been fully initialized, start dragging, since it's just been popped out
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                floatingWindow.DragMove();
        }

        private DockingGroupTabItem GetReorderingTabItem(TabGroup tabGroup)
        {
            //Check if any item is currently reordering
            View reorderingView = tabGroup.Items.FirstOrDefault(cur => tabGroup.GetContainer(cur).ReorderManager.IsActive);
            return tabGroup.GetContainer(reorderingView);
        }

        private void DisableAllReorderingSiblings()
        {
            TabGroup viewParentAsTabGroup = ManagedView.ParentContent as TabGroup;
            if (viewParentAsTabGroup != null)
            {
                foreach (var view in viewParentAsTabGroup.Items)
                    viewParentAsTabGroup.GetContainer(view).ReorderManager.StopReordering();
            }
        }


    }
}
