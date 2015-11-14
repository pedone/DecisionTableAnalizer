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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelperLibrary;
using DockingLibrary;

namespace DTXaml.Views
{
    /// <summary>
    /// Interaction logic for DecisionTableManagerView.xaml
    /// </summary>
    [ViewUsage(DockingLibrary.ViewUsage.Single, DefaultDockDirection = ExtendedDockDirection.Inner)]
    public partial class DecisionTableManagerView : View
    {
        public DecisionTableManagerView()
        {
            InitializeComponent();
        }

        private void DTItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListView parentListView = TreeHelper.FindAncestor<ListView>(sender as ListViewItem);
            if (parentListView != null)
                parentListView.GetBindingExpression(ListView.SelectedItemProperty).UpdateSource();
        }

    }
}
