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

namespace DTXaml
{
    public class DTReadOnlyCellTemplateSelector : DataTemplateSelector
    {

        public static DTReadOnlyCellTemplateSelector Instance { get; private set; }

        static DTReadOnlyCellTemplateSelector()
        {
            Instance = new DTReadOnlyCellTemplateSelector();
        }

        private DTReadOnlyCellTemplateSelector()
        { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var row = item as RowViewModel;
            if (row == null)
                return null;

            bool isCondition = row.Header is ConditionViewModel;
            bool isAction = row.Header is ActionViewModel;

            IEnumerable<DependencyObject> cellPanelRoute = TreeHelper.GetRouteToAncestor<DataGridCellsPanel>(container);
            if (cellPanelRoute.Count() < 2)
                return null;

            DataGridCellsPanel cellPanel = cellPanelRoute.ElementAt(0) as DataGridCellsPanel;
            DataGridCell currentCell = cellPanelRoute.ElementAt(1) as DataGridCell;
            //First item is name of the condition/action, so minus 1
            int ruleIndex = cellPanel.Children.IndexOf(currentCell) - 1;

            if (isCondition)
            {
                FrameworkElementFactory conditionFactory = new FrameworkElementFactory(typeof(TextBlock));
                conditionFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                conditionFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                Binding textBinding = new Binding(string.Format("States[{0}].Name", ruleIndex))
                {
                    Mode = BindingMode.OneTime
                };
                conditionFactory.SetBinding(TextBlock.TextProperty, textBinding);
                return new DataTemplate { VisualTree = conditionFactory };
            }
            else if (isAction)
            {
                FrameworkElementFactory actionFactory = new FrameworkElementFactory(typeof(TextBlock));
                actionFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                actionFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                Binding textBinding = new Binding(string.Format("States[{0}].Name", ruleIndex))
                {
                    Mode = BindingMode.OneTime
                };
                actionFactory.SetBinding(TextBlock.TextProperty, textBinding);
                return new DataTemplate { VisualTree = actionFactory };
            }

            return null;
        }

    }
}
