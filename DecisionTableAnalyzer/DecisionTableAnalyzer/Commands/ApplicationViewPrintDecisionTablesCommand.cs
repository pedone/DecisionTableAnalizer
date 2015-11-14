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
    public class ApplicationViewPrintDecisionTablesCommand : DTCommand<ApplicationViewModel>
    {

        private static FontFamily FontFamilyProjectName { get; set; }
        private static FontStyle FontStyleProjectName { get; set; }
        private static FontWeight FontWeightProjectName { get; set; }
        private static double FontSizeProjectName { get; set; }
        private static Brush BrushProjectName { get; set; }

        private static FontFamily FontFamilyProjectDescription { get; set; }
        private static FontStyle FontStyleProjectDescription { get; set; }
        private static FontWeight FontWeightProjectDescription { get; set; }
        private static double FontSizeProjectDescription { get; set; }
        private static Brush BrushProjectDescription { get; set; }

        private static FontFamily FontFamilyDecisionTablesSectionHeader { get; set; }
        private static FontStyle FontStyleDecisionTablesSectionHeader { get; set; }
        private static FontWeight FontWeightDecisionTablesSectionHeader { get; set; }
        private static double FontSizeDecisionTablesSectionHeader { get; set; }
        private static Brush BrushDecisionTablesSectionHeader { get; set; }

        private static FontFamily FontFamilyDTElementsSectionHeader { get; set; }
        private static FontStyle FontStyleDTElementsSectionHeader { get; set; }
        private static FontWeight FontWeightDTElementsSectionHeader { get; set; }
        private static double FontSizeDTElementsSectionHeader { get; set; }
        private static Brush BrushDTElementsSectionHeader { get; set; }

        private static FontFamily FontFamilyDecisionTableName { get; set; }
        private static FontStyle FontStyleDecisionTableName { get; set; }
        private static FontWeight FontWeightDecisionTableName { get; set; }
        private static double FontSizeDecisionTableName { get; set; }
        private static Brush BrushDecisionTableName { get; set; }

        private static FontFamily FontFamilyDecisionTableDescription { get; set; }
        private static FontStyle FontStyleDecisionTableDescription { get; set; }
        private static FontWeight FontWeightDecisionTableDescription { get; set; }
        private static double FontSizeDecisionTableDescription { get; set; }
        private static Brush BrushDecisionTableDescription { get; set; }

        private static FontFamily FontFamilyDTElementName { get; set; }
        private static FontStyle FontStyleDTElementName { get; set; }
        private static FontWeight FontWeightDTElementName { get; set; }
        private static double FontSizeDTElementName { get; set; }
        private static Brush BrushDTElementName { get; set; }

        private static FontFamily FontFamilyDTElementDescription { get; set; }
        private static FontStyle FontStyleDTElementDescription { get; set; }
        private static FontWeight FontWeightDTElementDescription { get; set; }
        private static double FontSizeDTElementDescription { get; set; }
        private static Brush BrushDTElementDescription { get; set; }

        static ApplicationViewPrintDecisionTablesCommand()
        {
            FontFamily calibriFontFamily = new FontFamily("Calibri");

            FontFamilyProjectName = calibriFontFamily;
            FontStyleProjectName = FontStyles.Normal;
            FontWeightProjectName = FontWeights.Bold;
            FontSizeProjectName = 23;
            BrushProjectName = (Brush)new BrushConverter().ConvertFrom("#2A454E");

            FontFamilyProjectDescription = calibriFontFamily;
            FontStyleProjectDescription = FontStyles.Normal;
            FontWeightProjectDescription = FontWeights.Normal;
            FontSizeProjectDescription = 17;
            BrushProjectDescription = Brushes.Gray;

            FontFamilyDecisionTablesSectionHeader = calibriFontFamily;
            FontStyleDecisionTablesSectionHeader = FontStyles.Normal;
            FontWeightDecisionTablesSectionHeader = FontWeights.SemiBold;
            FontSizeDecisionTablesSectionHeader = 20;
            BrushDecisionTablesSectionHeader = (Brush)new BrushConverter().ConvertFrom("#53625F");

            FontFamilyDTElementsSectionHeader = calibriFontFamily;
            FontStyleDTElementsSectionHeader = FontStyles.Normal;
            FontWeightDTElementsSectionHeader = FontWeights.SemiBold;
            FontSizeDTElementsSectionHeader = 20;
            BrushDTElementsSectionHeader = (Brush)new BrushConverter().ConvertFrom("#53625F");

            FontFamilyDecisionTableName = calibriFontFamily;
            FontStyleDecisionTableName = FontStyles.Normal;
            FontWeightDecisionTableName = FontWeights.Heavy;
            FontSizeDecisionTableName = 17;
            BrushDecisionTableName = (Brush)new BrushConverter().ConvertFrom("#2A454E");

            FontFamilyDecisionTableDescription = calibriFontFamily;
            FontStyleDecisionTableDescription = FontStyles.Normal;
            FontWeightDecisionTableDescription = FontWeights.Normal;
            FontSizeDecisionTableDescription = 15;
            BrushDecisionTableDescription = Brushes.Black;

            FontFamilyDTElementName = calibriFontFamily;
            FontStyleDTElementName = FontStyles.Normal;
            FontWeightDTElementName = FontWeights.DemiBold;
            FontSizeDTElementName = 15;
            BrushDTElementName = Brushes.Gray;

            FontFamilyDTElementDescription = calibriFontFamily;
            FontStyleDTElementDescription = FontStyles.Normal;
            FontWeightDTElementDescription = FontWeights.Normal;
            FontSizeDTElementDescription = 15;
            BrushDTElementDescription = Brushes.Black;
        }

        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (contextViewModel.Project.DecisionTableManager.DecisionTables.Count == 0)
            {
                MessageBox.Show("The project does not contain any decision tables.", "No decision tables");
                return;
            }

            try
            {
                PrintDecisionTables(contextViewModel);
            }
            catch
            {
                MessageBox.Show("An error occured while printing the decision tables.", "Error");
            }
        }

        public void PrintDecisionTables(ApplicationViewModel contextViewModel)
        {
            DTProject project = contextViewModel.Project;

            Section projectSection = new Section(new Paragraph(new Run(project.Name)
            {
                FontSize = FontSizeProjectName,
                FontStyle = FontStyleProjectName,
                FontWeight = FontWeightProjectName,
                FontFamily = FontFamilyProjectName,
                Foreground = BrushProjectName
            }));
            if (!project.Description.IsEmpty())
            {
                projectSection.Blocks.Add(new Paragraph(new Run(project.Description)
                {
                    FontSize = FontSizeProjectDescription,
                    FontStyle = FontStyleProjectDescription,
                    FontFamily = FontFamilyProjectDescription,
                    FontWeight = FontWeightProjectDescription,
                    Foreground = BrushProjectDescription
                }));
            }

            Section decisionTablesDetailsSection = new Section(new Paragraph(new Run("Decision Tables")
            {
                FontSize = FontSizeDecisionTablesSectionHeader,
                FontStyle = FontStyleDecisionTablesSectionHeader,
                FontWeight = FontWeightDecisionTablesSectionHeader,
                FontFamily = FontFamilyDecisionTablesSectionHeader,
                Foreground = BrushDecisionTablesSectionHeader
            }));
            Section conditionsSection = new Section(new Paragraph(new Run("Conditions")
            {
                FontSize = FontSizeDTElementsSectionHeader,
                FontStyle = FontStyleDTElementsSectionHeader,
                FontWeight = FontWeightDTElementsSectionHeader,
                FontFamily = FontFamilyDTElementsSectionHeader,
                Foreground = BrushDTElementsSectionHeader
            }));
            Section actionsSection = new Section(new Paragraph(new Run("Actions")
            {
                FontSize = FontSizeDTElementsSectionHeader,
                FontStyle = FontStyleDTElementsSectionHeader,
                FontWeight = FontWeightDTElementsSectionHeader,
                FontFamily = FontFamilyDTElementsSectionHeader,
                Foreground = BrushDTElementsSectionHeader
            }));
            if (project.DecisionTableManager.DecisionTables.Count() > 0)
            {
                foreach (var decisionTable in project.DecisionTableManager.DecisionTables)
                {
                    Paragraph curDecisionTableHeaderParagraph = new Paragraph();
                    curDecisionTableHeaderParagraph.Inlines.Add(new Run(decisionTable.Name)
                    {
                        FontSize = FontSizeDecisionTableName,
                        FontStyle = FontStyleDecisionTableName,
                        FontWeight = FontWeightDecisionTableName,
                        FontFamily = FontFamilyDecisionTableName,
                        Foreground = BrushDecisionTableName
                    });

                    Paragraph curDecisionTableDescriptionParagraph = new Paragraph();
                    curDecisionTableDescriptionParagraph.Inlines.Add(new Run(decisionTable.Description)
                    {
                        FontSize = FontSizeDecisionTableDescription,
                        FontStyle = FontStyleDecisionTableDescription,
                        FontWeight = FontWeightDecisionTableDescription,
                        FontFamily = FontFamilyDecisionTableDescription,
                        Foreground = BrushDecisionTableDescription
                    });

                    decisionTablesDetailsSection.Blocks.Add(curDecisionTableHeaderParagraph);
                    if (!decisionTable.Description.IsEmpty())
                        decisionTablesDetailsSection.Blocks.Add(curDecisionTableDescriptionParagraph);

                    //Conditions
                    foreach (var condition in decisionTable.Conditions)
                    {
                        Paragraph curConditionHeaderParagraph = new Paragraph();
                        curConditionHeaderParagraph.Inlines.Add(new Run(condition.Name)
                        {
                            FontSize = FontSizeDTElementName,
                            FontStyle = FontStyleDTElementName,
                            FontWeight = FontWeightDTElementName,
                            FontFamily = FontFamilyDTElementName,
                            Foreground = BrushDTElementName
                        });
                        Paragraph curConditionDescriptionParagraph = new Paragraph();
                        curConditionDescriptionParagraph.Inlines.Add(new Run(condition.Description)
                        {
                            FontSize = FontSizeDTElementDescription,
                            FontStyle = FontStyleDTElementDescription,
                            FontWeight = FontWeightDTElementDescription,
                            FontFamily = FontFamilyDTElementDescription,
                            Foreground = BrushDTElementDescription
                        });

                        conditionsSection.Blocks.Add(curConditionHeaderParagraph);
                        if (!condition.Description.IsEmpty())
                            conditionsSection.Blocks.Add(curConditionDescriptionParagraph);
                    }

                    //Actions
                    foreach (var action in decisionTable.Actions)
                    {
                        Paragraph curActionHeaderParagraph = new Paragraph();
                        curActionHeaderParagraph.Inlines.Add(new Run(action.Name)
                        {
                            FontSize = FontSizeDTElementName,
                            FontStyle = FontStyleDTElementName,
                            FontWeight = FontWeightDTElementName,
                            FontFamily = FontFamilyDTElementName,
                            Foreground = BrushDTElementName
                        });
                        Paragraph curActionDescriptionParagraph = new Paragraph();
                        curActionDescriptionParagraph.Inlines.Add(new Run(action.Description)
                        {
                            FontSize = FontSizeDTElementDescription,
                            FontStyle = FontStyleDTElementDescription,
                            FontWeight = FontWeightDTElementDescription,
                            FontFamily = FontFamilyDTElementDescription,
                            Foreground = BrushDTElementDescription
                        });

                        actionsSection.Blocks.Add(curActionHeaderParagraph);
                        if (!action.Description.IsEmpty())
                            actionsSection.Blocks.Add(curActionDescriptionParagraph);
                    }
                }
            }
            else
            {
                decisionTablesDetailsSection.Blocks.Add(new Paragraph(new Run("There are no decision tables.") { FontStyle = FontStyles.Italic }));
            }


            //Decision table views
            Section decisionTableViewSection = new Section();
            foreach (var decisionTable in project.DecisionTableManager.DecisionTables)
            {
                decisionTableViewSection.Blocks.Add(BuildDecisionTableView(decisionTable));
            }

            decisionTableViewSection.BreakPageBefore = true;

            FlowDocument document = new FlowDocument();
            document.Blocks.Add(projectSection);
            document.Blocks.Add(decisionTablesDetailsSection);
            document.Blocks.Add(conditionsSection);
            document.Blocks.Add(actionsSection);
            document.Blocks.Add(decisionTableViewSection);

            PrintRequirementsDialogModel dialogModel = new PrintRequirementsDialogModel { RequirementsDocument = document };
            PrintRequirementsDialog dialog = new PrintRequirementsDialog { Owner = App.Current.MainWindow, DataContext = dialogModel };
            dialog.ShowDialog();
        }

        private Block BuildDecisionTableView(DecisionTable decisionTable)
        {
            var tableGridBrush = Brushes.Gray;
            var tableGridThickness = new Thickness(1.5);

            Table dtTable = new Table { CellSpacing = 0, BorderBrush = tableGridBrush, BorderThickness = tableGridThickness };
            dtTable.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            for (int i = 1; i < decisionTable.Conditions.Count(); i++)
                dtTable.Columns.Add(new TableColumn());

            int columnCount = decisionTable.RuleCount + 1;
            TableRowGroup rowGroup = new TableRowGroup();

            //Title row
            Paragraph titleParagraph = new Paragraph(new Run(decisionTable.Name)) { FontSize = 24, FontWeight = FontWeights.Bold };
            TableCell titleCell = new TableCell(titleParagraph) { ColumnSpan = columnCount, TextAlignment = TextAlignment.Left };
            TableRow titleRow = new TableRow { Background = Brushes.WhiteSmoke };
            titleRow.Cells.Add(titleCell);

            //Header row
            TableRow headerRow = new TableRow { Background = Brushes.LightGoldenrodYellow };
            headerRow.Cells.Add(new TableCell());
            for (int i = 0; i < decisionTable.RuleCount; i++)
            {
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run((i + 1).ToString()))
                {
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                }));
            }

            var dtElementMargin = new Thickness(3);
            //Conditions
            List<TableRow> conditionRows = new List<TableRow>();
            foreach (var condition in decisionTable.Conditions)
            {
                TableRow curRow = new TableRow();
                curRow.Cells.Add(new TableCell(new Paragraph(new Run(condition.Name))
                {
                    Margin = new Thickness(5, 0, 0, 0)
                }));
                foreach (var state in condition.States)
                    curRow.Cells.Add(new TableCell(new Paragraph(new Run(state.Name))
                    {
                        TextAlignment = TextAlignment.Center,
                        Margin = dtElementMargin
                    }));

                conditionRows.Add(curRow);
            }

            //Actions
            Brush actionBackgroundBrush = Brushes.LightGray;
            List<TableRow> actionRows = new List<TableRow>();
            foreach (var action in decisionTable.Actions)
            {
                TableRow curRow = new TableRow();
                Paragraph nameParagraph = new Paragraph(new Run(action.Name)) { Margin = new Thickness(5, 0, 0, 0) };
                curRow.Cells.Add(new TableCell(nameParagraph) { Background = actionBackgroundBrush });
                foreach (var state in action.States)
                {
                    Paragraph stateParagraph = new Paragraph(new Run(state.Name)) { TextAlignment = TextAlignment.Center };
                    curRow.Cells.Add(new TableCell(stateParagraph) { Background = actionBackgroundBrush });
                }

                actionRows.Add(curRow);
            }

            rowGroup.Rows.Add(titleRow);
            rowGroup.Rows.Add(headerRow);
            foreach (var row in conditionRows)
                rowGroup.Rows.Add(row);
            foreach (var row in actionRows)
                rowGroup.Rows.Add(row);

            foreach (var row in rowGroup.Rows)
                foreach (var cell in row.Cells)
                {
                    cell.BorderBrush = tableGridBrush;
                    cell.BorderThickness = tableGridThickness;
                }

            dtTable.RowGroups.Add(rowGroup);

            if (decisionTable.RuleCount <= 8)
                return dtTable;
            else
                return RotateBlock(dtTable);
        }

        private Block RotateBlock(Block block)
        {
            var rotateDocument = new FlowDocument { PagePadding = new Thickness(0) };
            rotateDocument.Blocks.Add(block);

            var scrollViewer = new FlowDocumentScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                LayoutTransform = new RotateTransform(90),
                Document = rotateDocument
            };
            var container = new BlockUIContainer
            {
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                Child = scrollViewer
            };

            return container;
        }

    }
}
