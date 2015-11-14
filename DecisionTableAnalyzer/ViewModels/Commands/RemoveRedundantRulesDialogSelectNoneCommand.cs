using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class RemoveRedundantRulesDialogSelectNoneCommand : ViewModelCommand<RemoveRedundantRulesDialogModel>
    {
        public override bool CanExecute(RemoveRedundantRulesDialogModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Rules != null && contextViewModel.Rules.Count > 0;
        }

        public override void Execute(RemoveRedundantRulesDialogModel contextViewModel)
        {
            contextViewModel.Rules.ForEach(cur => cur.IsSelected = false);
        }
    }
}
