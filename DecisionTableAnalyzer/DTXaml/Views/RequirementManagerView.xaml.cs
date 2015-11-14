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

namespace DTXaml.Views
{
    [ViewUsage(DockingLibrary.ViewUsage.Single, DefaultDockDirection = ExtendedDockDirection.Inner)]
    public partial class RequirementManagerView : View
    {

        public RequirementManagerView()
        {
            InitializeComponent();
        }

        private void RequirementItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListView parentListView = TreeHelper.FindAncestor<ListView>(sender as ListViewItem);
            if (parentListView != null)
                parentListView.GetBindingExpression(ListView.SelectedItemProperty).UpdateSource();
        }
    }
}
