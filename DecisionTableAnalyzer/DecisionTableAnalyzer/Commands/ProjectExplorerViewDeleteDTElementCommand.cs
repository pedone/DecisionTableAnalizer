using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewDeleteDTElementCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedItemModel is DTElement;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            DTElement selectedElement = contextViewModel.SelectedItemModel as DTElement;
            var parentTable = contextViewModel.Project.DecisionTableManager.DecisionTables.FirstOrDefault(cur => cur.Elements.Contains(selectedElement));
            if (parentTable != null)
                parentTable.Remove(selectedElement);
        }
    }
}
