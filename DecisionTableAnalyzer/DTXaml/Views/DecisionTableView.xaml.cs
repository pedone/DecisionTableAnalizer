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
using DockingLibrary;
using HelperLibrary;
using ExtensionLibrary;
using ViewModels;

namespace DTXaml.Views
{
    /// <summary>
    /// Interaction logic for DecisionTableView.xaml
    /// </summary>
    [ViewUsage(DockingLibrary.ViewUsage.Multiple, DefaultDockDirection = ExtendedDockDirection.Inner)]
    public partial class DecisionTableView : View
    {
        public DecisionTableView()
        {
            InitializeComponent();
        }

        private void DataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var rows = e.NewValue as List<RowViewModel>;
            if (rows != null)
                GenerateColumns(sender as DataGrid, rows);
        }

        private void GenerateColumns(DataGrid dataGrid, List<RowViewModel> rows)
        {
            if (dataGrid == null || rows.Count == 0)
                return;

            var nameColumn = dataGrid.Columns.First();
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(nameColumn);

            var ruleCount = rows.First().States.Count;
            for (int i = 0; i < ruleCount; i++)
            {
                dataGrid.Columns.Add(new DataGridTemplateColumn
                {
                    Header = i + 1,
                    CellTemplateSelector = DTCellTemplateSelector.Instance,
                    MinWidth = 70
                });
            }
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedCells.Count == 0)
                return;

            DecisionTableViewModel viewModel = DataContext as DecisionTableViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedRuleIndex = dataGrid.SelectedCells[0].Column.DisplayIndex - 1;
            }
        }

    }
}
