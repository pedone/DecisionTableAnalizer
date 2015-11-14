using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using DTCore;

namespace ViewModels.Commands
{
    public class StatusViewClearStatusCommand : ViewModelCommand<StatusViewModel>
    {
        public override bool CanExecute(StatusViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Messages.Count > 0;
        }

        public override void Execute(StatusViewModel contextViewModel)
        {
            contextViewModel.Messages.Clear();
        }
    }
}
