using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.Windows.Input;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewShowRequirementManagerViewCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            RequirementManagerView requirementView = contextViewModel.DockManager.ShowView<RequirementManagerView>(activate: true);
            requirementView.DataContext = new RequirementManagerViewModel(contextViewModel.Project.RequirementManager);
        }
    }
}
