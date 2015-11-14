using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using System.Diagnostics;
using FunctionalTreeLibrary;

namespace DockingLibrary
{

    [TemplatePart(Name = "PART_LeftAutoHideChannel", Type = typeof(AutoHideChannel))]
    [TemplatePart(Name = "PART_TopAutoHideChannel", Type = typeof(AutoHideChannel))]
    [TemplatePart(Name = "PART_RightAutoHideChannel", Type = typeof(AutoHideChannel))]
    [TemplatePart(Name = "PART_BottomAutoHideChannel", Type = typeof(AutoHideChannel))]
    public class DockManager : DockingContentControl<DockingBase>
    {

        #region Events

        public event ValueChangedEventHandler<DockManager, View> ActiveViewChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty CenterContentProperty;

        public DocumentGroupContainer CenterContent
        {
            get { return (DocumentGroupContainer)GetValue(CenterContentProperty); }
            set { SetValue(CenterContentProperty, value); }
        }

        #endregion

        #region Property Handlers

        private static void CenterContentChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DockManager)d).UpdateTemporaryAutoHideViewGroups();
        }

        #endregion

        #region Variables

        public View ActiveView { get; private set; }
        private bool _isActiveViewLocked;

        private List<View> _viewsInternal;
        public ReadOnlyCollection<View> Views { get; private set; }

        private List<DockingBase> _dockingContentsInternal;
        public ReadOnlyCollection<DockingBase> DockingContents { get; private set; }

        private List<FloatingWindow> _floatingWindowsInternal;
        public ReadOnlyCollection<FloatingWindow> FloatingWindows { get; private set; }

        private AutoHideChannel LeftAutoHideChannelPart { get; set; }
        private AutoHideChannel TopAutoHideChannelPart { get; set; }
        private AutoHideChannel RightAutoHideChannelPart { get; set; }
        private AutoHideChannel BottomAutoHideChannelPart { get; set; }

        /// <summary>
        /// Since some ViewGroups are registered before the template was applied, there are no AutoHideChannels yet,
        /// making it impossible to add the viewGroups to them.
        /// They are saved in this list temporarily and added to the AutoHideChannels as soon as they're available.
        /// </summary>
        private List<ViewGroup> _temporaryAutoHideViewGroupList;

        #endregion

        #region Properties

        /// <summary>
        /// True, if everything is initialized to add a ViewGroup to an AutoHideChannel.
        /// </summary>
        private bool IsAutoHideChannelReady
        {
            get
            {
                return RightAutoHideChannelPart != null && LeftAutoHideChannelPart != null && TopAutoHideChannelPart != null && BottomAutoHideChannelPart != null &&
                    //The centerContent needs to be initialized, because the dockingRoute/DockDirection of the viewGroup will be computed using the CenterContent
                    CenterContent != null;
            }
        }

        #endregion

        static DockManager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockManager), new FrameworkPropertyMetadata(typeof(DockManager)));

            //Register Dependency Properties
            CenterContentProperty = DependencyProperty.Register("CenterContent", typeof(DocumentGroupContainer), typeof(DockManager), new UIPropertyMetadata(null, CenterContentChangedHandler));
        }

        public DockManager()
        {
            DockManager = this;

            _viewsInternal = new List<View>();
            Views = new ReadOnlyCollection<View>(_viewsInternal);

            _dockingContentsInternal = new List<DockingBase>();
            DockingContents = new ReadOnlyCollection<DockingBase>(_dockingContentsInternal);

            _floatingWindowsInternal = new List<FloatingWindow>();
            FloatingWindows = new ReadOnlyCollection<FloatingWindow>(_floatingWindowsInternal);

            _temporaryAutoHideViewGroupList = new List<ViewGroup>();
        }

        //BMK TODO
        private void InsertIntoDockingTree(View view, ExtendedDockDirection dockDirection)
        {
            Debug.Assert(CenterContent != null, "CenterContent is null.");
            Debug.Assert(view != null, "view is null.");
            Debug.Assert(view.IsNew, "view is not new.");
            Debug.Assert(dockDirection != ExtendedDockDirection.None, "dockDirection is none.");

            if (dockDirection == ExtendedDockDirection.Inner)
            {
                CenterContent.Content.Add(view);
            }
            else if (dockDirection == ExtendedDockDirection.Left ||
                dockDirection == ExtendedDockDirection.Top ||
                dockDirection == ExtendedDockDirection.Right ||
                dockDirection == ExtendedDockDirection.Bottom)
            {
                IDockingContentElement centerContentParent = CenterContent.ParentContent;
                if (centerContentParent != null)
                {
                    DockGroup newDockGroup = new DockGroup();
                    if (dockDirection == ExtendedDockDirection.Bottom || dockDirection == ExtendedDockDirection.Top)
                        newDockGroup.Orientation = Orientation.Vertical;

                    if (centerContentParent.ReplaceItem(CenterContent, newDockGroup))
                    {
                        TabGroup tabGroup = new TabGroup();
                        tabGroup.Add(view);

                        if (dockDirection == ExtendedDockDirection.Bottom || dockDirection == ExtendedDockDirection.Right)
                        {
                            newDockGroup.First = CenterContent;
                            newDockGroup.Last = tabGroup;
                        }
                        else if (dockDirection == ExtendedDockDirection.Top || dockDirection == ExtendedDockDirection.Left)
                        {
                            newDockGroup.First = tabGroup;
                            newDockGroup.Last = CenterContent;
                        }
                    }
                }
            }

            //TODO
            
            //IDockingContainer lastParent = _viewContainer.ContainsKey(view.GetType()) ?
            //    FindDockingContentChild<DockingContent>(Content, _viewContainer[view.GetType()]) as IDockingContainer : null;
            //if (lastParent != null)
            //    lastParent.Items.Insert(0, view);
            //// Inner Dock
            //else if (view.DockDirection == ExtendedDock.Inner)
            //{
            //    DocumentGroup firstDocumentGroup = Helper.FindVisualChild<DocumentGroup>(CenterContent);
            //    if (firstDocumentGroup != null)
            //    {
            //        firstDocumentGroup.Items.Insert(0, view);
            //        view.Show();
            //    }
            //    else if (CenterContent.Content == null)
            //        CenterContent.Content = new DocumentGroup { Items = new ObservableCollection<DockingContent> { view } };
            //}
            //else
            //    InsertViewGroupIntoDockingTree(new List<View> { view });
        }

        public override void OnApplyTemplate()
        {
            //Remove from functional tree
            if (LeftAutoHideChannelPart != null)
                this.RemoveFunctionalChild(LeftAutoHideChannelPart);
            if (TopAutoHideChannelPart != null)
                this.RemoveFunctionalChild(TopAutoHideChannelPart);
            if (RightAutoHideChannelPart != null)
                this.RemoveFunctionalChild(RightAutoHideChannelPart);
            if (BottomAutoHideChannelPart != null)
                this.RemoveFunctionalChild(BottomAutoHideChannelPart);

            //Get new parts
            LeftAutoHideChannelPart = Template.FindName("PART_LeftAutoHideChannel", this) as AutoHideChannel;
            TopAutoHideChannelPart = Template.FindName("PART_TopAutoHideChannel", this) as AutoHideChannel;
            RightAutoHideChannelPart = Template.FindName("PART_RightAutoHideChannel", this) as AutoHideChannel;
            BottomAutoHideChannelPart = Template.FindName("PART_BottomAutoHideChannel", this) as AutoHideChannel;

            Debug.Assert(LeftAutoHideChannelPart != null, "PART_LeftAutoHideChannel must be defined.");
            Debug.Assert(TopAutoHideChannelPart != null, "PART_TopAutoHideChannel must be defined.");
            Debug.Assert(RightAutoHideChannelPart != null, "PART_RightAutoHideChannel must be defined.");
            Debug.Assert(BottomAutoHideChannelPart != null, "PART_BottomAutoHideChannel must be defined.");

            //Add to functional tree
            if (LeftAutoHideChannelPart != null)
                this.AddFunctionalChild(LeftAutoHideChannelPart);
            if (TopAutoHideChannelPart != null)
                this.AddFunctionalChild(TopAutoHideChannelPart);
            if (RightAutoHideChannelPart != null)
                this.AddFunctionalChild(RightAutoHideChannelPart);
            if (BottomAutoHideChannelPart != null)
                this.AddFunctionalChild(BottomAutoHideChannelPart);
        }

        private void UpdateTemporaryAutoHideViewGroups()
        {
            if (_temporaryAutoHideViewGroupList.Count > 0 && IsAutoHideChannelReady)
            {
                foreach (ViewGroup group in _temporaryAutoHideViewGroupList)
                    AddAutoHideViewGroup(group);

                _temporaryAutoHideViewGroupList.Clear();
            }
        }

        private void AddAutoHideViewGroup(ViewGroup viewGroup)
        {
            if (!viewGroup.CanAutoHide)
                return;

            if (!IsAutoHideChannelReady)
            {
                _temporaryAutoHideViewGroupList.Add(viewGroup);
                return;
            }

            if (viewGroup.DockDirection == DockDirection.Left)
                LeftAutoHideChannelPart.Add(viewGroup);
            else if (viewGroup.DockDirection == DockDirection.Up)
                TopAutoHideChannelPart.Add(viewGroup);
            else if (viewGroup.DockDirection == DockDirection.Right)
                RightAutoHideChannelPart.Add(viewGroup);
            else if (viewGroup.DockDirection == DockDirection.Down)
                BottomAutoHideChannelPart.Add(viewGroup);
        }

        protected override void OnDockingContentAttached(IDockingElement item)
        {
            if (item is DockingBase)
            {
                DockingBase dockingItem = item as DockingBase;
                if (DockingContents.Contains(dockingItem))
                    return;

                _dockingContentsInternal.Add(dockingItem);

                if (dockingItem is View)
                    _viewsInternal.Add((View)dockingItem);
                else if (dockingItem is TabGroup)
                {
                    ViewGroup viewGroup = ((TabGroup)dockingItem).ViewGroup;
                    viewGroup.ViewDockStateChanging += ViewGroup_ViewDockStateChanging;
                    viewGroup.ViewDockStateChanged += ViewGroup_ViewDockStateChanged;
                    if (viewGroup.CanAutoHide)
                        AddAutoHideViewGroup(viewGroup);
                }
            }
            else if (item is FloatingWindow)
            {
                FloatingWindow dockingWindow = item as FloatingWindow;
                if (FloatingWindows.Contains(dockingWindow))
                    return;

                _floatingWindowsInternal.Add(dockingWindow);
            }

            //Add all children of the added item too
            foreach (var child in FunctionalTreeHelper.GetFunctionalChildren(item))
            {
                if (child is DockingBase)
                    OnDockingContentAttached(child as DockingBase);
            }
        }

        private void ViewGroup_ViewDockStateChanged(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (e.NewValue == DockState.Hide)
                this.AddFunctionalChild(e.Source);
        }

        private void ViewGroup_ViewDockStateChanging(object sender, ValueChangedEventArgs<View, DockState> e)
        {
            if (e.NewValue != DockState.Hide)
                this.RemoveFunctionalChild(e.Source);
        }

        protected override void OnDockingContentDetached(IDockingElement item)
        {
            _dockingContentsInternal.RemoveAll(cur => cur.Id.Equals(item.Id));
            if (item is View)
            {
                _viewsInternal.RemoveAll(cur => cur.Id.Equals(item.Id));
                if (ActiveView == item)
                    SetActiveView(null);
            }
        }
        
        internal DockDirection CalculateDockDirection(DockingBase dockingElement)
        {
            if (CenterContent == null)
                throw new ArgumentNullException("CenterContent", "CenterContent is null.");
            if (dockingElement == null)
                throw new ArgumentNullException("dockingElement", "dockingElement is null.");
            if (!DockingContents.Contains(dockingElement))
                throw new ArgumentException("dockingElement is not child of dockManager.");
            if (CenterContent == dockingElement)
                throw new ArgumentException("dockingElement can not be the CenterContent.");

            DockingRoute elementRoute = CalculateDockingRoute(dockingElement);
            DockingRoute centerContentRoute = CalculateDockingRoute(CenterContent);

            for (int i = 0; i < elementRoute.RouteLength; i++)
            {
                DockDirection elementDirection = elementRoute.GetDirection(i);
                DockDirection centerContentDirection = centerContentRoute.GetDirection(i);
                if (elementDirection == DockDirection.None || centerContentDirection == DockDirection.None)
                    continue;

                if (elementDirection != centerContentDirection)
                    return elementDirection;
            }

            //Because this will often fail in designer mode, commented for now
            //Debug.Fail("No DockDirection found.");
            Debug.WriteLine("No DockDirection found.");

            return DockDirection.None;
        }

        private DockingRoute CalculateDockingRoute(IDockingElement dockingElement)
        {
            DockingRoute route = new DockingRoute();

            IDockingElement curElement = dockingElement;
            IDockingElement curParent = dockingElement.ParentContent;
            while (curParent != null)
            {
                DockGroup parentAsGroup = curParent as DockGroup;
                if (parentAsGroup != null)
                {
                    if (parentAsGroup.Orientation == Orientation.Vertical && parentAsGroup.First == curElement)
                        route.Add(DockDirection.Up);
                    else if (parentAsGroup.Orientation == Orientation.Vertical && parentAsGroup.Last == curElement)
                        route.Add(DockDirection.Down);
                    else if (parentAsGroup.Orientation == Orientation.Horizontal && parentAsGroup.First == curElement)
                        route.Add(DockDirection.Left);
                    else if (parentAsGroup.Orientation == Orientation.Horizontal && parentAsGroup.Last == curElement)
                        route.Add(DockDirection.Right);
                }

                curElement = curElement.ParentContent;
                curParent = curElement.ParentContent;
            }

            return route;
        }

        public View ShowView(Func<View, bool> predicate, bool activate = false)
        {
            return ShowView<View>(predicate, activate);
        }

        public ViewType ShowView<ViewType>(Func<ViewType, bool> predicate = null, bool activate = false)
            where ViewType : View, new()
        {
            ViewType view = GetOrCreateView<ViewType>(predicate);

            if (activate)
                view.Activate();
            else
            {
                LockActiveView();
                view.Show();
                UnlockActiveView();
            }

            return view;
        }

        public View GetOrCreateView(Func<View, bool> predicate)
        {
            return GetOrCreateView<View>(predicate);
        }

        public T GetOrCreateView<T>(Func<T, bool> predicate = null)
            where T : View, new()
        {
            T match;
            if (predicate != null)
                match = Views.FirstOrDefault(cur => cur is T && predicate((T)cur)) as T;
            else
                match = Views.FirstOrDefault(cur => cur is T) as T;

            if (match != null)
                return match;

            return CreateNewView<T>();
        }

        public View GetOrCreateView(Type viewType, Func<View, bool> predicate = null)
        {
            View match;
            if (predicate != null)
                match = Views.FirstOrDefault(cur => cur.GetType() == viewType && predicate(cur));
            else
                match = Views.FirstOrDefault(cur => cur.GetType() == viewType);

            if (match != null)
                return match;

            return CreateNewView(viewType);
        }

        /// <summary>
        /// Creates a new View if the assigned ViewUsage allows it.
        /// </summary>
        public T CreateNewView<T>()
            where T : View, new()
        {
            return CreateNewView(typeof(T)) as T;
        }

        public View CreateNewView(Type viewType)
        {
            if (viewType == typeof(View))
                return new View();

            int maxViewCount = 1;
            ViewUsageAttribute viewUsage = Attribute.GetCustomAttribute(viewType, typeof(ViewUsageAttribute)) as ViewUsageAttribute;
            if (viewUsage != null && viewUsage.ViewUsage == ViewUsage.Multiple)
            {
                if (viewUsage.MaximumInstanceCount > 1)
                    maxViewCount = viewUsage.MaximumInstanceCount;
                else
                    //Unlimited Views
                    maxViewCount = 0;
            }

            bool isNewViewAllowed = (maxViewCount == 0 || GetViews(viewType).Count() < maxViewCount);
            if (isNewViewAllowed)
            {
                var newView = Activator.CreateInstance(viewType) as View;
                ExtendedDockDirection viewDockDirection = viewUsage != null ? viewUsage.DefaultDockDirection : ExtendedDockDirection.Right;
                InsertIntoDockingTree(newView, viewDockDirection);
                return newView;
            }

            return null;
        }

        public IEnumerable<View> GetViews(Func<View, bool> predicate)
        {
            return GetViews<View>(predicate);
        }

        public IEnumerable<T> GetViews<T>(Func<T, bool> predicate = null)
            where T : View, new()
        {
            if (predicate != null)
                return Views.Where(cur => cur is T && predicate((T)cur)).Cast<T>();
            else
                return Views.Where(cur => cur is T).Cast<T>();
        }

        public IEnumerable<View> GetViews(Type viewType, Func<View, bool> predicate = null)
        {
            if (predicate != null)
                return Views.Where(cur => cur.GetType() == viewType && predicate(cur));
            else
                return Views.Where(cur => cur.GetType() == viewType);
        }

        public DockingBase GetDockingContent(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException("id is null or empty.", "id");

            return GetDockingContent<DockingBase>(cur => cur.Id.Equals(id));
        }

        public DockingBase GetDockingContent(Func<DockingBase, bool> predicate)
        {
            return GetDockingContent<DockingBase>(predicate);
        }

        public T GetDockingContent<T>(Func<T, bool> predicate)
            where T : DockingBase
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate", "predicate is null.");

            return DockingContents.FirstOrDefault(cur => cur is T && predicate((T)cur)) as T;
        }

        public IEnumerable<DockingBase> GetDockingContents(Func<DockingBase, bool> predicate)
        {
            return GetDockingContents<DockingBase>(predicate);
        }

        public IEnumerable<T> GetDockingContents<T>(Func<T, bool> predicate)
            where T : DockingBase
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate", "predicate is null.");

            return DockingContents.Where(cur => cur is T && predicate((T)cur)).Cast<T>();
        }

        internal void SetActiveView(View view)
        {
            if (_isActiveViewLocked)
                return;

            View oldActiveView = ActiveView;
            ActiveView = view;

            if (oldActiveView != null)
                oldActiveView.NotifyDeactivated();
            if (ActiveView != null)
                ActiveView.NotifyActivated();

            if (ActiveViewChanged != null)
                ActiveViewChanged(this, new ValueChangedEventArgs<DockManager, View>(this, oldActiveView, ActiveView));
        }

        /// <summary>
        /// Resets the active view to the first document view in the center content, except if a content
        /// in the conter content is already activated
        /// </summary>
        public void ResetActiveViewToCenterContent(bool resetToNullIfUnable = false)
        {
            if (CenterContent == null)
                return;

            bool isActivatedContentInCenterContent = false;
            if (ActiveView != null)
            {
                IDockingElement curContent = ActiveView;
                while (curContent != null && !isActivatedContentInCenterContent)
                {
                    if (curContent == CenterContent)
                        isActivatedContentInCenterContent = true;

                    curContent = curContent.ParentContent;
                }
            }
            if (isActivatedContentInCenterContent)
                return;

            //TODO anpassen wenn CenterContent nicht mehr aus einzelner DocumentGroup gesteht
            if (CenterContent.Content != null)
                CenterContent.Content.Activate();
            else if (resetToNullIfUnable)
                SetActiveView(null);
        }

        /// <summary>
        /// Locks the active view in order to prevent changes while an operation is in progress.
        /// This should always be called together with UnlockActiveView() in the same context in order
        /// to avoid locking the active view forever.
        /// </summary>
        public void LockActiveView()
        {
            _isActiveViewLocked = true;
        }

        public void UnlockActiveView()
        {
            _isActiveViewLocked = false;
        }

    }
}
