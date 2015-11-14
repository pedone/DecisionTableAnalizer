using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;
using DecisionTableAnalyzer.Dialogs;
using DecisionTableAnalyzer.Models;
using Microsoft.Win32;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;

namespace DecisionTableAnalyzer.Commands
{
    public class PrintRequirementsDialogPrintCommand : DTCommand<PrintRequirementsDialogModel>
    {
        public override bool CanExecute(PrintRequirementsDialogModel contextViewModel)
        {
            return contextViewModel != null;
        }

        public override void Execute(PrintRequirementsDialogModel contextViewModel)
        {
            try
            {
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    var flowDoc = contextViewModel.RequirementsDocument;
                    flowDoc.PageHeight = printDialog.PrintableAreaHeight;
                    flowDoc.PageWidth = printDialog.PrintableAreaWidth;
                    flowDoc.PagePadding = new Thickness(25);
                    flowDoc.ColumnGap = 0;
                    flowDoc.ColumnWidth = (flowDoc.PageWidth -
                                           flowDoc.ColumnGap -
                                           flowDoc.PagePadding.Left -
                                           flowDoc.PagePadding.Right);

                    printDialog.PrintDocument(((IDocumentPaginatorSource)flowDoc)
                                             .DocumentPaginator,
                                             "DecisionTableAnalyzer Print Job");
                }
            }
            catch
            {
                MessageBox.Show("The document could not be printed.", "Error");
            }
        }
    }
}
