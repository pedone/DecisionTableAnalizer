using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewShowDecisionTableDataCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DockManager != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DecisionTableDetailsView dtDataView = contextViewModel.DockManager.ShowView<DecisionTableDetailsView>(activate: true);
            dtDataView.DataContext = new DecisionTableDetailsViewModel(contextViewModel.SelectedItemModel as DecisionTable);
        }
    }
}
