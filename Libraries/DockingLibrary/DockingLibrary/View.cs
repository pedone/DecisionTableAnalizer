using System;
using System.Windows;
using System.Windows.Media;
using FunctionalTreeLibrary;

namespace DockingLibrary
{
    [ViewUsage(DockingLibrary.ViewUsage.Multiple)]
    public class View : DockingContentControl
    {

        #region Events

        public event ValueChangedEventHandler<View, DockState> DockStateChanged;
        public event ValueChangedEventHandler<View, DockState> DockStateChanging;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty IconProperty;

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        #endregion

        #region Variables

        private ViewGroup _ViewGroup;
        public ViewGroup ViewGroup
        {
            get { return _ViewGroup; }
            internal set
            {
                if (_ViewGroup == value)
                    return;

                if (_ViewGroup != null)
                {
                    _ViewGroup.Remove(this);
                    _ViewGroup.DockStateChanging -= ViewGroup_DockStateChanging;
                    _ViewGroup.DockStateChanged -= ViewGroup_DockStateChanged;
                }

                _ViewGroup = value;

                if (_ViewGroup != null)
                {
                    _ViewGroup.Add(this);
                    _ViewGroup.DockStateChanging += ViewGroup_DockStateChanging;
                    _ViewGroup.DockStateChanged += ViewGroup_DockStateChanged;
                }
            }
        }

        private bool _IsDockingVisible = true;
        private bool IsDockingVisible
        {
            get { return _IsDockingVisible; }
            set
            {
                if (_IsDockingVisible != value)
                {
                    if (value && ViewGroup.DockState == DockingLibrary.DockState.Hide)
                        ViewGroup.RestoreLastDockState();

                    if (_IsDockingVisible)
                        OnDockStateChangingInternal(ViewGroup.DockState, DockState.Hide);
                    else
                        OnDockStateChangingInternal(DockState.Hide, ViewGroup.DockState);

                    _IsDockingVisible = value;
                    NotifyPropertyChanged("DockState");

                    if (_IsDockingVisible)
                        OnDockStateChangedInternal(DockState.Hide, ViewGroup.DockState);
                    else
                        OnDockStateChangedInternal(ViewGroup.DockState, DockState.Hide);
                }
            }
        }

        #endregion

        #region Properties

        public DockState DockState
        {
            get
            {
                if (!IsDockingVisible || ViewGroup == null)
                    return DockState.Hide;

                return ViewGroup.DockState;
            }
        }

        internal bool IsShown
        {
            get
            {
                ISelectingViewContainer parentViewContainer = ParentContent as ISelectingViewContainer;
                if (parentViewContainer != null)
                    return parentViewContainer.SelectedView == this;

                AutoHideChannel parentChannel = ParentContent as AutoHideChannel;
                if (parentChannel != null)
                    return parentChannel.CurrentWindowView == this;

                return false;
            }
        }

        /// <summary>
        /// True, if the view is new and has not yet been assigned to a ViewGroup
        /// </summary>
        public bool IsNew
        {
            get { return ViewGroup == null; }
        }

        public bool IsActive
        {
            get { return DockManager != null && DockManager.ActiveView == this; }
        }

        #endregion

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));

            //Register Dependency Properties
            HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(View), new UIPropertyMetadata(String.Empty));
            IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(View), new UIPropertyMetadata(null));
        }

        public View()
        {
            PreviewMouseDown += View_PreviewMouseDown;
        }

        private void View_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Activate();
        }

        /// <summary>
        /// Shows the view without activating it.
        /// </summary>
        public void Show()
        {
            if (IsShown)
                return;

            if (!IsDockingVisible)
                IsDockingVisible = true;

            ISelectingViewContainer parentViewContainer = ParentContent as ISelectingViewContainer;
            if (parentViewContainer != null)
                parentViewContainer.Show(this);

            AutoHideChannel parentChannel = ParentContent as AutoHideChannel;
            if (parentChannel != null)
                parentChannel.Show(this);

            OnShow();
        }

        public void Hide()
        {
            IsDockingVisible = false;
            if (IsActive)
                DockManager.SetActiveView(null);

            OnHide();
        }

        /// <summary>
        /// Activates and shows the view.
        /// </summary>
        public void Activate()
        {
            if (DockManager != null && !IsActive)
            {
                Show();
                DockManager.SetActiveView(this);
            }
        }

        public void Deactivate()
        {
            if (IsActive)
                DockManager.SetActiveView(null);
        }

        internal void NotifyActivated()
        {
            if (IsActive)
            {
                OnActivated();
                NotifyPropertyChanged("IsActive");
            }
        }

        internal void NotifyDeactivated()
        {
            if (!IsActive)
            {
                OnDeactivated();
                NotifyPropertyChanged("IsActive");
            }
        }

        private void ViewGroup_DockStateChanging(object sender, ValueChangedEventArgs<ViewGroup, DockState> e)
        {
            if (e.NewValue == DockingLibrary.DockState.Hide)
                Hide();

            if (DockState != DockingLibrary.DockState.Hide && e.NewValue != DockingLibrary.DockState.Hide)
                OnDockStateChangingInternal(e.OldValue, e.NewValue);
        }

        private void ViewGroup_DockStateChanged(object sender, ValueChangedEventArgs<ViewGroup, DockState> e)
        {
            if (IsDockingVisible)
                OnDockStateChangedInternal(e.OldValue, e.NewValue);
        }

        private void OnDockStateChangingInternal(DockState oldValue, DockState newValue)
        {
            if (newValue == DockState.Hide || newValue == DockState.AutoHide)
                Deactivate();

            OnDockStateChanged(oldValue, newValue);

            if (DockStateChanging != null)
                DockStateChanging(this, new ValueChangedEventArgs<View, DockState>(this, oldValue, newValue));
        }

        private void OnDockStateChangedInternal(DockState oldValue, DockState newValue)
        {
            OnDockStateChanged(oldValue, newValue);

            if (DockStateChanged != null)
                DockStateChanged(this, new ValueChangedEventArgs<View, DockState>(this, oldValue, newValue));
        }

        #region Virtuals

        public virtual void OnActivated()
        { }

        public virtual void OnDeactivated()
        { }

        public virtual void OnDockStateChanging(DockState oldDockState, DockState newDockState)
        { }

        public virtual void OnDockStateChanged(DockState oldDockState, DockState newDockState)
        { }

        public virtual void OnShow()
        { }

        public virtual void OnHide()
        { }
        
        #endregion

    }
}
