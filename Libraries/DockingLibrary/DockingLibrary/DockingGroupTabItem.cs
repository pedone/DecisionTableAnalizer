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

    public class DockingGroupTabItem : TabItem
    {

        #region Variables

        /// <summary>
        /// When the TabItem is collapsed/hidden, the desired size is set to empty, but the DocumentGroupPanel needs the desired
        /// size to calculate whethor or not the tabItem is visible. So the last desired size is saved in case the tabItem is invisible.
        /// </summary>
        internal Size LastDesiredSize { get; private set; }

        private bool _isOverflowHidden;
        internal bool IsOverflowHidden
        {
            get { return _isOverflowHidden; }
            set
            {
                if (_isOverflowHidden == value)
                    return;

                _isOverflowHidden = value;
                UpdateVisibility();
            }
        }

        public View View
        {
            get { return Content as View; }
        }

        internal TabReorderAndFlyoutManager ReorderManager { get; private set; }

        #endregion

        public DockingGroupTabItem()
        {
            ReorderManager = new TabReorderAndFlyoutManager(this);
        }

        private void UpdateVisibility()
        {
            if (IsOverflowHidden && Visibility != Visibility.Collapsed)
            {
                LastDesiredSize = DesiredSize;
                Visibility = Visibility.Collapsed;
            }
            else if (!IsOverflowHidden && Visibility != Visibility.Visible)
                Visibility = Visibility.Visible;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (View == null)
                return;

            if (e.MiddleButton == MouseButtonState.Pressed)
                View.Hide();
            else
                View.Activate();
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            View viewContent = Content as View;
            if (viewContent != null)
                viewContent.Activate();
        }

    }
}
