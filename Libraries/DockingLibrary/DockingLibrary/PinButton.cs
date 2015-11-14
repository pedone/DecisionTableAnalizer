using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace DockingLibrary
{
    internal class PinButton : Button
    {

        #region Dependency Properties

        internal static readonly DependencyProperty KeepButtonPressedProperty;
        internal static readonly DependencyProperty ForegroundOpacityMaskProperty;
        internal static readonly DependencyProperty PinModeProperty;
        public static readonly DependencyProperty IsActiveProperty;

        internal bool KeepButtonPressed
        {
            get { return (bool)GetValue(KeepButtonPressedProperty); }
            set { SetValue(KeepButtonPressedProperty, value); }
        }

        internal Brush ForegroundOpacityMask
        {
            get { return (Brush)GetValue(ForegroundOpacityMaskProperty); }
            set { SetValue(ForegroundOpacityMaskProperty, value); }
        }

        internal PinButtonMode PinMode
        {
            get { return (PinButtonMode)GetValue(PinModeProperty); }
            set { SetValue(PinModeProperty, value); }
        }

        internal bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        #endregion

        static PinButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PinButton), new FrameworkPropertyMetadata(typeof(PinButton)));

            //Register Dependency Properties
            KeepButtonPressedProperty = DependencyProperty.Register("KeepButtonPressed", typeof(bool), typeof(PinButton), new UIPropertyMetadata(false));
            ForegroundOpacityMaskProperty = DependencyProperty.Register("ForegroundOpacityMask", typeof(Brush), typeof(PinButton), new UIPropertyMetadata(null));
            PinModeProperty = DependencyProperty.Register("PinMode", typeof(PinButtonMode), typeof(PinButton), new UIPropertyMetadata(PinButtonMode.Normal));
            IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(PinButton), new UIPropertyMetadata(false));
        }

    }
}
