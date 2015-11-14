using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using System.Windows.Input;
using ViewModels.Commands;
using DockingLibrary;
using System.Windows;

namespace ViewModels
{

    public delegate void ProjectChangedHandler(ProjectViewModel oldProject, ProjectViewModel newProject);

    public class ApplicationViewModel : ViewModel
    {

        public event ProjectChangedHandler ProjectChanged;

        public DockManager DockManager { get; set; }
        public SettingsViewModel Settings { get; private set; }
        public StatusViewModel Status { get; private set; }

        private ProjectViewModel _CurrentProject;
        public ProjectViewModel CurrentProject
        {
            get { return _CurrentProject; }
            set
            {
                _CurrentProject = value;
                NotifyPropertyChanged<ProjectViewModel>(() => CurrentProject);
            }
        }

        public bool IsProjectLoaded
        {
            get { return CurrentProject != null; }
        }

        public ApplicationViewModel()
        {
            Settings = new SettingsViewModel();
            Status = new StatusViewModel();
        }

        public void CloseProject()
        {
            if (!IsProjectLoaded)
                return;

            string serviceId = "DTServices.CommonServices";
            string operationId = "UnloadEntities";
            ViewModelService.Instance.ExecuteOperation(serviceId, operationId);

            CurrentProject = null;

            //Unload all views except for the start view
            DockManager.GetViews(cur => !(cur.DataContext is StartViewModel)).ToList().ForEach(cur => cur.Close());

            //set the dataContext of the project explorer manually to null, because it didn't close, because autohide panes are only hidden
            var projectExplorerView = DockManager.GetViews(cur=>cur.DataContext is ProjectExplorerViewModel).FirstOrDefault();
            if (projectExplorerView != null)
                projectExplorerView.DataContext = null;
        }

        public void InitProject(EntityId projectId)
        {
            if (projectId == null)
                throw new ArgumentNullException("projectId", "projectId is null.");

            var newProject = ViewModelService.Instance.QueryViewModel<ProjectViewModel>(projectId);
            if (ProjectChanged != null)
                ProjectChanged(CurrentProject, newProject);

            CurrentProject = newProject;
            //Unload all views
            if (Settings.CloseStartPageAfterProjectLoad)
                DockManager.Views.ToList().ForEach(cur => cur.Close());
            else
                DockManager.GetViews(cur => !(cur.DataContext is StartViewModel)).ToList().ForEach(cur => cur.Close());

            SaveAsRecentProject(projectId);

            //Open next view
            if (Settings.ViewAfterProjectLoad == "Requirement Manager View")
            {
                string serviceId = "DTServices.CommonServices";
                string operationId = "GetRequirementManager";
                var viewModel = ViewModelService.Instance.ExecuteOperation<RequirementManagerViewModel>(serviceId, operationId, projectId);
                ViewService.Instance.ShowView(viewModel);
            }
            else if (Settings.ViewAfterProjectLoad == "Decision Table Manager View")
            {
                string serviceId = "DTServices.CommonServices";
                string operationId = "GetDecisionTableManager";
                var viewModel = ViewModelService.Instance.ExecuteOperation<DecisionTableManagerViewModel>(serviceId, operationId, projectId);
                ViewService.Instance.ShowView(viewModel);
            }

            //Init project explorer
            if (!ViewService.Instance.IsViewVisible<ProjectExplorerViewModel>(projectId))
            {
                var projectExplorerViewModel = ViewModelService.Instance.QueryViewModel<ProjectExplorerViewModel>(projectId);
                ViewService.Instance.ShowView(projectExplorerViewModel, activate: false, dockState: DockState.AutoHide);
            }

            //Init status view
            if (!ViewService.Instance.IsViewVisible<StatusViewModel>())
                ViewService.Instance.ShowView(Status, activate: false, dockState: DockState.AutoHide);
        }

        private void SaveAsRecentProject(EntityId projectId)
        {
            var project = ViewModelService.Instance.QueryViewModel<RecentProjectViewModel>(projectId);
            project.ApplicationViewModel = this;
            Settings.AddRecentProject(project);
        }

    }
}
