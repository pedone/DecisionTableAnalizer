using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Documents;
using ExtensionLibrary;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using DTCore;

namespace ViewModels.Commands
{
    public class ApplicationViewPrintDecisionTablesCommand : ViewModelCommand<ApplicationViewModel>
    {

        private static FontFamily FontFamilyProjectSectionHeader { get; set; }
        private static FontStyle FontStyleProjectSectionHeader { get; set; }
        private static FontWeight FontWeightProjectSectionHeader { get; set; }
        private static double FontSizeProjectSectionHeader { get; set; }
        private static Brush BrushProjectSectionHeader { get; set; }

        private static FontFamily FontFamilyProjectDescription { get; set; }
        private static FontStyle FontStyleProjectDescription { get; set; }
        private static FontWeight FontWeightProjectDescription { get; set; }
        private static double FontSizeProjectDescription { get; set; }
        private static Brush BrushProjectDescription { get; set; }

        private static FontFamily FontFamilyDecisionTableGroupSectionHeader { get; set; }
        private static FontStyle FontStyleDecisionTableGroupSectionHeader { get; set; }
        private static FontWeight FontWeightDecisionTableGroupSectionHeader { get; set; }
        private static double FontSizeDecisionTableGroupSectionHeader { get; set; }
        private static Brush BrushDecisionTableGroupSectionHeader { get; set; }

        private static FontFamily FontFamilyDecisionTableSectionHeader { get; set; }
        private static FontStyle FontStyleDecisionTableSectionHeader { get; set; }
        private static FontWeight FontWeightDecisionTableSectionHeader { get; set; }
        private static double FontSizeDecisionTableSectionHeader { get; set; }
        private static Brush BrushDecisionTableSectionHeader { get; set; }

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

        private static FontFamily FontFamilySubTableSectionHeader { get; set; }
        private static FontStyle FontStyleSubTableSectionHeader { get; set; }
        private static FontWeight FontWeightSubTableSectionHeader { get; set; }
        private static double FontSizeSubTableSectionHeader { get; set; }
        private static Brush BrushSubTableSectionHeader { get; set; }

        private static FontFamily FontFamilyDTElementDescription { get; set; }
        private static FontStyle FontStyleDTElementDescription { get; set; }
        private static FontWeight FontWeightDTElementDescription { get; set; }
        private static double FontSizeDTElementDescription { get; set; }
        private static Brush BrushDTElementDescription { get; set; }

        static ApplicationViewPrintDecisionTablesCommand()
        {
            FontFamily calibriFontFamily = new FontFamily("Calibri");

            FontFamilyProjectSectionHeader = calibriFontFamily;
            FontStyleProjectSectionHeader = FontStyles.Normal;
            FontWeightProjectSectionHeader = FontWeights.Bold;
            FontSizeProjectSectionHeader = 25;
            BrushProjectSectionHeader = (Brush)new BrushConverter().ConvertFrom("#36393B");

            FontFamilyProjectDescription = calibriFontFamily;
            FontStyleProjectDescription = FontStyles.Normal;
            FontWeightProjectDescription = FontWeights.Normal;
            FontSizeProjectDescription = 17;
            BrushProjectDescription = Brushes.Gray;

            FontFamilyDecisionTableGroupSectionHeader = calibriFontFamily;
            FontStyleDecisionTableGroupSectionHeader = FontStyles.Normal;
            FontWeightDecisionTableGroupSectionHeader = FontWeights.SemiBold;
            FontSizeDecisionTableGroupSectionHeader = 23;
            BrushDecisionTableGroupSectionHeader = (Brush)new BrushConverter().ConvertFrom("#45484B");

            FontFamilyDecisionTableSectionHeader = calibriFontFamily;
            FontStyleDecisionTableSectionHeader = FontStyles.Normal;
            FontWeightDecisionTableSectionHeader = FontWeights.SemiBold;
            FontSizeDecisionTableSectionHeader = 20;
            BrushDecisionTableSectionHeader = (Brush)new BrushConverter().ConvertFrom("#696758");

            FontFamilyDTElementsSectionHeader = calibriFontFamily;
            FontStyleDTElementsSectionHeader = FontStyles.Normal;
            FontWeightDTElementsSectionHeader = FontWeights.SemiBold;
            FontSizeDTElementsSectionHeader = 18;
            BrushDTElementsSectionHeader = (Brush)new BrushConverter().ConvertFrom("#53625F");

            FontFamilyDecisionTableName = calibriFontFamily;
            FontStyleDecisionTableName = FontStyles.Normal;
            FontWeightDecisionTableName = FontWeights.Heavy;
            FontSizeDecisionTableName = 16;
            BrushDecisionTableName = (Brush)new BrushConverter().ConvertFrom("#2A454E");

            FontFamilyDecisionTableDescription = calibriFontFamily;
            FontStyleDecisionTableDescription = FontStyles.Normal;
            FontWeightDecisionTableDescription = FontWeights.Normal;
            FontSizeDecisionTableDescription = 14;
            BrushDecisionTableDescription = Brushes.Black;

            FontFamilyDTElementName = calibriFontFamily;
            FontStyleDTElementName = FontStyles.Normal;
            FontWeightDTElementName = FontWeights.DemiBold;
            FontSizeDTElementName = 14;
            BrushDTElementName = Brushes.Gray;

            FontFamilySubTableSectionHeader = calibriFontFamily;
            FontStyleSubTableSectionHeader = FontStyles.Normal;
            FontWeightSubTableSectionHeader = FontWeights.DemiBold;
            FontSizeSubTableSectionHeader = 14;
            BrushSubTableSectionHeader = (Brush)new BrushConverter().ConvertFrom("#777257");

            FontFamilyDTElementDescription = calibriFontFamily;
            FontStyleDTElementDescription = FontStyles.Normal;
            FontWeightDTElementDescription = FontWeights.Normal;
            FontSizeDTElementDescription = 14;
            BrushDTElementDescription = Brushes.Black;
        }

        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetDecisionTableManager";
            var decisionTableManagerViewModel = ViewModelService.Instance.ExecuteOperation<DecisionTableManagerViewModel>(serviceId, operationId, contextViewModel.CurrentProject.EntityId);

            var printDecisionTablesDialogModel = ViewModelService.Instance.QueryViewModel<PrintDecisionTablesDialogModel>(decisionTableManagerViewModel.EntityId);
            if (ViewService.Instance.ShowDialog(printDecisionTablesDialogModel))
            {
                var selectedDecisionTables = printDecisionTablesDialogModel.DecisionTables.Where(cur => cur.IsSelected);
                try
                {
                    var projectDialogModel = ViewModelService.Instance.QueryViewModel<ProjectDialogModel>(contextViewModel.CurrentProject.EntityId);
                    var printDocument = BuildDecisionTablesPrintDocument(projectDialogModel, selectedDecisionTables);

                    PrintPreviewDialogModel previewDialogModel = new PrintPreviewDialogModel { Document = printDocument };
                    if (ViewService.Instance.ShowDialog(previewDialogModel))
                    {
                        PrintDocument(previewDialogModel.Document);
                    }
                }
                catch
                {
                    MessageBox.Show("An error occured while printing the decision tables.", "Error");
                }
            }
        }

        private void PrintDocument(FlowDocument document)
        {
            try
            {
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    document.PageHeight = printDialog.PrintableAreaHeight;
                    document.PageWidth = printDialog.PrintableAreaWidth;
                    document.PagePadding = new Thickness(25);
                    document.ColumnGap = 0;
                    document.ColumnWidth = (document.PageWidth -
                                           document.ColumnGap -
                                           document.PagePadding.Left -
                                           document.PagePadding.Right);

                    printDialog.PrintDocument(((IDocumentPaginatorSource)document)
                                             .DocumentPaginator,
                                             "DecisionTableAnalyzer Print Job");
                }
            }
            catch
            {
                MessageBox.Show("The document could not be printed.", "Error");
            }
        }

        public FlowDocument BuildDecisionTablesPrintDocument(ProjectDialogModel projectViewModel, IEnumerable<SystemDecisionTableViewModel> decisionTables)
        {
            Section projectSection = BuildNewProjectSection(projectViewModel.Name);
            if (!projectViewModel.Description.IsEmpty())
            {
                projectSection.Blocks.Add(new Paragraph(new Run(projectViewModel.Description)
                {
                    FontSize = FontSizeProjectDescription,
                    FontStyle = FontStyleProjectDescription,
                    FontFamily = FontFamilyProjectDescription,
                    FontWeight = FontWeightProjectDescription,
                    Foreground = BrushProjectDescription
                }));
            }

            Section decisionTableGroupSection = BuildNewDecisionTableGroupSection();
            foreach (var decisionTable in decisionTables)
            {
                var tableSection = BuildDecisionTableSection(decisionTable);
                decisionTableGroupSection.Blocks.Add(tableSection);
            }

            FlowDocument document = new FlowDocument();
            document.Blocks.Add(projectSection);
            document.Blocks.Add(decisionTableGroupSection);

            return document;
        }

        private Section BuildDecisionTableSection(SystemDecisionTableViewModel decisionTable)
        {
            var fullDecisionTableViewModel = ViewModelService.Instance.QueryViewModel<DecisionTableViewModel>(decisionTable.EntityId);
            Section decisionTableSection = BuildNewDecisionTableSection(decisionTable.Name);

            //Condition Sub Tables
            var selectedConditionSubTables = decisionTable.SubTables.Where(
                cur =>
                {
                    return cur.IsSelected &&
                        fullDecisionTableViewModel.Conditions.Any(condition => condition.ReferenceSubTableId == cur.EntityId);
                }).ToList();

            Section conditionSubTableSection = BuildNewConditionSubTableSection();
            foreach (var conditionTable in selectedConditionSubTables)
            {
                var conditionTableSection = BuildDecisionTableSection(conditionTable);
                conditionSubTableSection.Blocks.Add(conditionTableSection);
            }

            //Current Decision Table
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

            //Conditions
            Section conditionsSection = BuildNewConditionsSection();
            var selectedConditions = decisionTable.Conditions.Where(cur => cur.IsSelected).ToList();
            foreach (var condition in selectedConditions)
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
            Section actionsSection = BuildNewActionsSection();
            var selectedActions = decisionTable.Actions.Where(cur => cur.IsSelected).ToList();
            foreach (var action in selectedActions)
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

            //Decision table view
            Section decisionTableViewSection = new Section();
            decisionTableViewSection.Blocks.Add(BuildDecisionTableView(fullDecisionTableViewModel));

            //Action Sub Tables
            var selectedActionSubTables = decisionTable.SubTables.Where(
                cur =>
                {
                    return cur.IsSelected &&
                        fullDecisionTableViewModel.Actions.Any(action => action.ReferenceSubTableId == cur.EntityId);
                }).ToList();

            Section actionSubTableSection = BuildNewActionSubTableSection();
            foreach (var actionTable in selectedActionSubTables)
            {
                var actionTableSection = BuildDecisionTableSection(actionTable);
                actionSubTableSection.Blocks.Add(actionTableSection);
            }

            //Condition Sub Tables
            if (selectedConditionSubTables.Count > 0)
                decisionTableSection.Blocks.Add(conditionSubTableSection);

            //Current Table
            decisionTableSection.Blocks.Add(curDecisionTableHeaderParagraph);
            if (!decisionTable.Description.IsEmpty())
                decisionTableSection.Blocks.Add(curDecisionTableDescriptionParagraph);

            decisionTableSection.Blocks.Add(decisionTableViewSection);

            if (selectedConditions.Count > 0)
                decisionTableSection.Blocks.Add(conditionsSection);
            if (selectedActions.Count > 0)
                decisionTableSection.Blocks.Add(actionsSection);

            //Action Sub Tables
            if (selectedActionSubTables.Count > 0)
                decisionTableSection.Blocks.Add(actionSubTableSection);

            return decisionTableSection;
        }

        private Block BuildDecisionTableView(DecisionTableViewModel decisionTable)
        {
            var tableGridBrush = Brushes.Gray;
            var tableGridThickness = new Thickness(1.5);

            Table dtTable = new Table { CellSpacing = 0, BorderBrush = tableGridBrush, BorderThickness = tableGridThickness };
            dtTable.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            for (int i = 1; i < decisionTable.Conditions.Count(); i++)
                dtTable.Columns.Add(new TableColumn());

            int columnCount = decisionTable.Rules.Count + 1;
            TableRowGroup rowGroup = new TableRowGroup();

            //Title row
            Paragraph titleParagraph = new Paragraph(new Run(decisionTable.Name)) { FontSize = 24, FontWeight = FontWeights.Bold };
            TableCell titleCell = new TableCell(titleParagraph) { ColumnSpan = columnCount, TextAlignment = TextAlignment.Left };
            TableRow titleRow = new TableRow { Background = Brushes.WhiteSmoke };
            titleRow.Cells.Add(titleCell);

            //Header row
            TableRow headerRow = new TableRow { Background = Brushes.LightGoldenrodYellow };
            headerRow.Cells.Add(new TableCell());
            for (int i = 0; i < decisionTable.Rules.Count; i++)
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
            foreach (var conditionRow in decisionTable.Rows.Where(cur => cur.Header is ConditionViewModel))
            {
                var curCondition = (ConditionViewModel)conditionRow.Header;
                TableRow curRow = new TableRow();
                Paragraph nameParagraph = new Paragraph(new Run(curCondition.Name))
                {
                    Margin = new Thickness(5, 0, 0, 0)
                };
                if (curCondition.HasReferenceSubTable)
                    nameParagraph.TextDecorations = TextDecorations.Underline;

                curRow.Cells.Add(new TableCell(nameParagraph));
                foreach (var state in conditionRow.States)
                {
                    curRow.Cells.Add(new TableCell(new Paragraph(new Run(state.Name))
                    {
                        TextAlignment = TextAlignment.Center,
                        Margin = dtElementMargin
                    }));
                }

                conditionRows.Add(curRow);
            }

            //Actions
            Brush actionBackgroundBrush = Brushes.LightGray;
            List<TableRow> actionRows = new List<TableRow>();
            foreach (var actionRow in decisionTable.Rows.Where(cur => cur.Header is ActionViewModel))
            {
                var curAction = (ActionViewModel)actionRow.Header;
                TableRow curRow = new TableRow();
                Paragraph nameParagraph = new Paragraph(new Run(curAction.Name))
                {
                    Margin = new Thickness(5, 0, 0, 0)
                };
                if (curAction.HasReferenceSubTable)
                    nameParagraph.TextDecorations = TextDecorations.Underline;

                curRow.Cells.Add(new TableCell(nameParagraph) { Background = actionBackgroundBrush });
                foreach (var state in actionRow.States)
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

            if (decisionTable.Rules.Count <= 8)
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

        private static Section BuildNewDecisionTableGroupSection()
        {
            return new Section(new Paragraph(new Run("Decision Tables")
            {
                FontSize = FontSizeDecisionTableGroupSectionHeader,
                FontStyle = FontStyleDecisionTableGroupSectionHeader,
                FontWeight = FontWeightDecisionTableGroupSectionHeader,
                FontFamily = FontFamilyDecisionTableGroupSectionHeader,
                Foreground = BrushDecisionTableGroupSectionHeader
            }));
        }

        private static Section BuildNewProjectSection(string projectName)
        {
            return new Section(new Paragraph(new Run(projectName)
            {
                FontSize = FontSizeProjectSectionHeader,
                FontStyle = FontStyleProjectSectionHeader,
                FontWeight = FontWeightProjectSectionHeader,
                FontFamily = FontFamilyProjectSectionHeader,
                Foreground = BrushProjectSectionHeader
            }));
        }

        private static Section BuildNewConditionsSection()
        {
            return new Section(new Paragraph(new Run("Conditions")
            {
                FontSize = FontSizeDTElementsSectionHeader,
                FontStyle = FontStyleDTElementsSectionHeader,
                FontWeight = FontWeightDTElementsSectionHeader,
                FontFamily = FontFamilyDTElementsSectionHeader,
                Foreground = BrushDTElementsSectionHeader
            }));
        }

        private static Section BuildNewActionsSection()
        {
            return new Section(new Paragraph(new Run("Actions")
            {
                FontSize = FontSizeDTElementsSectionHeader,
                FontStyle = FontStyleDTElementsSectionHeader,
                FontWeight = FontWeightDTElementsSectionHeader,
                FontFamily = FontFamilyDTElementsSectionHeader,
                Foreground = BrushDTElementsSectionHeader
            }));
        }

        private static Section BuildNewActionSubTableSection()
        {
            return new Section(new Paragraph(new Run("Action Tables")
            {
                FontSize = FontSizeSubTableSectionHeader,
                FontStyle = FontStyleSubTableSectionHeader,
                FontWeight = FontWeightSubTableSectionHeader,
                FontFamily = FontFamilySubTableSectionHeader,
                Foreground = BrushSubTableSectionHeader
            }));
        }

        private static Section BuildNewConditionSubTableSection()
        {
            return new Section(new Paragraph(new Run("Condition Tables")
            {
                FontSize = FontSizeSubTableSectionHeader,
                FontStyle = FontStyleSubTableSectionHeader,
                FontWeight = FontWeightSubTableSectionHeader,
                FontFamily = FontFamilySubTableSectionHeader,
                Foreground = BrushSubTableSectionHeader
            }));
        }

        private static Section BuildNewDecisionTableSection(string decisionTableName)
        {
            return new Section(new Paragraph(new Run(decisionTableName)
            {
                FontSize = FontSizeDecisionTableSectionHeader,
                FontStyle = FontStyleDecisionTableSectionHeader,
                FontWeight = FontWeightDecisionTableSectionHeader,
                FontFamily = FontFamilyDecisionTableSectionHeader,
                Foreground = BrushDecisionTableSectionHeader
            }));
        }

    }
}
