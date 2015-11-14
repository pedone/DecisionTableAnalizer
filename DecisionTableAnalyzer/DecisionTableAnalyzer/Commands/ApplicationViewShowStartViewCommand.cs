using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.Windows.Input;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewShowStartViewCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            StartView startView = contextViewModel.DockManager.ShowView<StartView>(activate: true);
            startView.DataContext = new StartViewModel(contextViewModel);
        }
    }
}
