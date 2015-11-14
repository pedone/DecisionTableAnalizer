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
    public class DTCellTemplateSelector : DataTemplateSelector
    {

        public static DTCellTemplateSelector Instance { get; private set; }

        static DTCellTemplateSelector()
        {
            Instance = new DTCellTemplateSelector();
        }

        private DTCellTemplateSelector()
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
                FrameworkElementFactory conditionFactory = new FrameworkElementFactory(typeof(DTCellComboBox));
                conditionFactory.SetValue(DTCellComboBox.DisplayMemberPathProperty, "Name");
                conditionFactory.SetBinding(DTCellComboBox.ItemsSourceProperty, new Binding("Header.ValidStatesWithEmptyState"));
                Binding selectedItemBinding = new Binding(string.Format("States[{0}]", ruleIndex))
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                };
                conditionFactory.SetBinding(ComboBox.SelectedItemProperty, selectedItemBinding);
                return new DataTemplate { VisualTree = conditionFactory };
            }
            else if (isAction)
            {
                FrameworkElementFactory actionFactory = new FrameworkElementFactory(typeof(DTCellComboBox));
                actionFactory.SetValue(DTCellComboBox.DisplayMemberPathProperty, "Name");
                actionFactory.SetBinding(DTCellComboBox.ItemsSourceProperty, new Binding("Header.ValidStatesWithEmptyState"));
                Binding selectedItemBinding = new Binding(string.Format("States[{0}]", ruleIndex))
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                };
                actionFactory.SetBinding(ComboBox.SelectedItemProperty, selectedItemBinding);
                return new DataTemplate { VisualTree = actionFactory };
            }

            return null;
        }

    }
}
