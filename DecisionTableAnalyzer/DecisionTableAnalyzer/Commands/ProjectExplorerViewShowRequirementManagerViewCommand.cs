using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewShowRequirementManagerViewCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.DockManager != null && contextViewModel.Project != null;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            RequirementManagerView requirementView = contextViewModel.DockManager.ShowView<RequirementManagerView>(activate: true);
            requirementView.DataContext = new RequirementManagerViewModel(contextViewModel.Project.RequirementManager);
        }
    }
}
