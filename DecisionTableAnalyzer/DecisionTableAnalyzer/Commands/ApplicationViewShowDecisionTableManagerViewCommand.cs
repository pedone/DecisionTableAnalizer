﻿using System;
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
    public class ApplicationViewShowDecisionTableManagerViewCommand : DTCommand<ApplicationViewModel>
    {
        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null && contextViewModel.DockManager != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            DecisionTableManagerView dtManagerView = contextViewModel.DockManager.ShowView<DecisionTableManagerView>(activate: true);
            dtManagerView.DataContext = new DecisionTableManagerViewModel(contextViewModel.Project.DecisionTableManager, contextViewModel.DockManager);
        }
    }
}
