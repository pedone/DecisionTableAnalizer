using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewShowProjectExplorerViewCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null && contextViewModel.DockManager != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            contextViewModel.DockManager.ShowView<ProjectExplorerView>(activate: true);
        }
    }
}
