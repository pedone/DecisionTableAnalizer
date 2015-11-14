using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Documents;
using DecisionTableAnalyzer.Views;

namespace DecisionTableAnalyzer.Commands
{
    public class StartViewShowQuickTourCommand : DTCommand<StartViewModel>
    {
        public override bool CanExecute(StartViewModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(StartViewModel contextViewModel)
        {
            FlowDocument document = new FlowDocument();
            using (Stream quickTourStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DecisionTableAnalyzer.Content.QuickTour.txt"))
            using(StreamReader reader=new StreamReader(quickTourStream))
            {
                Paragraph headerParagraph = new Paragraph(new Run(reader.ReadLine()))
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold
                };

                Paragraph quickTourParagraph = new Paragraph(new Run(reader.ReadToEnd()));

                document.Blocks.Add(headerParagraph);
                document.Blocks.Add(quickTourParagraph);
            }

            DocumentViewModel documentViewModel = new DocumentViewModel
            {
                Header = "Quick Tour",
                Document = document
            };

            var documentView = contextViewModel.ApplicationViewModel.DockManager.ShowView<DocumentView>(activate: true);
            documentView.DataContext = documentViewModel;
        }
    }
}
