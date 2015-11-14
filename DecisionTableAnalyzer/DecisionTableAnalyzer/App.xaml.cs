using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DecisionTableAnalyzer.Properties;
using DTCore;
using ViewModels;
using System.Reflection;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DecisionTableAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public ApplicationViewModel ApplicationViewModel { get; private set; }

        new public static App Current
        {
            get { return Application.Current as App; }
        }

        static App()
        {
            ViewModelService.Instance.Init();
        }

        public App()
        {
            ApplicationViewModel = new ApplicationViewModel();
            ApplicationViewModel.ProjectChanged += ApplicationViewModel_ProjectChanged;
            InitSettings();

            //Load core assemblies
            Assembly.Load(new AssemblyName("DTServices"));
        }

        private void ApplicationViewModel_ProjectChanged(ProjectViewModel oldProject, ProjectViewModel newProject)
        {
            if (oldProject != null)
                oldProject.PropertyChanged -= (sender, e) => Project_PropertyChanged(sender, e, oldProject);
            if (newProject != null)
                newProject.PropertyChanged += (sender, e) => Project_PropertyChanged(sender, e, newProject);
        }

        private void Project_PropertyChanged(object sender, PropertyChangedEventArgs e, ProjectViewModel project)
        {
            if (e.PropertyName == "Name" || e.PropertyName == "Filename")
            {
                //Update the recent project string for the project
                List<string> recentProjects = Settings.Default.RecentProjects.Cast<string>().ToList();
                string oldRecentProjectString = recentProjects.FirstOrDefault(cur => cur.EndsWith(project.EntityId.Id));
                string newRecentProjectString = string.Format("{0}|{1}|{2}", project.Name, project.Filename, project.EntityId.Id);

                int oldProjectIndex = recentProjects.IndexOf(oldRecentProjectString);
                Settings.Default.RecentProjects.RemoveAt(oldProjectIndex);
                Settings.Default.RecentProjects.Insert(oldProjectIndex, newRecentProjectString);
                Settings.Default.Save();
            }
        }

        private void InitSettings()
        {
            var settings = ApplicationViewModel.Settings;

            //Init settings
            settings.CloseStartPageAfterProjectLoad = Settings.Default.CloseStartPageAfterProjectLoad;
            settings.ShowStartPageOnStartup = Settings.Default.ShowStartPageOnStartup;
            settings.ViewAfterProjectLoad = Settings.Default.ViewAfterProjectLoad;

            if (Settings.Default.RecentProjects == null)
                Settings.Default.RecentProjects = new StringCollection();

            settings.AddRecentProjects(ReadRecentProjects(Settings.Default.RecentProjects).Reverse());

            //Bind settings
            settings.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var settings = ApplicationViewModel.Settings;
            if (e.PropertyName == "CloseStartPageAfterProjectLoad")
                Settings.Default.CloseStartPageAfterProjectLoad = settings.CloseStartPageAfterProjectLoad;
            else if (e.PropertyName == "ShowStartPageOnStartup")
                Settings.Default.ShowStartPageOnStartup = settings.ShowStartPageOnStartup;
            else if (e.PropertyName == "ViewAfterProjectLoad")
                Settings.Default.ViewAfterProjectLoad = settings.ViewAfterProjectLoad;
            else if (e.PropertyName == "RecentProjects")
            {
                Settings.Default.RecentProjects.Clear();
                Settings.Default.RecentProjects.AddRange(WriteRecentProjects(settings.RecentProjects));
            }
            else
                return;

             Settings.Default.Save();
        }

        private IEnumerable<RecentProjectViewModel> ReadRecentProjects(StringCollection recentProjects)
        {
            foreach (var project in recentProjects)
            {
                var projectData = project.Split('|');
                if (projectData.Count() != 3 || string.IsNullOrEmpty(projectData[1]))
                    continue;

                yield return new RecentProjectViewModel
                {
                    ApplicationViewModel = ApplicationViewModel,
                    Name = projectData[0],
                    Filename = projectData[1],
                    Id = projectData[2]
                };
            }
        }

        private string[] WriteRecentProjects(IEnumerable<RecentProjectViewModel> recentProjects)
        {
            return recentProjects.Select(cur => string.Format("{0}|{1}|{2}", cur.Name, cur.Filename, cur.Id)).ToArray();
        }

    }
}
