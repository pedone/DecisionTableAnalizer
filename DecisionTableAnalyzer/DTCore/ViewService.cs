using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DockingLibrary;

namespace DTCore
{
    public class ViewService
    {

        public static ViewService Instance { get; private set; }
        public DockManager DockManager { get; private set; }

        static ViewService()
        {
            Instance = new ViewService();
        }

        private ViewService()
        { }

        public void Init(DockManager dockManager)
        {
            DockManager = dockManager;
        }

        public bool ShowDialog(ViewModel dialogModel)
        {
            var dialogMapping = Application.Current.TryFindResource(dialogModel.GetType()) as TypeMapping;
            if (dialogMapping == null)
                throw new ArgumentException(string.Format("No dialog found for type '{0}'", dialogModel.GetType()));
            if (!(typeof(Window).IsAssignableFrom(dialogMapping.Type)))
                throw new ArgumentException(string.Format("Invalid dialog found for type '{0}'. Dialog must inherit from 'System.Window'", dialogMapping.Type));

            var dialogWindow = Activator.CreateInstance(dialogMapping.Type) as Window;
            dialogWindow.DataContext = dialogModel;
            dialogWindow.Owner = Application.Current.MainWindow;

            return (bool)dialogWindow.ShowDialog();
        }

        public bool IsViewVisible<DataContextType>()
        {
            return IsViewVisible(dataContext => dataContext is DataContextType);
        }

        public bool IsViewVisible<DataContextType>(EntityId dataContextEntityId)
        {
            return IsViewVisible(dataContext => dataContext is DataContextType && dataContext.EntityId.Equals(dataContextEntityId));
        }

        /// <summary>
        /// True, if the view is currently being shown. That does not imply it's active or anything.
        /// </summary>
        public bool IsViewVisible(Predicate<ViewModel> predicate)
        {
            if (predicate == null)
                return false;

            var existingView = DockManager.GetViews(cur =>
            {
                var viewModel = cur.DataContext as ViewModel;
                return viewModel != null && predicate(viewModel);
            }).FirstOrDefault();

            return existingView != null;
        }

        public bool ShowViewExistingView<DataContextType>()
        {
            return ShowViewExistingView(dataContext => dataContext is DataContextType);
        }

        public bool ShowViewExistingView<DataContextType>(EntityId dataContextEntityId)
        {
            return ShowViewExistingView(dataContext => dataContext is DataContextType && dataContext.EntityId.Equals(dataContextEntityId));
        }

        /// <summary>
        /// Shows the first view with the given entityId
        /// </summary>
        public bool ShowViewExistingView(Predicate<ViewModel> predicate)
        {
            if (predicate == null)
                return false;

            var existingView = DockManager.GetViews(cur =>
                {
                    var viewModel = cur.DataContext as ViewModel;
                    return viewModel != null && predicate(viewModel);
                }).FirstOrDefault();

            if (existingView != null)
            {
                existingView.Show();
                return true;
            }

            return false;
        }

        public void ShowView(ViewModel viewModel, bool activate = true, DockState? dockState = null)
        {
            var viewMapping = Application.Current.TryFindResource(viewModel.GetType()) as TypeMapping;
            if (viewMapping == null)
                throw new ArgumentException(string.Format("No view found for type '{0}'", viewModel.GetType()));
            if (!(typeof(View).IsAssignableFrom(viewMapping.Type)))
                throw new ArgumentException(string.Format("Invalid view found for type '{0}'. View must inherit from 'System.Window'", viewMapping.Type));

            var view = DockManager.CreateNewView(viewMapping.Type);
            if (view == null)
                view = DockManager.GetViews(viewMapping.Type).FirstOrDefault();

            if (view != null)
            {
                view.DataContext = viewModel;
                view.Show();
                if (activate)
                    view.Activate();
                if (dockState != null)
                    view.ViewGroup.SetDockState((DockState)dockState);
            }
        }

    }
}
