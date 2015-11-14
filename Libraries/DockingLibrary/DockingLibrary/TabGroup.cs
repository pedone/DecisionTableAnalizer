using System.Windows;
using System;
using System.Windows.Controls;
using HelperLibrary;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using FunctionalTreeLibrary;

namespace DockingLibrary
{

    [TemplatePart(Name = "PART_TabGroupContentTabControl", Type = typeof(TabControl))]
    public class TabGroup : DockingItemsControl<View>, ISelectingViewContainer
    {

        #region Variables

        public ViewGroup ViewGroup { get; private set; }
        private TabControl TabGroupContentTabControlPart { get; set; }

        /// <summary>
        /// The TabGroup will be visible, whenever the ViewGroup has this DockState.
        /// </summary>
        private DockState VisibleDockState { get; set; }

        #endregion

        #region Properties

        public View SelectedView
        {
            get
            {
                if (SelectedIndex != -1 && SelectedIndex < Items.Count)
                    return (View)Items[SelectedIndex];

                return null;
            }
        }

        public int SelectedIndex
        {
            get
            {
                if (TabGroupContentTabControlPart == null)
                    return -1;

                return TabGroupContentTabControlPart.SelectedIndex;
            }
            set
            {
                if (TabGroupContentTabControlPart != null && value < Items.Count)
                {
                    //If the index is already set to the value, but the tab is visually not selected properly,
                    //it needs an actual change of the index, set it back to -1 first so there's a change
                    if (TabGroupContentTabControlPart.SelectedIndex == value)
                        TabGroupContentTabControlPart.SelectedIndex = -1;

                    TabGroupContentTabControlPart.SelectedIndex = value;
                }
            }
        }
        
        #endregion

        static TabGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabGroup), new FrameworkPropertyMetadata(typeof(TabGroup)));
        }

        public TabGroup()
            : this(DockState.Dock)
        { }

        internal TabGroup(DockState visibleDockState)
            : this(visibleDockState, DockState.Dock | DockState.AutoHide | DockState.Float | DockState.TabbedDocument | DockState.Hide)
        { }

        internal TabGroup(DockState visibleDockState, DockState validDockStates)
            : base(promoteLastItem: false)
        {
            AutoClose = true;
            VisibleDockState = visibleDockState;
            InitViewGroup(VisibleDockState, validDockStates);
        }

        private void InitViewGroup(DockState visibleDockState, DockState validDockStates)
        {
            ViewGroup = new ViewGroup(this, visibleDockState, validDockStates);

            ViewGroup.DockStateChanged += ViewGroup_DockStateChanged;
            ViewGroup.ViewsChanged += ViewGroup_ViewsChanged;
            ViewGroup.ViewDockStateChanging += ViewGroup_ViewDockStateChanging;
            ViewGroup.ViewDockStateChanged += ViewGroup_ViewDockStateChanged;
        }

        public override void OnApplyTemplate()
        {
            TabGroupContentTabControlPart = Template.FindName("PART_TabGroupContentTabControl", this) as TabControl;
            Debug.Assert(TabGroupContentTabControlPart != null, "PART_LeftAutoHideChannel must be defined.");
        }

        /// <summary>
        /// Activates the selected item
        /// </summary>
        public void Activate()
        {
            if (SelectedView != null)
                SelectedView.Activate();
        }

        private void ViewGroup_DockStateChanged(object sender, ValueChangedEventArgs<ViewGroup, DockState> e)
        {
            IsEmpty = e.NewValue != VisibleDockState;

            OnViewGroupDockStateChanged(e.OldValue, e.NewValue);
        }

        private void ViewGroup_ViewDockStateChanging(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (e.NewValue != VisibleDockState)
                Remove(e.Source);
        }

        private void ViewGroup_ViewDockStateChanged(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (e.NewValue == VisibleDockState)
                Add(e.Source);
        }

        private void ViewGroup_ViewsChanged(object sender, ValueChangedEventArgs<ViewGroup, IEnumerable<View>> e)
        {
            if (e.OldValue != null)
            {
                foreach (View view in e.OldValue)
                    Remove(view);
            }

            if (e.NewValue != null)
            {
                foreach (View view in e.NewValue)
                {
                    if (view.DockState == VisibleDockState)
                        Add(view);
                }
            }
        }

        protected override void OnItemsChanged(IEnumerable<View> oldItems, IEnumerable<View> newItems)
        {
            base.OnItemsChanged(oldItems, newItems);

            if (newItems != null)
            {
                foreach (View view in newItems)
                    ViewGroup.Add(view);
            }
        }

        internal DockingGroupTabItem GetContainer(View view)
        {
            if (Items.Contains(view))
                return TabGroupContentTabControlPart.ItemContainerGenerator.ContainerFromItem(view) as DockingGroupTabItem;

            return null;
        }

        public virtual bool Show(View view)
        {
            int index = Items.IndexOf(view);
            if (index == -1 || SelectedIndex == index)
                return true;

            SelectedIndex = index;
            return true;
        }

        #region Virtuals

        protected virtual void OnSelectionChanged(View view)
        { }

        protected virtual void OnViewGroupDockStateChanged(DockState oldState, DockState newState)
        { }

        #endregion

    }
}
