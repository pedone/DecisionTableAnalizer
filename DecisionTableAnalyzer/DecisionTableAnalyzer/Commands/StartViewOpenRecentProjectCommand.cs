using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class StartViewOpenRecentProjectCommand : DTCommand<RecentProjectLink>
    {
        public override bool CanExecute(RecentProjectLink contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(RecentProjectLink contextViewModel)
        {
            MainWindow mainWindow = App.Current.MainWindow as MainWindow;
            ApplicationViewModel applicationViewModel = mainWindow.ApplicationViewModel;

            if (File.Exists(contextViewModel.Filename))
            {
                DTProject loadedProject = new DTProject();
                if (loadedProject.Load(contextViewModel.Filename))
                    applicationViewModel.SetProject(loadedProject);
            }
            else
            {
                if (MessageBox.Show(string.Format("\"{0}\" could not be opened.\nWould you like to remove the Reference Link to this project?", contextViewModel.Name),
                    "Decision Table Analyzer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    List<string> recentProjects = Properties.Settings.Default.RecentProjects.Cast<string>().ToList();
                    string recentProjectString = recentProjects.FirstOrDefault(cur => cur.EndsWith(contextViewModel.ProjectId));
                    var recentProjectIndex = recentProjects.IndexOf(recentProjectString);
                    if (recentProjectIndex >= 0)
                    {
                        Properties.Settings.Default.RecentProjects.RemoveAt(recentProjectIndex);
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }
    }
}
