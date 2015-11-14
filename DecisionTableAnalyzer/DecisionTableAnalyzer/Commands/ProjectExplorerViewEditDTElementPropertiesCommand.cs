using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Models;
using DecisionTableAnalyzer.Dialogs;
using System.Windows;

namespace DecisionTableAnalyzer.Commands
{
    public class ProjectExplorerViewEditDTElementPropertiesCommand : DTCommand<ProjectExplorerViewModel>
    {
        public override bool CanExecute(ProjectExplorerViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.SelectedItemModel is DTElement;
        }

        public override void Execute(ProjectExplorerViewModel contextViewModel)
        {
            var selectedElement = contextViewModel.SelectedItemModel as DTElement;

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
                DecisionTable parentTable = contextViewModel.Project.DecisionTableManager.DecisionTables.FirstOrDefault(cur => cur.Elements.Contains(selectedElement));
                if (parentTable != null && dialogModel.Kind == DTElementKind.Action && parentTable.Actions.Except(new[] { selectedElement }).Any(cur => cur.Name == dialogModel.Name))
                {
                    MessageBox.Show("An action with that name already exists.", "Error");
                }
                else if (parentTable != null && dialogModel.Kind == DTElementKind.Condition && parentTable.Conditions.Except(new[] { selectedElement }).Any(cur => cur.Name == dialogModel.Name))
                {
                    MessageBox.Show("A condition with that name already exists.", "Error");
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
