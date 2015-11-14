using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewDeleteDecisionTableCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedItemModel is DecisionTable;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DecisionTable selectedTable = contextViewModel.SelectedItemModel as DecisionTable;
            contextViewModel.Project.DecisionTableManager.Remove(selectedTable);
        }
    }
}
