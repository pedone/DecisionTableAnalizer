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
using FunctionalTreeLibrary;

namespace FunctionalTreeDebuggerVisualizer
{
    /// <summary>
    /// Interaction logic for FunctionalTreeVisualizerWindow.xaml
    /// </summary>
    public partial class FunctionalTreeVisualizerWindow : Window
    {

        #region Variables

        public FunctionalTree FunctionalTree { get; private set; }

        #endregion

        #region Constructor
        public FunctionalTreeVisualizerWindow(FunctionalTree functionalTree)
        {
            InitializeComponent();

            FunctionalTree = functionalTree;
            SetupFunctionalTree();
        }
        #endregion

        #region SetupFunctionalTree
        private void SetupFunctionalTree()
        {
            if (FunctionalTree == null)
                return;

            TreeViewItem rootItem = new TreeViewItem { Header = FunctionalTree.Root.ToString() };
            FunctionalTreeContent.Items.Add(rootItem);
            AddFunctionalTreeItems(rootItem, FunctionalTreeHelper.GetFunctionalChildren(FunctionalTree.Root));
        }
        #endregion

        #region AddFunctionalTreeItems
        private void AddFunctionalTreeItems(TreeViewItem item, IEnumerable<IFunctionalTreeElement> elements)
        {
            foreach (IFunctionalTreeElement element in elements)
            {
                TreeViewItem subItem = new TreeViewItem { Header = element.ToString() };
                item.Items.Add(subItem);

                AddFunctionalTreeItems(subItem, FunctionalTreeHelper.GetFunctionalChildren(element));
            }
        }
        #endregion

    }
}
