using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewShowDecisionTableManagerViewCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null && contextViewModel.DockManager != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DecisionTableManagerView dtManagerView = contextViewModel.DockManager.ShowView<DecisionTableManagerView>(activate: true);
            dtManagerView.DataContext = new DecisionTableManagerViewModel(contextViewModel.Project.DecisionTableManager, contextViewModel.DockManager);
        }
    }
}
