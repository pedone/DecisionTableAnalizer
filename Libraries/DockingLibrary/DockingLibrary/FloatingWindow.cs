using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FunctionalTreeLibrary;

namespace DockingLibrary
{
    public class FloatingWindow : Window, IFunctionalTreeElement, IDockingElement
    {

        /// <param name="positionTargetX">Target position relative to the sizeTarget</param>
        public FloatingWindow(double positionTargetX, FrameworkElement sizeTarget)
        {
            ShowInTaskbar = false;
            ShowActivated = true;
            WindowStartupLocation = WindowStartupLocation.Manual;

            SetupPositionAndSize(positionTargetX, sizeTarget);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            IFunctionalTreeElement oldChildElement = oldContent as IFunctionalTreeElement;
            if (oldChildElement != null)
                this.RemoveFunctionalChild(oldChildElement);

            IFunctionalTreeElement newChildElement = newContent as IFunctionalTreeElement;
            if (newChildElement != null)
                this.AddFunctionalChild(newChildElement);
        }

        private void SetupPositionAndSize(double positionTargetX, FrameworkElement positionAndSizeBase)
        {
            Width = positionAndSizeBase.ActualWidth;
            Height = positionAndSizeBase.ActualHeight;

            Point mousePosition = Mouse.GetPosition(positionAndSizeBase);
            double windowOffsetX = positionTargetX - mousePosition.X;

            //TODO calculate value
            int windowHeaderCenterY = 7;
            Point targetPosition = positionAndSizeBase.PointToScreen(new Point(-windowOffsetX, mousePosition.Y - windowHeaderCenterY));

            Left = targetPosition.X;
            Top = targetPosition.Y;
        }


        public DockManager DockManager
        {
            get
            {
                return this.GetFunctionalValue(DockingBase.DockManagerProperty) as DockManager;
            }
            set
            {
                this.SetFunctionalValue(DockingBase.DockManagerProperty, value);
            }
        }

        public IDockingContentElement ParentContent
        {
            get
            {
                return FunctionalTreeHelper.GetFunctionalParent(this) as IDockingContentElement;
            }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
    }
}
