using System.Windows;
using System;
using System.Linq;
using HelperLibrary;

namespace DockingLibrary
{

    public class DocumentGroup : TabGroup
    {

        #region Dependency Properties

        internal static readonly DependencyProperty IsEmptyProperty;
        public static readonly DependencyProperty HasHiddenViewsProperty;

        internal bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            set { SetValue(IsEmptyProperty, value); }
        }

        public bool HasHiddenViews
        {
            get { return (bool)GetValue(HasHiddenViewsProperty); }
            set { SetValue(HasHiddenViewsProperty, value); }
        }

        #endregion

        static DocumentGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentGroup), new FrameworkPropertyMetadata(typeof(DocumentGroup)));

            //Register Dependency Properties
            IsEmptyProperty = DependencyProperty.Register("IsEmpty", typeof(bool), typeof(DocumentGroup), new UIPropertyMetadata(true));
            HasHiddenViewsProperty = DependencyProperty.Register("HasHiddenViews", typeof(bool), typeof(DocumentGroup), new UIPropertyMetadata(false));
        }

        public DocumentGroup()
            : base(DockState.TabbedDocument, DockState.TabbedDocument | DockState.Hide | DockState.Float)
        {
            AutoClose = false;
        }

        /// <summary>
        /// Check for hidden views and update the hidden document menu
        /// </summary>
        internal void UpdateHiddenViews()
        {
            HasHiddenViews = Items.Any(cur =>
                {
                    DockingGroupTabItem curItem = GetContainer(cur);
                    return curItem != null && curItem.IsOverflowHidden;
                });
        }

        protected override void OnViewGroupDockStateChanged(DockState oldState, DockState newState)
        {
            IsEmpty = (newState == DockState.Hide);
        }

        protected override void OnItemsChanged(System.Collections.Generic.IEnumerable<View> oldItems, System.Collections.Generic.IEnumerable<View> newItems)
        {
            base.OnItemsChanged(oldItems, newItems);

            IsEmpty = (Items.Count == 0);
        }

        public override bool Show(View view)
        {
            bool success = base.Show(view);

            if (success && SelectedView != null && SelectedView.Visibility != Visibility.Visible)
                //Move the hidden view to the beginning
                Move(SelectedView, 0);

            return success;
        }

    }
}
