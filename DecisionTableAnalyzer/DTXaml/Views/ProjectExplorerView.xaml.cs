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
using ViewModels;
using DTCore;

namespace DTXaml.Views
{

    [ViewUsage(DockingLibrary.ViewUsage.Single, DefaultDockDirection = ExtendedDockDirection.Right)]
    public partial class ProjectExplorerView : View
    {
        public ProjectExplorerView()
        {
            InitializeComponent();
        }

        public void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = TreeHelper.FindAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (treeViewItem != null)
                treeViewItem.IsSelected = true;
        }

        public void TreeViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as ProjectExplorerViewModel;
            var treeViewItem = TreeHelper.FindAncestor<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (treeViewItem != null && viewModel != null)
                viewModel.SelectedElement = treeViewItem.DataContext as ViewModel;
        }

    }
}
