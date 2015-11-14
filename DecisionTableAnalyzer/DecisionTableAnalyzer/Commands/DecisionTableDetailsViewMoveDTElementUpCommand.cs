using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableDetailsViewMoveDTElementUpCommand : DTCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            if (contextViewModel == null || contextViewModel.SelectedElement == null)
                return false;

            var selectedElement = contextViewModel.SelectedElement;
            if (selectedElement.Kind == Models.DTElementKind.Condition)
                return contextViewModel.DecisionTable.Conditions.ToList().IndexOf(selectedElement) > 0;
            else if (selectedElement.Kind == Models.DTElementKind.Action)
                return contextViewModel.DecisionTable.Actions.ToList().IndexOf(selectedElement) > 0;

            return false;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            contextViewModel.DecisionTable.MoveElement(contextViewModel.SelectedElement, MoveDestination.Up);
        }
    }
}
