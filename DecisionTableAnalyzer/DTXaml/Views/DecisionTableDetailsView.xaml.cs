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
    [ViewUsage(DockingLibrary.ViewUsage.Multiple, DefaultDockDirection = ExtendedDockDirection.Inner)]
    public partial class DecisionTableDetailsView : View
    {
        public DecisionTableDetailsView()
        {
            InitializeComponent();
        }

    }
}
