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
    public class ApplicationViewSaveProjectCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (!contextViewModel.Project.IsFilenameValid || !contextViewModel.Project.Save())
                contextViewModel.SaveProjectAsCommand.Execute(contextViewModel);
        }
    }
}
