using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewModels.Utils;

namespace ViewModels.Commands
{
    public class ProjectExplorerViewShowDecisionTableCommand : ViewModelCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement is SystemDecisionTableViewModel;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<DecisionTableViewModel>(contextViewModel.SelectedElement.EntityId))
            {
                var decisionTableViewModel = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(contextViewModel.SelectedElement.EntityId);

                if (decisionTableViewModel.Rules.Count == 0)
                {
                    var rules = DecisionTableViewModelUtils.Instance.BuildRules(decisionTableViewModel);
                    if (rules.Count > 0)
                    {
                        string ownerCollection = "Rules";
                        ViewModelService.Instance.InsertViewModels(rules, decisionTableViewModel.EntityId, ownerCollection);
                    }
                }

                ViewService.Instance.ShowView(decisionTableViewModel);
            }
        }
    }
}
