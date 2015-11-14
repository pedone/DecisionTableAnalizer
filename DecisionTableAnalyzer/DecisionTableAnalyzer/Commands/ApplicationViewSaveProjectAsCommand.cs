using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.Windows.Input;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewSaveProjectAsCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = contextViewModel.Project.Name.Replace(' ', '_'),
                AddExtension = true,
                DefaultExt = "dta",
                Filter = "DecisionTable Project|*.dta"
            };

            if (dialog.ShowDialog() == true)
            {
                contextViewModel.Project.Save(dialog.FileName);
            }
        }
    }
}
