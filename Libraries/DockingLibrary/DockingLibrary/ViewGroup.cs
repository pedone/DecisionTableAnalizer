using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DockingLibrary
{
    public class ViewGroup
    {

        #region Events

        public event ValueChangedEventHandler<ViewGroup, IEnumerable<View>> ViewsChanged;

        public event ValueChangedEventHandler<View, DockState> ViewDockStateChanging;
        public event ValueChangedEventHandler<View, DockState> ViewDockStateChanged;

        public event ValueChangedEventHandler<ViewGroup, DockState> DockStateChanging;
        public event ValueChangedEventHandler<ViewGroup, DockState> DockStateChanged;

        #endregion

        #region Variables

        private ObservableCollection<View> _viewsInternal;
        public ReadOnlyObservableCollection<View> Views { get; private set; }

        public DockState DockState { get; private set; }
        public DockingBase DockingOwner { get; private set; }

        /// <summary>
        /// The DockStates, that may be activated on this viewGroup.
        /// </summary>
        private DockState _validDockStates;
        private DockState _lastDockState = DockState.AutoHide;

        #endregion

        #region Properties

        public DockManager DockManager
        {
            get
            {
                if (DockingOwner != null)
                    return DockingOwner.DockManager;

                return null;
            }
        }

        public DockDirection DockDirection
        {
            get { return DockManager.CalculateDockDirection(DockingOwner); }
        }

        public bool CanAutoHide
        {
            get { return IsValidDockState(DockingLibrary.DockState.AutoHide); }
        }
        public bool CanDock
        {
            get { return IsValidDockState(DockingLibrary.DockState.Dock); }
        }
        public bool CanFloat
        {
            get { return IsValidDockState(DockingLibrary.DockState.Float); }
        }
        public bool CanHide
        {
            get { return IsValidDockState(DockingLibrary.DockState.Hide); }
        }
        public bool CanTabbedDocument
        {
            get { return IsValidDockState(DockingLibrary.DockState.TabbedDocument); }
        }

        #endregion

        public ViewGroup(DockingBase dockingOwner, DockState visibleOwnerDockState, DockState validDockStates)
        {
            if (dockingOwner == null)
                throw new ArgumentNullException("dockingOwner", "dockingOwner is null.");

            _validDockStates = validDockStates;
            DockingOwner = dockingOwner;
            DockState = visibleOwnerDockState;

            _viewsInternal = new ObservableCollection<View>();
            Views = new ReadOnlyObservableCollection<View>(_viewsInternal);
            _viewsInternal.CollectionChanged += OnViewsChangedInternal;
        }

        public void Add(View view)
        {
            if (!Views.Contains(view))
                _viewsInternal.Add(view);
        }

        public void Remove(View view)
        {
            if (Views.Contains(view))
                _viewsInternal.Remove(view);
        }

        private void OnViewsChangedInternal(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<View> oldViews = e.OldItems != null ? e.OldItems.OfType<View>() : null;
            IEnumerable<View> newViews = e.NewItems != null ? e.NewItems.OfType<View>() : null;

            if (oldViews != null)
            {
                foreach (View view in oldViews)
                {
                    view.DockStateChanging -= View_DockStateChanging;
                    view.DockStateChanged -= View_DockStateChanged;
                    if (view.ViewGroup == this)
                        view.ViewGroup = null;
                }
            }
            if (newViews != null)
            {
                foreach (View view in newViews)
                {
                    view.ViewGroup = this;
                    view.DockStateChanging += View_DockStateChanging;
                    view.DockStateChanged += View_DockStateChanged;
                }
            }

            if (ViewsChanged != null)
                ViewsChanged(this, new ValueChangedEventArgs<ViewGroup, IEnumerable<View>>(this, oldViews, newViews));
        }

        private void View_DockStateChanging(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (ViewDockStateChanging != null)
                ViewDockStateChanging(this, new ValueChangedEventArgs<View, DockState>(e.Source, e.OldValue, e.NewValue));
        }

        private void View_DockStateChanged(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (ViewDockStateChanged != null)
                ViewDockStateChanged(this, new ValueChangedEventArgs<View, DockState>(e.Source, e.OldValue, e.NewValue));

            if (Views.All(cur => cur.DockState == DockState.Hide))
                SetDockState(DockingLibrary.DockState.Hide);
            else if (DockState == DockState.Hide)
                SetDockState(_lastDockState);
        }

        public bool IsValidDockState(DockState state)
        {
            return (_validDockStates & state) == state;
        }

        public void SetDockState(DockState state)
        {
            if (DockState != state && IsValidDockState(state))
            {
                if (DockStateChanging != null)
                    DockStateChanging(this, new ValueChangedEventArgs<ViewGroup, DockState>(this, DockState, state));

                _lastDockState = DockState;
                DockState = state;

                if (DockStateChanged != null)
                    DockStateChanged(this, new ValueChangedEventArgs<ViewGroup, DockState>(this, _lastDockState, DockState));
            }
        }

        public void RestoreLastDockState()
        {
            SetDockState(_lastDockState);
        }

    }
}
