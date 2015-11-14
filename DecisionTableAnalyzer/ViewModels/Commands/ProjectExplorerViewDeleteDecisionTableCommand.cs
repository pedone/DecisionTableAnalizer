using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewDeleteDecisionTableCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemDecisionTableViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();
            ViewModelService.Instance.DeleteViewModel(contextViewModel.SelectedElement);
            HistoryService.Instance.EndSession();
        }
    }
}
