using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels;

namespace ViewModels.Commands
{
    public class RequirementManagerViewRemoveRequirementCommand : ViewModelCommand<RequirementManagerViewModel>
    {
        public override bool CanExecute(RequirementManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedRequirement != null;
        }

        public override void Execute(RequirementManagerViewModel contextViewModel)
        {
            HistoryService.Instance.BeginSession();
            ViewModelService.Instance.DeleteViewModel(contextViewModel.SelectedRequirement);
            HistoryService.Instance.EndSession();
        }
    }
}
