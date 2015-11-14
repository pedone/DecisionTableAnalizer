using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using HelperLibrary;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace DockingLibrary
{
    internal class DocumentGroupPanel : StackPanel
    {

        public DocumentGroupPanel()
        {
            Orientation = Orientation.Horizontal;
            Background = Brushes.Transparent;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count == 0)
                return base.MeasureOverride(availableSize);

            List<UIElement> childrenList = Children.OfType<UIElement>().ToList();
            //Use double.PositiveInfinity as width, as it optimizes text measurement compared to a finite value
            //see http://blogs.msdn.com/b/visualstudio/archive/2010/03/23/wpf-in-visual-studio-part-5-window-management.aspx
            childrenList.ForEach(cur => cur.Measure(new Size(double.PositiveInfinity, availableSize.Height)));

            //Hide the items, that don't fit one the documentGroup header
            double remainingSpace = availableSize.Width;
            foreach (DockingGroupTabItem item in childrenList.OfType<DockingGroupTabItem>())
            {
                Size actualDesiredSize = (item.DesiredSize.Width == 0 && item.DesiredSize.Height == 0 ? item.LastDesiredSize : item.DesiredSize);
                if (actualDesiredSize.Width <= remainingSpace)
                {
                    remainingSpace -= actualDesiredSize.Width;
                    item.IsOverflowHidden = false;
                }
                else
                {
                    remainingSpace = 0;
                    item.IsOverflowHidden = true;
                }
            }

            DockingGroupTabItem firstTabItem = childrenList.OfType<DockingGroupTabItem>().FirstOrDefault();
            if (firstTabItem != null && firstTabItem.View != null)
            {
                DocumentGroup documentGroup = firstTabItem.View.ParentContent as DocumentGroup;
                if (documentGroup != null)
                    documentGroup.UpdateHiddenViews();
            }

            double maxHeight = childrenList.Max(cur => cur.DesiredSize.Height);
            return new Size(availableSize.Width, maxHeight);
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            DockingGroupTabItem selectedTabItem = Children.OfType<DockingGroupTabItem>().FirstOrDefault(cur => cur.IsSelected);
            if (selectedTabItem != null &&
                selectedTabItem.View != null &&
                !selectedTabItem.View.IsActive)
            {
                selectedTabItem.View.Activate();
            }
        }

    }
}
