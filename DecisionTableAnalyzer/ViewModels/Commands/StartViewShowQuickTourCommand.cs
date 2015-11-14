using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Documents;
using DTCore;

namespace ViewModels.Commands
{
    public class StartViewShowQuickTourCommand : ViewModelCommand<StartViewModel>
    {
        public override bool CanExecute(StartViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(StartViewModel contextViewModel)
        {
            FlowDocument document = new FlowDocument();
            string quickTourHeader = Application.Current.FindResource("QuickTourHeader") as string;
            if (string.IsNullOrEmpty(quickTourHeader))
                quickTourHeader = "Welcome to Decision Table Analyzer";

            string quickTourText = Application.Current.FindResource("QuickTourText") as string;
            if (quickTourText == null)
                return;

            Paragraph headerParagraph = new Paragraph(new Run(quickTourHeader))
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };

            Paragraph quickTourParagraph = new Paragraph(new Run(quickTourText));

            document.Blocks.Add(headerParagraph);
            document.Blocks.Add(quickTourParagraph);

            DocumentViewModel documentViewModel = new DocumentViewModel
            {
                Header = "Quick Tour",
                Document = document
            };

            ViewService.Instance.ShowView(documentViewModel);
        }
    }
}
