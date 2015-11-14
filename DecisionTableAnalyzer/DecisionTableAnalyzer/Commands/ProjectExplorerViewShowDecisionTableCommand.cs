using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewShowDecisionTableCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DockManager != null && contextViewModel.SelectedItemModel is DecisionTable;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DecisionTableView view = contextViewModel.DockManager.ShowView<DecisionTableView>(activate: true);
            view.DataContext = new DecisionTableViewModel(contextViewModel.SelectedItemModel as DecisionTable);
        }
    }
}
