using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;

namespace DecisionTableAnalyzer.Commands
{
    public class RequirementManagerViewRemoveRequirementCommand : DTCommand<RequirementManagerViewModel>
    {
        public override bool CanExecute(RequirementManagerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedRequirement != null;
        }

        public override void Execute(RequirementManagerViewModel contextViewModel)
        {
            contextViewModel.RequirementManager.Remove(contextViewModel.SelectedRequirement);
        }
    }
}
