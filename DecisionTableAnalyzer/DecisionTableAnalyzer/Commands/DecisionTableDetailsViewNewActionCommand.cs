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
    public class DecisionTableDetailsViewNewActionCommand : DTCommand<DecisionTableDetailsViewModel>
    {
        public override bool CanExecute(DecisionTableDetailsViewModel contextViewModel)
        {
            return true;
        }

        public override void Execute(DecisionTableDetailsViewModel contextViewModel)
        {
            DTElementDialogModel dialogModel = new DTElementDialogModel { Kind = DTElementKind.Action };
            DTElementDialog dialog = new DTElementDialog
            {
                DataContext = dialogModel,
                Owner = App.Current.MainWindow
            };
            if (dialog.ShowDialog() == true)
            {
                DTElement newElement = new DTElement
                {
                    Name = dialogModel.Name,
                    Kind = dialogModel.Kind,
                    Description = dialogModel.Description
                };

                if (contextViewModel.DecisionTable.Actions.Any(cur => cur.Name == newElement.Name))
                    MessageBox.Show("An action with that name already exists.", "Error");
                else
                    contextViewModel.DecisionTable.Add(newElement);
            }
        }
    }
}
