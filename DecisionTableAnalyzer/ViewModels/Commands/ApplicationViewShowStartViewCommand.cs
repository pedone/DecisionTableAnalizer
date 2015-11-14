using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using DTCore;
using ViewModels;

namespace ViewModels.Commands
{
    public class ApplicationViewShowStartViewCommand : ViewModelCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (!ViewService.Instance.ShowViewExistingView<StartViewModel>())
            {
                StartViewModel viewModel = new StartViewModel { ApplicationViewModel = contextViewModel };
                ViewService.Instance.ShowView(viewModel);
            }
        }
    }
}
