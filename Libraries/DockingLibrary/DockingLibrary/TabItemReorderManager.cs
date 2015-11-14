using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DockingLibrary
{
    internal class TabItemReorderManager
    {

        /// <summary>
        /// Maximum distance, reordering is not stopped when leaving the item to the sides.
        /// When passing this distance, reordering will be aborted.
        /// </summary>
        private const int MaxOutsideReorderDistance = 50;

        #region Variables

        private bool _IsActive;
        #region IsActive
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
        #endregion

        public DockingGroupTabItem ManagedItem { get; private set; }

        private bool MouseLeftWhileReordering { get; set; }
        /// <summary>
        /// True, if the item was at least once moved to a new position
        /// </summary>
        private bool ItemMoved { get; set; }

        #endregion

        #region Constructor
        public TabItemReorderManager(DockingGroupTabItem owner)
        {
            ManagedItem = owner;
            InitHandler();
        }
        #endregion

        #region StartReordering
        public void StartReordering()
        {
            IsActive = true;
            MouseLeftWhileReordering = false;
            ItemMoved = false;
        }
        #endregion

        #region StopReordering
        public void StopReordering()
        {
            IsActive = false;
            MouseLeftWhileReordering = false;
            ItemMoved = false;

            if (Mouse.Captured == ManagedItem)
                Mouse.Capture(null);
        }
        #endregion

        #region InitHandler
        private void InitHandler()
        {
            ManagedItem.AddHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(ManagedItem_MouseDown), true);
            ManagedItem.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(ManagedItem_MouseUp), true);
            ManagedItem.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(ManagedItem_MouseLeave), true);
            ManagedItem.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(ManagedItem_MouseMove), true);
            ManagedItem.AddHandler(Mouse.MouseEnterEvent, new MouseEventHandler(ManagedItem_MouseEnter), true);
        }
        #endregion

        #region ManagedItem_MouseUp
        private void ManagedItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StopReordering();
        }
        #endregion

        #region ManagedItem_MouseDown
        private void ManagedItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DisableAllReorderingSiblings();
                StartReordering();
            }
        }
        #endregion

        #region MoveTab
        private void MoveTab(DockingGroupTabItem item, DockingGroupTabItem targetItem, TabGroup parentTabGroup)
        {
            ManagedItem.View.DockManager.LockActiveView();
            parentTabGroup.Move(item.View, parentTabGroup.Items.IndexOf(targetItem.View));
            ManagedItem.View.DockManager.UnlockActiveView();

            item.ReorderManager.ItemMoved = true;
        }
        #endregion

        #region ManagedItem_MouseEnter
        private void ManagedItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ManagedItem.View == null || e.LeftButton != MouseButtonState.Pressed)
                return;
            
            TabGroup viewParentAsTabGroup = ManagedItem.View.ParentContent as TabGroup;
            if (viewParentAsTabGroup == null)
                return;

            DockingGroupTabItem reorderingItem = GetReorderingTabItem(viewParentAsTabGroup);
            if (reorderingItem != null && reorderingItem.View != null && reorderingItem.View != ManagedItem.View)
                MoveTab(reorderingItem, ManagedItem, viewParentAsTabGroup);
        }
        #endregion

        #region HandleMouseMoveAfterSmallItemSwitched
        /// <summary>
        /// TODO think of a better name
        /// After an item has switched places with a larger item, the mouse will be in the larger item.
        /// When that happens, and the mouse is moved in the opposite direction until it reaches the space
        /// where the original item was placed, then the items should be switched back.
        /// </summary>
        private void HandleMouseMoveAfterSmallItemSwitched(Rect bounds, Point mousePosition)
        {
            TabGroup viewParentAsTabGroup = ManagedItem.View.ParentContent as TabGroup;
            if (viewParentAsTabGroup == null)
                return;

            DockingGroupTabItem reorderingItem = GetReorderingTabItem(viewParentAsTabGroup);
            if (reorderingItem != null && reorderingItem.View != ManagedItem.View)
            {
                Rect reorderingItemBounds = VisualTreeHelper.GetDescendantBounds(reorderingItem);
                int reorderingItemIndex = viewParentAsTabGroup.Items.IndexOf(reorderingItem.View);
                int thisIndex = viewParentAsTabGroup.Items.IndexOf(ManagedItem.View);

                //Reordering item is right to this
                if (thisIndex < reorderingItemIndex)
                {
                    //While the indexes have been switched already, the visual items have not yet switched, let them finish
                    if (ManagedItem.TranslatePoint(new Point(), reorderingItem).X > 0)
                        return;

                    if (mousePosition.X < reorderingItemBounds.Width)
                        MoveTab(reorderingItem, ManagedItem, viewParentAsTabGroup);
                }
                else
                {
                    //While the indexes have been switched already, the visual items have not yet switched, let them finish
                    if (ManagedItem.TranslatePoint(new Point(), reorderingItem).X < 0)
                        return;

                    if (mousePosition.X > (bounds.Width - reorderingItemBounds.Width))
                        MoveTab(reorderingItem, ManagedItem, viewParentAsTabGroup);
                }
            }
        }
        #endregion

        #region ManagedItem_MouseMove
        private void ManagedItem_MouseMove(object sender, MouseEventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(ManagedItem);
            Point mousePosition = e.GetPosition(ManagedItem);

            HandleMouseMoveAfterSmallItemSwitched(bounds, mousePosition);
            HandleMouseMoveOutsideReorderingItem(bounds, mousePosition);
        }
        #endregion

        #region HandleMouseMoveOutsideReorderingItem
        private void HandleMouseMoveOutsideReorderingItem(Rect thisBounds, Point mousePosition)
        {
            if (!(IsActive && MouseLeftWhileReordering))
                return;

            if (thisBounds.Contains(mousePosition))
            {
                //If mouse again in item continue reordering
                MouseLeftWhileReordering = false;
                Mouse.Capture(null);
            }
            else if (mousePosition.X < -MaxOutsideReorderDistance || (mousePosition.X - thisBounds.Right) > MaxOutsideReorderDistance ||
                mousePosition.Y < -MaxOutsideReorderDistance || (mousePosition.Y - thisBounds.Bottom) > MaxOutsideReorderDistance)
            {
                //If mouse too far outside item, stop reordering
                StopReordering();
            }
        }
        #endregion

        #region ManagedItem_MouseLeave
        private void ManagedItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsActive || ManagedItem.View == null)
                return;

            Rect bounds = VisualTreeHelper.GetDescendantBounds(ManagedItem);
            Point mousePos = e.GetPosition(ManagedItem);

            TabGroup viewParentAsTabGroup = ManagedItem.View.ParentContent as TabGroup;
            if (viewParentAsTabGroup == null)
                return;

            bool isFirstItem = viewParentAsTabGroup.Items.First() == ManagedItem.View;
            bool isLastItem = viewParentAsTabGroup.Items.Last() == ManagedItem.View;

            bool leftBoundsToLeftAndFirstItem = mousePos.X < bounds.X && isFirstItem;
            bool leftBoundsToRightAndLastItem = mousePos.X > bounds.Right && isLastItem;
            bool leftBoundsToTopOrBottom = mousePos.Y < bounds.Top || mousePos.Y > bounds.Bottom;

            if (leftBoundsToLeftAndFirstItem || leftBoundsToRightAndLastItem || leftBoundsToTopOrBottom)
            {
                if (ItemMoved)
                {
                    MouseLeftWhileReordering = true;
                    Mouse.Capture(ManagedItem);
                }
                else
                    StopReordering();
            }
        }
        #endregion

        #region GetReorderingTabItem
        private DockingGroupTabItem GetReorderingTabItem(TabGroup tabGroup)
        {
            //Check if any item is currently reordering
            View reorderingView = tabGroup.Items.FirstOrDefault(cur => tabGroup.GetContainer(cur).ReorderManager.IsActive);
            return tabGroup.GetContainer(reorderingView);
        }
        #endregion

        #region DisableAllReorderingSiblings
        private void DisableAllReorderingSiblings()
        {
            if (ManagedItem.View == null)
                return;

            TabGroup viewParentAsTabGroup = ManagedItem.View.ParentContent as TabGroup;
            if (viewParentAsTabGroup != null)
            {
                foreach (var view in viewParentAsTabGroup.Items)
                    viewParentAsTabGroup.GetContainer(view).ReorderManager.StopReordering();
            }
        }
        #endregion


    }
}
