using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DockingLibrary
{

    public class DockTarget : ContentControl
    {

        #region Dependency Properties

        #region AdornerBehavior
        public DockTargetAdornerBehavior AdornerBehavior
        {
            get { return (DockTargetAdornerBehavior)GetValue(AdornerBehaviorProperty); }
            set { SetValue(AdornerBehaviorProperty, value); }
        }
        public static readonly DependencyProperty AdornerBehaviorProperty =
            DependencyProperty.Register("AdornerBehavior", typeof(DockTargetAdornerBehavior), typeof(DockTarget), new UIPropertyMetadata(DockTargetAdornerBehavior.InnerSmall));
        #endregion

        #endregion

        #region Constructor
        static DockTarget()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockTarget), new FrameworkPropertyMetadata(typeof(DockTarget)));
        }

        public DockTarget()
        {
            //AddHandler(DockTarget.MouseEnterEvent,new RoutedEventHandler(DockTarget_MouseEnter), true);
            //AddHandler(DockTarget.MouseLeaveEvent, new RoutedEventHandler(DockTarget_MouseLeave), true);
        }
        #endregion

        #region DockTarget_MouseEnter
        void DockTarget_MouseEnter(object sender, RoutedEventArgs e)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer.Add(new DockTargetAdorner(this, AdornerBehavior));
        }
        #endregion

        #region DockTarget_MouseLeave
        void DockTarget_MouseLeave(object sender, RoutedEventArgs e)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            Adorner[] usedAdorners = adornerLayer.GetAdorners(this);
            if (usedAdorners != null)
                foreach (var adorner in usedAdorners)
                    if (adorner is DockTargetAdorner)
                        adornerLayer.Remove(adorner);
        }
        #endregion

    }
}
