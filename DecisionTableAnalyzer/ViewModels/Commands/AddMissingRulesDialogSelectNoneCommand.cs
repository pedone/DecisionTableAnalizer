using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class AddMissingRulesDialogSelectNoneCommand : ViewModelCommand<AddMissingRulesDialogModel>
    {
        public override bool CanExecute(AddMissingRulesDialogModel contextViewModel)
        {
            return contextViewModel != null &&
                contextViewModel.Rules.Any(cur => cur.IsSelected);
        }

        public override void Execute(AddMissingRulesDialogModel contextViewModel)
        {
            contextViewModel.Rules.ForEach(cur => cur.IsSelected = false);
        }
    }
}
