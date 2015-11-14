using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using System.Collections.ObjectModel;

namespace ViewModels
{

    public class SettingsViewModel : ViewModel
    {

        private bool _CloseStartPageAfterProjectLoad;
        public bool CloseStartPageAfterProjectLoad
        {
            get { return _CloseStartPageAfterProjectLoad; }
            set
            {
                if (_CloseStartPageAfterProjectLoad != value)
                {
                    _CloseStartPageAfterProjectLoad = value;
                    NotifyPropertyChanged<bool>(() => CloseStartPageAfterProjectLoad);
                }
            }
        }

        private bool _ShowStartPageOnStartup;
        public bool ShowStartPageOnStartup
        {
            get { return _ShowStartPageOnStartup; }
            set
            {
                if (_ShowStartPageOnStartup != value)
                {
                    _ShowStartPageOnStartup = value;
                    NotifyPropertyChanged<bool>(() => ShowStartPageOnStartup);
                }
            }
        }

        private string _ViewAfterProjectLoad;
        public string ViewAfterProjectLoad
        {
            get { return _ViewAfterProjectLoad; }
            set
            {
                if (_ViewAfterProjectLoad != value)
                {
                    _ViewAfterProjectLoad = value;
                    NotifyPropertyChanged<string>(() => ViewAfterProjectLoad);
                }
            }
        }

        private ObservableCollection<RecentProjectViewModel> _RecentProjectsInternal;
        private ReadOnlyObservableCollection<RecentProjectViewModel> _RecentProjects;
        public ReadOnlyObservableCollection<RecentProjectViewModel> RecentProjects
        {
            get { return _RecentProjects; }
            set
            {
                if (_RecentProjects != value)
                {
                    _RecentProjects = value;
                    NotifyPropertyChanged<ReadOnlyObservableCollection<RecentProjectViewModel>>(() => RecentProjects);
                }
            }
        }

        public SettingsViewModel()
        {
            _RecentProjectsInternal = new ObservableCollection<RecentProjectViewModel>();
            _RecentProjects = new ReadOnlyObservableCollection<RecentProjectViewModel>(_RecentProjectsInternal);
        }

        public void AddRecentProject(RecentProjectViewModel project)
        {
            if (!RecentProjects.Any(cur => cur.Id == project.Id))
            {
                _RecentProjectsInternal.Insert(0, project);

                while (RecentProjects.Count > 10)
                    _RecentProjectsInternal.RemoveAt(10);
            }
            else
            {
                var existingProject = RecentProjects.FirstOrDefault(cur => cur.Id == project.Id);
                RecentProjectViewModel replacingProject = existingProject.EntityId != null ? existingProject : project;
                _RecentProjectsInternal.Remove(existingProject);
                _RecentProjectsInternal.Insert(0, replacingProject);
            }

            NotifyPropertyChanged<ReadOnlyCollection<RecentProjectViewModel>>(() => RecentProjects);
        }

        public void AddRecentProjects(IEnumerable<RecentProjectViewModel> projects)
        {
            foreach (var project in projects)
                AddRecentProject(project);
        }

        public void RemoveRecentProject(RecentProjectViewModel project)
        {
            _RecentProjectsInternal.Remove(project);
            NotifyPropertyChanged<ReadOnlyCollection<RecentProjectViewModel>>(() => RecentProjects);
        }

    }
}
