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

namespace DTXaml.Dialogs
{
    /// <summary>
    /// Interaction logic for CheckForContradictionDialog.xaml
    /// </summary>
    public partial class CheckForContradictionDialog : Window
    {
        public CheckForContradictionDialog()
        {
            InitializeComponent();
        }

        private void RemoveContradictedRules_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
