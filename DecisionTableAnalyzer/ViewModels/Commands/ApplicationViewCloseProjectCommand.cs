using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewCloseProjectCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            contextViewModel.CloseProject();
        }
    }
}
