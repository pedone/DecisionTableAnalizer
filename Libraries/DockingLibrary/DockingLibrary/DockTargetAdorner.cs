using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Input;

namespace DockingLibrary
{
    internal class DockTargetAdorner : Adorner
    {

        #region Variables

        private Control _Child;
        public Control Child
        {
            get { return _Child; }
            set
            {
                if (_Child != null)
                    RemoveVisualChild(_Child);
                
                _Child = value;
                if (_Child != null)
                    AddVisualChild(_Child);
            }
        }
        public DockTargetAdornerBehavior Behaviour { get; set; }

        #endregion

        #region Constructor
        public DockTargetAdorner(UIElement adornedElement, DockTargetAdornerBehavior behavior)
            : base(adornedElement)
        {
            Behaviour = behavior;

            IsHitTestVisible = false;
            Mouse.Capture(this);
            //Panel.SetZIndex(this, 10);
            Child = new DockTargetVisualizerSmall();
        }
        #endregion

        #region VisualChildrenCount
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }
        #endregion

        #region GetVisualChild
        protected override Visual GetVisualChild(int index)
        {
            return Child;
        }
        #endregion

        #region OnRender
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            if (Behaviour == DockTargetAdornerBehavior.InnerSmall)
            {
                
                //Point center = new Point(RenderSize.Width * 0.5, RenderSize.Height * 0.5);
                //Rect targetRect = new Rect(center.X - _DockTargetSmallBrush.Width * 0.5,
                //                        center.Y - _DockTargetSmallBrush.Height * 0.5,
                //                        _DockTargetSmallBrush.Width,
                //                        _DockTargetSmallBrush.Height);
                //drawingContext.DrawImage(_DockTargetSmallBrush, targetRect);
            //}
            //if (Behaviour == DockTargetAdornerBehavior.Tab)
            //{
                Rect outerRect = new Rect(AdornedElement.RenderSize);
                Rect innerRect = new Rect
                {
                    X = outerRect.X + 6,
                    Y = outerRect.Y + 6,
                    Width = outerRect.Width - 12,
                    Height = outerRect.Height - 12
                };

                SolidColorBrush outerBrush = new SolidColorBrush(Colors.Gray) { Opacity = 0.5 };
                SolidColorBrush innerBrush = new SolidColorBrush(Colors.AliceBlue) { Opacity = 0.5 };

                drawingContext.DrawRoundedRectangle(outerBrush, null, outerRect, 2, 2);
                drawingContext.DrawRectangle(innerBrush, null, innerRect);
            //base.OnRender();
            }
        }
        #endregion

        #region MeasureOverride
        protected override Size MeasureOverride(Size constraint)
        {
            _Child.Measure(constraint);
            return _Child.DesiredSize;
        }
        #endregion

        protected override Size ArrangeOverride(Size finalSize)
        {
            _Child.Arrange(new Rect(new Point(100, 100), finalSize));
            return new Size(_Child.ActualWidth, _Child.ActualHeight);
        }

    }
}
