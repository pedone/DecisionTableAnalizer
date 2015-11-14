using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace DockingLibrary
{
    public class TabGroupPanel : StackPanel
    {

        public TabGroupPanel()
        {
            Orientation = Orientation.Horizontal;
            Background = Brushes.Transparent;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count == 0)
                return base.MeasureOverride(availableSize);

            var childrenList = Children.Cast<UIElement>().ToList();
            //Use double.PositiveInfinity as width, as it optimizes text measurement compared to a finite value
            //see http://blogs.msdn.com/b/visualstudio/archive/2010/03/23/wpf-in-visual-studio-part-5-window-management.aspx
            childrenList.ForEach(cur => cur.Measure(new Size(double.PositiveInfinity, availableSize.Height)));

            double allChildrenWidth = childrenList.Sum(cur => cur.DesiredSize.Width);
            double maxHeight = childrenList.Max(cur => cur.DesiredSize.Height);

            if (allChildrenWidth <= availableSize.Width)
                return new Size(allChildrenWidth, maxHeight);

            childrenList.Sort(SortChildByWidthDescending);
            double difference = allChildrenWidth - availableSize.Width;
            int curChildIndex = 0;
            while (difference > 0 && curChildIndex < (childrenList.Count - 1))
            {
                UIElement curChild = childrenList[curChildIndex];
                UIElement nextChild = childrenList[curChildIndex + 1];

                double differenceToNextChild = curChild.DesiredSize.Width - nextChild.DesiredSize.Width;
                if (difference <= differenceToNextChild)
                {
                    curChild.Measure(new Size(curChild.DesiredSize.Width - difference, availableSize.Height));
                    difference = 0;
                }
                else
                {
                    curChild.Measure(new Size(curChild.DesiredSize.Width - differenceToNextChild, availableSize.Height));
                    difference -= differenceToNextChild;
                }

                curChildIndex++;
            }
            if (difference > 0)
            {
                double widthPerChild = availableSize.Width / childrenList.Count;
                childrenList.ForEach(cur => cur.Measure(new Size(widthPerChild, availableSize.Height)));
            }

            return new Size(availableSize.Width, maxHeight);
        }

        private static int SortChildByWidthDescending(UIElement x, UIElement y)
        {
            if (x.DesiredSize.Width == y.DesiredSize.Width)
                return 0;
            if (x.DesiredSize.Width < y.DesiredSize.Width)
                return 1;

            return -1;
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
