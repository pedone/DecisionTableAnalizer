using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class PrintRequirementsDialogCheckUncheckElement : ViewModelCommand<PrintDecisionTablesDialogModel>
    {
        public override bool CanExecute(PrintDecisionTablesDialogModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(PrintDecisionTablesDialogModel contextViewModel)
        {
            var selectedElement = contextViewModel.SelectedElement;
            if (selectedElement == null)
                return;

            if (selectedElement.IsSelected)
            {
                CheckViewModelsToTop(contextViewModel, selectedElement);
                CheckSubViewModels(selectedElement);
            }
            else
                UncheckSubViewModels(selectedElement);
        }

        private void UncheckSubViewModels(ViewModel element)
        {
            var subElements = GetSubViewModels(element);
            foreach (var viewModel in subElements)
                viewModel.IsSelected = false;
        }

        private void CheckSubViewModels(ViewModel element)
        {
            var subElements = GetSubViewModels(element);
            foreach (var viewModel in subElements)
                viewModel.IsSelected = true;
        }

        private IEnumerable<ViewModel> GetSubViewModels(ViewModel element)
        {
            if (element is SystemActionViewModel || element is SystemConditionViewModel)
            {
                return Enumerable.Empty<ViewModel>();
            }

            List<ViewModel> viewModels = new List<ViewModel>();
            if (element is PrintDecisionTablesDialogModel)
            {
                var contextViewModel = (PrintDecisionTablesDialogModel)element;
                viewModels.AddRange(contextViewModel.DecisionTables);
                foreach (var table in contextViewModel.DecisionTables)
                    viewModels.AddRange(GetSubViewModels(table));
            }
            else if (element is SystemDecisionTableManagerViewModel)
            {
                var decisionTableManagerViewModel = (SystemDecisionTableManagerViewModel)element;
                viewModels.AddRange(decisionTableManagerViewModel.DecisionTables);
                foreach (var table in decisionTableManagerViewModel.DecisionTables)
                    viewModels.AddRange(GetSubViewModels(table));
            }
            else if (element is SystemDecisionTableViewModel)
            {
                var decisionTableViewModel = (SystemDecisionTableViewModel)element;
                viewModels.AddRange(decisionTableViewModel.Conditions);
                viewModels.AddRange(decisionTableViewModel.Actions);
                viewModels.AddRange(decisionTableViewModel.SubTables);
                foreach (var table in decisionTableViewModel.SubTables)
                    viewModels.AddRange(GetSubViewModels(table));
            }

            return viewModels;
        }

        private void CheckViewModelsToTop(PrintDecisionTablesDialogModel contextViewModel, ViewModel element)
        {
            var viewModelsToTop = GetViewModelsToTop(contextViewModel, element);
            foreach (var viewModel in viewModelsToTop)
                viewModel.IsSelected = true;
        }

        private List<ViewModel> GetViewModelsToTop(PrintDecisionTablesDialogModel contextViewModel, ViewModel element)
        {
            List<ViewModel> viewModels = new List<ViewModel>();
            if (element == contextViewModel)
                return viewModels;

            viewModels.Add(contextViewModel);
            if (!contextViewModel.DecisionTables.Contains(element))
            {
                foreach (var table in contextViewModel.DecisionTables)
                {
                    if (table.Actions.Contains(element) || table.Conditions.Contains(element))
                    {
                        viewModels.Add(table);
                        return viewModels;
                    }
                    else
                    {
                        var subViewModels = GetViewModelsToTop(table, element);
                        if (subViewModels.Count() > 0)
                        {
                            viewModels.Add(table);
                            viewModels.AddRange(subViewModels);
                            return viewModels;
                        }
                    }
                }
            }

            return viewModels;
        }

        private IEnumerable<ViewModel> GetViewModelsToTop(SystemDecisionTableViewModel decisionTable, ViewModel element)
        {
            List<ViewModel> viewModels = new List<ViewModel>();
            if (decisionTable.SubTables.Contains(element))
            {
                viewModels.Add(decisionTable);
                return viewModels;
            }

            foreach (var table in decisionTable.SubTables)
            {
                if (table.Actions.Contains(element) || table.Conditions.Contains(element))
                {
                    viewModels.Add(decisionTable);
                    viewModels.Add(table);
                    return viewModels;
                }
                else
                {
                    var subViewModels = GetViewModelsToTop(table, element);
                    if (subViewModels.Count() > 0)
                    {
                        viewModels.Add(decisionTable);
                        viewModels.Add(table);
                        viewModels.AddRange(subViewModels);
                        return viewModels;
                    }
                }
            }

            return Enumerable.Empty<ViewModel>();
        }

    }
}
