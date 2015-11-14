using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCore;

namespace ViewModels.Commands
{
    public class PrintRequirementsDialogSelectNoneCommand : ViewModelCommand<PrintRequirementsDialogModel>
    {
        public override bool CanExecute(PrintRequirementsDialogModel contextViewModel)
        {
            return contextViewModel != null &&
                (contextViewModel.FunctionalRequirements.Any(cur => cur.IsSelected) ||
                contextViewModel.NonFunctionalRequirements.Any(cur => cur.IsSelected));
        }

        public override void Execute(PrintRequirementsDialogModel contextViewModel)
        {
            contextViewModel.FunctionalRequirements.ForEach(cur => cur.IsSelected = false);
            contextViewModel.NonFunctionalRequirements.ForEach(cur => cur.IsSelected = false);
        }
    }
}
