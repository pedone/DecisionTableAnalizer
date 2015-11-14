using System;
using System.Windows.Controls;
using System.Windows;
using HelperLibrary;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;
using FunctionalTreeLibrary;

namespace DockingLibrary
{
    /// <summary>
    /// Interaction logic for AutoHideChannel.xaml
    /// </summary>
    [TemplatePart(Name = "PART_WindowResizeSplitter", Type = typeof(GridSplitter))]
    [TemplatePart(Name = "PART_WindowContainer", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_AutoHidePinButton", Type = typeof(PinButton))]
    [TemplatePart(Name = "PART_ClosePinButton", Type = typeof(PinButton))]
    public class AutoHideChannel : DockingContentBase
    {

        #region Dependency Properties

        internal static readonly DependencyPropertyKey ChannelWidthProperty;
        internal static readonly DependencyPropertyKey ChannelHeightProperty;
        public static readonly DependencyPropertyKey CurrentWindowViewProperty;

        internal double ChannelWidth
        {
            get { return (double)GetValue(ChannelWidthProperty.DependencyProperty); }
            private set { SetValue(ChannelWidthProperty, value); }
        }

        internal double ChannelHeight
        {
            get { return (double)GetValue(ChannelHeightProperty.DependencyProperty); }
            private set { SetValue(ChannelHeightProperty, value); }
        }

        public View CurrentWindowView
        {
            get { return (View)GetValue(CurrentWindowViewProperty.DependencyProperty); }
            private set { SetValue(CurrentWindowViewProperty, value); }
        }

        #endregion

        #region Variables

        private ObservableCollection<AutoHideChannelItem> _items;
        public ReadOnlyObservableCollection<AutoHideChannelItem> Items { get; private set; }

        private Dock _DockDirection = Dock.Right;
        public Dock DockDirection
        {
            get { return _DockDirection; }
            set
            {
                if (_DockDirection != value)
                {
                    _DockDirection = value;
                    NotifyPropertyChanged("DockDirection");

                    UpdateChannelSize();
                }
            }
        }

        private static readonly double DefaultChannelSize = 25d;
        private static readonly double EmptyChannelSize = 5d;

        private DispatcherTimer _showWindowTimer;
        private DispatcherTimer _hideWindowTimer;

        private List<ViewGroup> _viewGroups;
        public ReadOnlyCollection<ViewGroup> ViewGroups { get; private set; }

        private GridSplitter WindowResizeSplitterPart { get; set; }
        private UIElement WindowContainerPart { get; set; }
        private PinButton AutoHidePinButtonPart { get; set; }
        private PinButton ClosePinButtonPart { get; set; }

        #endregion

        #region Properties

        public bool IsWindowOpen
        {
            get
            {
                return WindowContainerPart != null && WindowContainerPart.IsVisible &&
                    WindowResizeSplitterPart != null && WindowResizeSplitterPart.IsVisible;
            }
        }

        private bool IsMouseOverWindow
        {
            get
            {
                return WindowContainerPart != null && WindowResizeSplitterPart != null &&
                (WindowContainerPart.IsMouseOver || WindowResizeSplitterPart.IsMouseOver);
            }
        }

        private bool IsMouseOverAutoHideChannelItem
        {
            get
            {
                return Items.Any(cur => cur.IsMouseOver);
            }
        }

        #endregion

        static AutoHideChannel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoHideChannel), new FrameworkPropertyMetadata(typeof(AutoHideChannel)));

            //Register Dependency Properties
            ChannelWidthProperty = DependencyProperty.RegisterReadOnly("ChannelWidth", typeof(double), typeof(AutoHideChannel), new UIPropertyMetadata(0d));
            ChannelHeightProperty = DependencyProperty.RegisterReadOnly("ChannelHeight", typeof(double), typeof(AutoHideChannel), new UIPropertyMetadata(0d));
            CurrentWindowViewProperty = DependencyProperty.RegisterReadOnly("CurrentWindowView", typeof(View), typeof(AutoHideChannel), new UIPropertyMetadata(null));
        }

        public AutoHideChannel()
        {
            AutoClose = false;

            _viewGroups = new List<ViewGroup>();
            ViewGroups = new ReadOnlyCollection<ViewGroup>(_viewGroups);

            _items = new ObservableCollection<AutoHideChannelItem>();
            Items = new ReadOnlyObservableCollection<AutoHideChannelItem>(_items);

            UpdateChannelSize();
            SetupWindowTimer();

            KeyDown += AutoHideChannel_KeyDown;
        }

        private void AutoHideChannel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && CurrentWindowView != null && CurrentWindowView.IsActive)
                DockManager.ResetActiveViewToCenterContent();
        }

        public override void OnApplyTemplate()
        {
            MouseEventHandler updateWindowAction = (s, e) => UpdateWindowVisibility();

            if (WindowContainerPart != null)
            {
                WindowContainerPart.MouseEnter -= updateWindowAction;
                WindowContainerPart.MouseLeave -= updateWindowAction;
                WindowContainerPart.MouseDown -= WindowContainerPart_MouseDown;
            }
            if (WindowResizeSplitterPart != null)
            {
                WindowResizeSplitterPart.MouseLeave -= updateWindowAction;
                WindowResizeSplitterPart.MouseEnter -= updateWindowAction;
            }
            if (AutoHidePinButtonPart != null)
            {
                AutoHidePinButtonPart.Click -= AutoHidePinButtonPart_Click;
            }
            if (ClosePinButtonPart != null)
            {
                ClosePinButtonPart.Click -= ClosePinButtonPart_Click;
            }

            WindowResizeSplitterPart = Template.FindName("PART_WindowResizeSplitter", this) as GridSplitter;
            WindowContainerPart = Template.FindName("PART_WindowContainer", this) as UIElement;
            AutoHidePinButtonPart = Template.FindName("PART_AutoHidePinButton", this) as PinButton;
            ClosePinButtonPart = Template.FindName("PART_ClosePinButton", this) as PinButton;

            Debug.Assert(WindowResizeSplitterPart != null, "PART_WindowResizeSplitter must be defined.");
            Debug.Assert(WindowContainerPart != null, "PART_WindowContainer must be defined.");
            Debug.Assert(AutoHidePinButtonPart != null, "PART_AutoHidePinButton must be defined.");
            Debug.Assert(ClosePinButtonPart != null, "PART_ClosePinButton must be defined.");

            if (WindowContainerPart != null)
            {
                WindowContainerPart.MouseEnter += updateWindowAction;
                WindowContainerPart.MouseLeave += updateWindowAction;
                WindowContainerPart.MouseDown += WindowContainerPart_MouseDown;
            }
            if (WindowResizeSplitterPart != null)
            {
                WindowResizeSplitterPart.MouseLeave += updateWindowAction;
                WindowResizeSplitterPart.MouseEnter += updateWindowAction;
            }
            if (AutoHidePinButtonPart != null)
            {
                AutoHidePinButtonPart.Click += AutoHidePinButtonPart_Click;
            }
            if (ClosePinButtonPart != null)
            {
                ClosePinButtonPart.Click += ClosePinButtonPart_Click;
            }

            HideWindowWithoutDelay();
        }

        private void WindowContainerPart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentWindowView != null)
                CurrentWindowView.Activate();
        }

        private void ClosePinButtonPart_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentWindowView != null)
                CurrentWindowView.Hide();

            HideWindowWithoutDelay();
            UpdateChannelSize();
        }

        private void AutoHidePinButtonPart_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentWindowView != null)
            {
                DockManager.LockActiveView();

                UnloadViewGroup(CurrentWindowView.ViewGroup);
                CurrentWindowView.ViewGroup.SetDockState(DockState.Dock);
                CurrentWindowView.Show();

                DockManager.UnlockActiveView();
            }

            HideWindowWithoutDelay();
            UpdateChannelSize();
        }

        private void SetupWindowTimer()
        {
            if (_showWindowTimer == null)
            {
                _showWindowTimer = new DispatcherTimer();
                _showWindowTimer.Tick += ShowWindowTimer_Tick;
            }
            if (_hideWindowTimer == null)
            {
                _hideWindowTimer = new DispatcherTimer();
                _hideWindowTimer.Tick += HideWindowTimer_Tick;
            }

            _showWindowTimer.Interval = TimeSpan.FromMilliseconds(500);
            _hideWindowTimer.Interval = TimeSpan.FromMilliseconds(500);
        }

        private void UpdateChannelSize()
        {
            if (DockDirection == Dock.Right || DockDirection == Dock.Left)
            {
                if (Items.Count > 0)
                    ChannelWidth = DefaultChannelSize;
                else
                    ChannelWidth = EmptyChannelSize;
            }
            else
            {
                if (Items.Count > 0)
                    ChannelHeight = DefaultChannelSize;
                else
                    ChannelHeight = EmptyChannelSize;
            }
        }

        protected override void OnDockManagerChanged(DockManager oldDockManager, DockManager newDockManager)
        {
            if (oldDockManager != null)
                oldDockManager.ActiveViewChanged -= DockManager_ActiveViewChanged;
            if (newDockManager != null)
                newDockManager.ActiveViewChanged += DockManager_ActiveViewChanged;
        }

        private void DockManager_ActiveViewChanged(object sender, ValueChangedEventArgs<DockManager, View> e)
        {
            if (e.NewValue != CurrentWindowView && IsWindowOpen && !IsMouseOverWindow && !IsMouseOverAutoHideChannelItem)
                HideWindowWithoutDelay();
        }

        public void Add(ViewGroup viewGroup)
        {
            if (ViewGroups.Contains(viewGroup))
                return;

            _viewGroups.Add(viewGroup);
            viewGroup.DockStateChanging += ViewGroup_DockStateChanging;
            viewGroup.DockStateChanged += ViewGroup_DockStateChanged;
            viewGroup.ViewDockStateChanging += ViewGroup_ViewDockStateChanging;
            viewGroup.ViewDockStateChanged += ViewGroup_ViewDockStateChanged;

            if (viewGroup.DockState == DockState.AutoHide)
                LoadViewGroup(viewGroup);
        }

        private void Add(View view)
        {
            if (!Items.Any(cur => cur.View == view))
            {
                this.AddFunctionalChild(view);

                AutoHideChannelItem autoHideItem = new AutoHideChannelItem(view);
                autoHideItem.IsMouseOverChanged += AutoHideItem_IsMouseOverChanged;
                autoHideItem.PreviewMouseDown += AutoHideItem_PreviewMouseDown;

                _items.Add(autoHideItem);
                UpdateChannelSize();
            }
        }

        public void Remove(ViewGroup viewGroup)
        {
            if (!ViewGroups.Contains(viewGroup))
                return;

            viewGroup.DockStateChanging -= ViewGroup_DockStateChanging;
            viewGroup.DockStateChanged -= ViewGroup_DockStateChanged;
            viewGroup.ViewDockStateChanging -= ViewGroup_ViewDockStateChanging;
            viewGroup.ViewDockStateChanged -= ViewGroup_ViewDockStateChanged;

            if (viewGroup.DockState == DockState.AutoHide)
                UnloadViewGroup(viewGroup);

            _viewGroups.Remove(viewGroup);
            UpdateChannelSize();
        }

        private void Remove(View view)
        {
            this.RemoveFunctionalChild(view);

            AutoHideChannelItem autoHideItem = Items.FirstOrDefault(cur => cur.View == view);
            if (autoHideItem != null)
            {
                autoHideItem.IsMouseOverChanged -= AutoHideItem_IsMouseOverChanged;
                autoHideItem.PreviewMouseDown -= AutoHideItem_PreviewMouseDown;
                _items.Remove(autoHideItem);
            }
        }

        private void LoadViewGroup(ViewGroup viewGroup)
        {
            foreach (View view in viewGroup.Views)
            {
                if (view.DockState == DockState.AutoHide)
                    Add(view);
            }

            UpdateChannelSize();
        }

        private void UnloadViewGroup(ViewGroup viewGroup)
        {
            foreach (View view in viewGroup.Views)
                Remove(view);

            UpdateChannelSize();
        }

        private void ViewGroup_ViewDockStateChanging(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (e.NewValue != DockState.AutoHide)
                Remove(e.Source);
        }

        private void ViewGroup_ViewDockStateChanged(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (e.NewValue == DockState.AutoHide)
                Add(e.Source);
        }

        private void ViewGroup_DockStateChanging(object sender, ValueChangedEventArgs<ViewGroup, DockState> e)
        {
            if (e.NewValue != DockState.AutoHide)
                UnloadViewGroup(e.Source);
        }

        private void ViewGroup_DockStateChanged(object sender, ValueChangedEventArgs<ViewGroup, DockState> e)
        {
            if (e.NewValue == DockState.AutoHide)
                LoadViewGroup(e.Source);
        }

        private void AutoHideItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            AutoHideChannelItem item = (AutoHideChannelItem)sender;
            item.View.Activate();
        }

        private void AutoHideItem_IsMouseOverChanged(object sender, ValueChangedEventArgs<AutoHideChannelItem, bool> e)
        {
            if (e.NewValue == true)
                ShowWindowWithDelay(e.Source.View);
            else if (CurrentWindowView != null && CurrentWindowView.IsActive)
            {
                AbortHidingWindow();
                //Abort view change
                if (_showWindowTimer.IsEnabled)
                    _showWindowTimer.Stop();
            }
            else
                HideWindowWithDelay();
        }

        private void UpdateWindowVisibility()
        {
            if (IsMouseOverWindow)
                AbortHidingWindow();
            else if (CurrentWindowView == null || !CurrentWindowView.IsActive)
                HideWindowWithDelay();
        }

        private void AbortHidingWindow()
        {
            if (_hideWindowTimer.IsEnabled)
                _hideWindowTimer.Stop();
        }

        private void ShowWindowWithDelay(View viewToShow)
        {
            AbortHidingWindow();

            if (_showWindowTimer.IsEnabled)
                _showWindowTimer.Stop();

            _showWindowTimer.Tag = viewToShow;
            _showWindowTimer.Start();
        }

        private void HideWindowWithDelay()
        {
            if (_showWindowTimer.IsEnabled)
                _showWindowTimer.Stop();

            AbortHidingWindow();

            if (IsWindowOpen)
                _hideWindowTimer.Start();
        }

        private void ShowWindowWithoutDelay(View viewToShow)
        {
            AbortHidingWindow();

            CurrentWindowView = viewToShow;

            if (!IsWindowOpen)
            {
                WindowContainerPart.Visibility = Visibility.Visible;
                WindowResizeSplitterPart.Visibility = Visibility.Visible;
            }

            if (_showWindowTimer.IsEnabled)
                _showWindowTimer.Stop();
        }

        private void HideWindowWithoutDelay()
        {
            WindowContainerPart.Visibility = Visibility.Collapsed;
            WindowResizeSplitterPart.Visibility = Visibility.Collapsed;

            CurrentWindowView = null;

            if (_hideWindowTimer.IsEnabled)
                _hideWindowTimer.Stop();
        }

        private void ShowWindowTimer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            ShowWindowWithoutDelay((View)timer.Tag);
        }

        private void HideWindowTimer_Tick(object sender, EventArgs e)
        {
            HideWindowWithoutDelay();
            _hideWindowTimer.Stop();
        }

        public void Show(View view)
        {
            if (Items.Any(cur => cur.View == view) && CurrentWindowView != view)
                ShowWindowWithoutDelay(view);
        }

        protected override bool IsChild(DockingBase item)
        {
            if (item == null)
                return false;

            return Items.Any(cur => cur.View == item);
        }
        
        protected override bool ReplaceItemInternal(DockingBase oldItem, DockingBase newItem)
        {
            //TODO decide
            //throw new NotSupportedException();
            return false;
        }

        protected override bool RemoveInternal(DockingBase item)
        {
            //TODO decide
            //throw new NotSupportedException();
            return false;
        }

    }
}
