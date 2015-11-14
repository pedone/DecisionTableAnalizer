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
using HelperLibrary;
using ViewModels;
using DTCore;

namespace DTXaml.Dialogs
{
    /// <summary>
    /// Interaction logic for PrintDecisionTablesDialog.xaml
    /// </summary>
    public partial class PrintDecisionTablesDialog : Window
    {
        public PrintDecisionTablesDialog()
        {
            InitializeComponent();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public void TreeViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as PrintDecisionTablesDialogModel;
            var treeViewItem = TreeHelper.FindAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (treeViewItem != null && viewModel != null)
                viewModel.SelectedElement = treeViewItem.DataContext as ViewModel;
        }

    }
}
