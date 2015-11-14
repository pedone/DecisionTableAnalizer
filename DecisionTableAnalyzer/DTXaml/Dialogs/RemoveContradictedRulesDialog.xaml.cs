using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModels;

namespace DTXaml.Dialogs
{
    /// <summary>
    /// Interaction logic for RemoveContradictedRulesDialog.xaml
    /// </summary>
    public partial class RemoveContradictedRulesDialog : Window
    {
        public RemoveContradictedRulesDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DataGridRules_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rows = e.NewValue as List<RowViewModel>;
            var dataContext = DataContext as RemoveContradictedRulesDialogModel;
            if (rows != null && dataContext != null)
                GenerateRuleColumns(sender as DataGrid, rows, dataContext.Rules);
        }

        private void DataGridContradictions_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rows = e.NewValue as List<ContradictionViewModel>;
            var dataContext = DataContext as RemoveContradictedRulesDialogModel;
            if (rows != null && dataContext != null)
                GenerateContradictionColumns(sender as DataGrid, rows, dataContext.Rules);
        }

        private void GenerateContradictionColumns(DataGrid dataGrid, List<ContradictionViewModel> rows, List<RuleViewModel> rules)
        {
            if (dataGrid == null || rows.Count == 0)
                return;

            var nameColumn = dataGrid.Columns.First();
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(nameColumn);

            var sortedRules = rules.OrderBy(cur => cur.Index);
            foreach (var rule in sortedRules)
            {
                dataGrid.Columns.Add(new DataGridTemplateColumn
                {
                    Header = rule,
                    HeaderTemplateSelector = ContradictionSelectionHeaderTemplateSelector.Instance,
                    CellTemplateSelector = ContradictionCellTemplateSelector.Instance,
                    MinWidth = 45,
                    MaxWidth = 45
                });
            }
        }

        private void GenerateRuleColumns(DataGrid dataGrid, List<RowViewModel> rows, List<RuleViewModel> rules)
        {
            if (dataGrid == null || rows.Count == 0)
                return;

            var nameColumn = dataGrid.Columns.First();
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(nameColumn);

            foreach (var rule in rules)
            {
                dataGrid.Columns.Add(new DataGridTemplateColumn
                {
                    Header = rule.Index + 1,
                    CellTemplateSelector = DTReadOnlyCellTemplateSelector.Instance,
                    MinWidth = 70
                });
            }
        }

    }

}
