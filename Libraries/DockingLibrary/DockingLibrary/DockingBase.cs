using System.Windows.Controls;
using System.Windows;
using System;
using System.ComponentModel;
using HelperLibrary;
using System.Windows.Markup;
using System.Diagnostics;
using FunctionalTreeLibrary;

namespace DockingLibrary
{

    public abstract class DockingBase : Control, INotifyPropertyChanged, IFunctionalTreeElement, IDockingElement
    {

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public static readonly FunctionalEvent ClosedEvent;

        #endregion

        #region Functional Properties

        public static readonly FunctionalProperty DockManagerProperty;

        public DockManager DockManager
        {
            get { return (DockManager)this.GetFunctionalValue(DockManagerProperty); }
            set { this.SetFunctionalValue(DockManagerProperty, value); }
        }

        #endregion

        #region Dependency Properties

        internal static readonly DependencyProperty IsEmptyProperty;
        public static readonly DependencyProperty DockingWidthProperty;
        public static readonly DependencyProperty DockingHeightProperty;

        public GridLength DockingWidth
        {
            get { return (GridLength)GetValue(DockingWidthProperty); }
            set { SetValue(DockingWidthProperty, value); }
        }

        public GridLength DockingHeight
        {
            get { return (GridLength)GetValue(DockingHeightProperty); }
            set { SetValue(DockingHeightProperty, value); }
        }

        internal bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            set { SetValue(IsEmptyProperty, value); }
        }

        #endregion

        #region Property Handler

        private static void DockManagerChangedHandler(IFunctionalTreeElement element, FunctionalPropertyChangedEventArgs e)
        {
            DockingBase dockingElement = element as DockingBase;
            if (dockingElement != null && e.NewValue != e.OldValue)
                dockingElement.OnDockManagerChanged(e.OldValue as DockManager, e.NewValue as DockManager);
        }

        #endregion

        #region Properties

        public IDockingContentElement ParentContent
        {
            get
            {
                return FunctionalTreeHelper.GetFunctionalParent(this) as IDockingContentElement;
            }
        }

        #endregion

        #region Variables

        public string Id { get; private set; }

        #endregion

        static DockingBase()
        {
            //Register Dependency Properties
            IsEmptyProperty = DependencyProperty.Register("IsEmpty", typeof(bool), typeof(DockingBase), new UIPropertyMetadata(false));
            DockingWidthProperty = DependencyProperty.Register("DockingWidth", typeof(GridLength), typeof(DockingBase), new UIPropertyMetadata(new GridLengthConverter().ConvertFrom("*")));
            DockingHeightProperty = DependencyProperty.Register("DockingHeight", typeof(GridLength), typeof(DockingBase), new UIPropertyMetadata(new GridLengthConverter().ConvertFrom("*")));

            //Register Functional Properties
            DockManagerProperty = FunctionalProperty.Register("DockManager", typeof(DockManager), typeof(DockingBase),
                new FunctionalPropertyMetadata(null, FunctionalPropertyMetadataOptions.Inherits, DockManagerChangedHandler));

            //Register Events
            ClosedEvent = FunctionalEventManager.RegisterEvent("Closed", FunctionalStrategy.Bubble, typeof(FunctionalEventHandler), typeof(DockingBase));
        }

        public DockingBase()
        {
            Id = Guid.NewGuid().ToString();

            this.AddAttachedToFunctionalTreeHandler(tree =>
            {
                tree.FunctionalChildAttached += FunctionalTree_FunctionalChildAttached;
                tree.FunctionalChildDetached += FunctionalTree_FunctionalChildDetached;
            });
            this.AddDetachedFromFunctionalTreeHandler(tree =>
            {
                tree.FunctionalChildAttached -= FunctionalTree_FunctionalChildAttached;
                tree.FunctionalChildDetached -= FunctionalTree_FunctionalChildDetached;
            });

            this.AddFunctionalHandler(ClosedEvent, new FunctionalEventHandler(OnDockingContentClosedInternal));
        }

        private void FunctionalTree_FunctionalChildAttached(IFunctionalTreeElement child, IFunctionalTreeElement parent, FunctionalTree functionalTree)
        {
            if (child == this)
                return;

            IDockingElement dockingChild = child as IDockingElement;
            if (dockingChild != null)
                OnDockingContentAttached(dockingChild);
        }

        private void FunctionalTree_FunctionalChildDetached(IFunctionalTreeElement child, IFunctionalTreeElement parent, FunctionalTree functionalTree)
        {
            if (child == this)
                return;

            IDockingElement dockingChild = child as IDockingElement;
            if (dockingChild != null)
                OnDockingContentDetached(dockingChild);
        }

        private void OnDockingContentClosedInternal(IFunctionalTreeElement sender, FunctionalEventArgs e)
        {
            Debug.Assert(e.Source is DockingBase, "Source is not an DockingBase.");
            if (e.Source is DockingBase)
                OnDockingDescendentClosed((DockingBase)e.Source);
        }

        public void Close()
        {
            CancelEventArgs args = new CancelEventArgs();
            OnClosing(args);

            if (!args.Cancel)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(ClosedEvent));
        }

        #region Virtuals

        protected virtual void OnDockManagerChanged(DockManager oldDockManager, DockManager newDockManager)
        { }

        protected virtual void OnClosing(CancelEventArgs e)
        { }

        protected virtual void OnDockingContentAttached(IDockingElement item)
        { }

        protected virtual void OnDockingContentDetached(IDockingElement item)
        { }

        public virtual void OnDockingDescendentClosed(DockingBase item)
        { }

        #endregion

        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

    }
}
