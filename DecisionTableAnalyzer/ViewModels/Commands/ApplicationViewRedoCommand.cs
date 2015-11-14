using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewRedoCommand : ViewModelCommand<ApplicationViewModel>
    {

        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return HistoryService.Instance.CanRedo;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            HistoryService.Instance.Redo();
        }

    }
}
