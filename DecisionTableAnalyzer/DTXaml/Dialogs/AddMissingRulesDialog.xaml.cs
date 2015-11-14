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
    /// Interaction logic for AddMissingRulesDialog.xaml
    /// </summary>
    public partial class AddMissingRulesDialog : Window
    {
        public AddMissingRulesDialog()
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

        private void DataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rows = e.NewValue as List<RowViewModel>;
            var dataContext = DataContext as AddMissingRulesDialogModel;
            if (rows != null && dataContext != null)
                GenerateColumns(sender as DataGrid, rows, dataContext.Rules);
        }

        private void GenerateColumns(DataGrid dataGrid, List<RowViewModel> rows, List<RuleViewModel> rules)
        {
            if (dataGrid == null || rows.Count == 0)
                return;

            var nameColumn = dataGrid.Columns.First();
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(nameColumn);

            var sortedRules = rules.OrderBy(cur => cur.Index);
            foreach(var rule in sortedRules)
            {
                dataGrid.Columns.Add(new DataGridTemplateColumn
                {
                    Header = rule,
                    HeaderTemplateSelector = RuleSelectorHeaderTemplateSelector.Instance,
                    CellTemplateSelector = DTReadOnlyCellTemplateSelector.Instance,
                    MinWidth = 70
                });
            }
        }

    }

}
