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
    
    public class ToolButton : Button
    {

        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty DisabledIconProperty;

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public ImageSource DisabledIcon
        {
            get { return (ImageSource)GetValue(DisabledIconProperty); }
            set { SetValue(DisabledIconProperty, value); }
        }

        static ToolButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolButton), new FrameworkPropertyMetadata(typeof(ToolButton)));

            IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ToolButton), new UIPropertyMetadata(null));
            DisabledIconProperty = DependencyProperty.Register("DisabledIcon", typeof(ImageSource), typeof(ToolButton), new UIPropertyMetadata(null));
        }

    }
}
