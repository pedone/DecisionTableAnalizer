using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableDetailsViewEditElementCommand : DTCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedElement != null;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            var selectedElement = contextViewModel.SelectedElement;
            DTElementDialogModel dialogModel = new DTElementDialogModel
            {
                Name = selectedElement.Name,
                Kind = selectedElement.Kind,
                Description = selectedElement.Description,
            };
            DTElementDialog dialog = new DTElementDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow,
            };

            if (dialog.ShowDialog() == true)
            {
                if (dialogModel.Kind == DTElementKind.Action && contextViewModel.DecisionTable.Actions.Except(new[] { selectedElement }).Any(cur => cur.Name == dialogModel.Name))
                {
                    MessageBox.Show("An action with that name already exists.", "Error");
                }
                else if (dialogModel.Kind == DTElementKind.Condition && contextViewModel.DecisionTable.Conditions.Except(new[] { selectedElement }).Any(cur => cur.Name == dialogModel.Name))
                {
                    MessageBox.Show("An condition with that name already exists.", "Error");
                }
                else
                {
                    selectedElement.Name = dialogModel.Name;
                    selectedElement.Description = dialogModel.Description;
                    selectedElement.Kind = dialogModel.Kind;
                }
            }
        }
    }
}
