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

namespace UICore.Resources.Controls
{
    
    public class DTCellComboBox : ComboBox
    {

        static DTCellComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DTCellComboBox), new FrameworkPropertyMetadata(typeof(DTCellComboBox)));
        }

        public DTCellComboBox()
        {
            MouseDoubleClick += DTCellComboBox_MouseDoubleClick;
            PreviewMouseRightButtonDown += DTCellComboBox_MouseRightButtonDown;
        }

        private void DTCellComboBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = true;
        }

        private void DTCellComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedIndex = ((SelectedIndex + 1) % Items.Count);
        }

    }
}
