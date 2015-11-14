using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;
using DecisionTableAnalyzer.Models;

namespace DecisionTableAnalyzer.Commands
{
    public class DecisionTableViewRefreshTableViewCommand : DTCommand<DecisionTableViewModel>
    {
        public override bool CanExecute(DecisionTableViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(DecisionTableViewModel contextViewModel)
        {
            //Check if refresh is neccessary (subtracting the name column from the tableview)
            //if ((contextViewModel.TableView.Columns.Count - 1) == contextViewModel.DecisionTable.RuleCount)
            //    return;

            //GridView tableView = contextViewModel.TableView;
            //tableView.Columns.Clear();

            //if (contextViewModel.DecisionTable.Elements.Count == 0)
            //    return;

            //tableView.Columns.Add(new GridViewColumn
            //{
            //    //Header = "Name",
            //    Width = 200,
            //    DisplayMemberBinding = new Binding("Name"),
            //});

            //for (int i = 0; i < contextViewModel.DecisionTable.RuleCount; i++)
            //{
            //    GridViewColumn gridColumn = new GridViewColumn
            //    {
            //        Header = i + 1,
            //        Width = double.NaN,
            //        CellTemplateSelector = DTCellTemplateSelector.Instance
            //    };

            //    tableView.Columns.Add(gridColumn);
            //}
        }

    }
}
