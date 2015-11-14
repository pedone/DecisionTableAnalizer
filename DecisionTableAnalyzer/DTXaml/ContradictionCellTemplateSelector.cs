using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using HelperLibrary;
using UICore.Resources.Controls;
using ViewModels;
using System.Windows.Media;

namespace DTXaml
{
    public class ContradictionCellTemplateSelector : DataTemplateSelector
    {

        public static ContradictionCellTemplateSelector Instance { get; private set; }

        static ContradictionCellTemplateSelector()
        {
            Instance = new ContradictionCellTemplateSelector();
        }

        private ContradictionCellTemplateSelector()
        { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var row = item as ContradictionViewModel;
            if (row == null)
                return null;

            IEnumerable<DependencyObject> cellPanelRoute = TreeHelper.GetRouteToAncestor<DataGridCellsPanel>(container);
            if (cellPanelRoute.Count() < 2)
                return null;

            DataGridCellsPanel cellPanel = cellPanelRoute.ElementAt(0) as DataGridCellsPanel;
            DataGridCell currentCell = cellPanelRoute.ElementAt(1) as DataGridCell;
            //First item is the header, so minus 1
            int ruleIndex = cellPanel.Children.IndexOf(currentCell) - 1;

            //Build common background border
            //selected rows and columns will be highlighted
            var borderStyle = new Style { TargetType = typeof(Border) };
            var highlightBrush = new SolidColorBrush(Color.FromArgb(0xE0, 0xC6, 0xD3, 0xD3));
            var isSelectedTriggerOne = new DataTrigger
            {
                Binding = new Binding("Header.IsSelected"),
                Value = true
            };
            isSelectedTriggerOne.Setters.Add(new Setter(Border.BackgroundProperty, highlightBrush));

            var isSelectedTriggerTwo = new DataTrigger
            {
                Binding = new Binding(string.Format("Rules[{0}].IsSelected", ruleIndex)),
                Value = true
            };
            isSelectedTriggerTwo.Setters.Add(new Setter(Border.BackgroundProperty, highlightBrush));

            borderStyle.Triggers.Add(isSelectedTriggerOne);
            borderStyle.Triggers.Add(isSelectedTriggerTwo);

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.StyleProperty, borderStyle);

            //since this is readonly and the states won't change, there's no binding required here.
            //just decide what properties to use right here.
            bool? isContradicted = row.ContradictionStates[ruleIndex];
            if (isContradicted == null)
            {
                borderFactory.SetValue(Border.BackgroundProperty, Brushes.LightGray);
            }
            else if (isContradicted == true)
            {
                FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                textBlockFactory.SetValue(TextBlock.TextProperty, "X");
                textBlockFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                textBlockFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

                borderFactory.AppendChild(textBlockFactory);
            }

            return new DataTemplate { VisualTree = borderFactory };
        }

    }
}
