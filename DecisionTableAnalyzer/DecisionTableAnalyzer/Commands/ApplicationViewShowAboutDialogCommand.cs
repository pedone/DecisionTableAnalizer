using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Documents;
using ExtensionLibrary;
using System.Windows.Media;
using System.Windows;
using DecisionTableAnalyzer.Views;
using System.Windows.Controls;

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewShowAboutDialogCommand : DTCommand<ApplicationViewModel>
    {

        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return true;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            AboutDialog dialog = new AboutDialog { Owner = App.Current.MainWindow };
            dialog.ShowDialog();
        }

    }
}
