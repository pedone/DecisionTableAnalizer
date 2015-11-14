using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewShowStatusViewCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<StatusViewModel>())
                ViewService.Instance.ShowView(contextViewModel.Status);
        }
    }
}
