using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewOpenProjectCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "DecisionTable Project|*.dta|Xml File|*.xml|All Files|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                DTProject loadedProject = new DTProject();
                if (loadedProject.Load(dialog.FileName))
                    contextViewModel.SetProject(loadedProject);
            }
        }
    }
}
