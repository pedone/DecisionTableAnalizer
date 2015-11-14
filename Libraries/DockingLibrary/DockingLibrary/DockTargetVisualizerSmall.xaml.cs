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

namespace DockingLibrary
{
    /// <summary>
    /// Interaction logic for DockTargetVisualizerSmall.xaml
    /// </summary>
    public partial class DockTargetVisualizerSmall : UserControl
    {
        public DockTargetVisualizerSmall()
        {
            InitializeComponent();

            IsHitTestVisible = true;
            AddHandler(DockTargetVisualizerSmall.MouseMoveEvent, new RoutedEventHandler(MouseMove_Handler), true);
        }

        private void MouseMove_Handler(object sender, RoutedEventArgs e)
        { }

    }
}
