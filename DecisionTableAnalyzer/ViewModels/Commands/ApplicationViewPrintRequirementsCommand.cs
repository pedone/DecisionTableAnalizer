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
using DTCore;
using System.Windows.Controls;

namespace ViewModels.Commands
{
    public class ApplicationViewPrintRequirementsCommand : ViewModelCommand<ApplicationViewModel>
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

        private static FontFamily FontFamilyRequirementSectionHeader { get; set; }
        private static FontStyle FontStyleRequirementSectionHeader { get; set; }
        private static FontWeight FontWeightRequirementSectionHeader { get; set; }
        private static double FontSizeRequirementSectionHeader { get; set; }
        private static Brush BrushRequirementSectionHeader { get; set; }

        private static FontFamily FontFamilyRequirementName { get; set; }
        private static FontStyle FontStyleRequirementName { get; set; }
        private static FontWeight FontWeightRequirementName { get; set; }
        private static double FontSizeRequirementName { get; set; }
        private static Brush BrushRequirementName { get; set; }

        private static FontFamily FontFamilyRequirementPriorityHeader { get; set; }
        private static FontStyle FontStyleRequirementPriorityHeader { get; set; }
        private static FontWeight FontWeightRequirementPriorityHeader { get; set; }
        private static double FontSizeRequirementPriorityHeader { get; set; }
        private static Brush BrushRequirementPriorityHeader { get; set; }

        private static FontFamily FontFamilyRequirementPriority { get; set; }
        private static FontStyle FontStyleRequirementPriority { get; set; }
        private static FontWeight FontWeightRequirementPriority { get; set; }
        private static double FontSizeRequirementPriority { get; set; }
        private static Brush BrushRequirementPriority { get; set; }

        private static FontFamily FontFamilyRequirementDescription { get; set; }
        private static FontStyle FontStyleRequirementDescription { get; set; }
        private static FontWeight FontWeightRequirementDescription { get; set; }
        private static double FontSizeRequirementDescription { get; set; }
        private static Brush BrushRequirementDescription { get; set; }

        static ApplicationViewPrintRequirementsCommand()
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

            FontFamilyRequirementSectionHeader = calibriFontFamily;
            FontStyleRequirementSectionHeader = FontStyles.Normal;
            FontWeightRequirementSectionHeader = FontWeights.SemiBold;
            FontSizeRequirementSectionHeader = 20;
            BrushRequirementSectionHeader = (Brush)new BrushConverter().ConvertFrom("#53625F");

            FontFamilyRequirementName = calibriFontFamily;
            FontStyleRequirementName = FontStyles.Normal;
            FontWeightRequirementName = FontWeights.Heavy;
            FontSizeRequirementName = 17;
            BrushRequirementName = (Brush)new BrushConverter().ConvertFrom("#2A454E");

            FontFamilyRequirementPriorityHeader = calibriFontFamily;
            FontStyleRequirementPriorityHeader = FontStyles.Normal;
            FontWeightRequirementPriorityHeader = FontWeights.DemiBold;
            FontSizeRequirementPriorityHeader = 15;
            BrushRequirementPriorityHeader = Brushes.Gray;

            FontFamilyRequirementPriority = calibriFontFamily;
            FontStyleRequirementPriority = FontStyles.Normal;
            FontWeightRequirementPriority = FontWeights.Normal;
            FontSizeRequirementPriority = 15;
            BrushRequirementPriority = Brushes.Black;

            FontFamilyRequirementDescription = calibriFontFamily;
            FontStyleRequirementDescription = FontStyles.Normal;
            FontWeightRequirementDescription = FontWeights.Normal;
            FontSizeRequirementDescription = 15;
            BrushRequirementDescription = Brushes.Black;
        }

        public override bool CanExecute(ApplicationViewModel contextViewModel)
        {
            return contextViewModel != null && contextViewModel.IsProjectLoaded;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            string serviceId = "DTServices.CommonServices";
            string operationId = "GetRequirementManager";
            var requirementManagerViewModel = ViewModelService.Instance.ExecuteOperation<RequirementManagerViewModel>(serviceId, operationId, contextViewModel.CurrentProject.EntityId);

            var printRequirementsDialogModel = ViewModelService.Instance.QueryViewModel<PrintRequirementsDialogModel>(requirementManagerViewModel.EntityId);
            if (ViewService.Instance.ShowDialog(printRequirementsDialogModel))
            {
                var selectedFunctionalRequirements = printRequirementsDialogModel.FunctionalRequirements.Where(cur => cur.IsSelected);
                var selectedNonFunctionalRequirements = printRequirementsDialogModel.NonFunctionalRequirements.Where(cur => cur.IsSelected);
                try
                {
                    var projectDialogModel = ViewModelService.Instance.QueryViewModel<ProjectDialogModel>(contextViewModel.CurrentProject.EntityId);
                    var printDocument = BuildRequirementsPrintDocument(projectDialogModel, selectedFunctionalRequirements, selectedNonFunctionalRequirements);

                    PrintPreviewDialogModel previewDialogModel = new PrintPreviewDialogModel { Document = printDocument };
                    if (ViewService.Instance.ShowDialog(previewDialogModel))
                    {
                        PrintDocument(previewDialogModel.Document);
                    }
                }
                catch
                {
                    MessageBox.Show("An error occured while printing the requirements.", "Error");
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

        public FlowDocument BuildRequirementsPrintDocument(ProjectDialogModel projectViewModel, IEnumerable<RequirementViewModel> functionalRequirements, IEnumerable<RequirementViewModel> nonFunctionalRequirements)
        {
            Section projectSection = new Section(new Paragraph(new Run(projectViewModel.Name)
            {
                FontSize = FontSizeProjectName,
                FontStyle = FontStyleProjectName,
                FontWeight = FontWeightProjectName,
                FontFamily = FontFamilyProjectName,
                Foreground = BrushProjectName
            }));
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

            Section functionalRequiremntsSection = new Section(new Paragraph(new Run("Functional Requirements")
            {
                FontSize = FontSizeRequirementSectionHeader,
                FontStyle = FontStyleRequirementSectionHeader,
                FontWeight = FontWeightRequirementSectionHeader,
                FontFamily = FontFamilyRequirementSectionHeader,
                Foreground = BrushRequirementSectionHeader
            }));

            foreach (var functionalRequiremnt in functionalRequirements)
            {
                Paragraph curRequirementHeaderParagraph = new Paragraph();
                curRequirementHeaderParagraph.Inlines.Add(new Run(functionalRequiremnt.Name)
                {
                    FontSize = FontSizeRequirementName,
                    FontStyle = FontStyleRequirementName,
                    FontWeight = FontWeightRequirementName,
                    FontFamily = FontFamilyRequirementName,
                    Foreground = BrushRequirementName
                });
                curRequirementHeaderParagraph.Inlines.Add(new LineBreak());
                curRequirementHeaderParagraph.Inlines.Add(new Run("Priority: ")
                {
                    FontSize = FontSizeRequirementPriorityHeader,
                    FontStyle = FontStyleRequirementPriorityHeader,
                    FontWeight = FontWeightRequirementPriorityHeader,
                    FontFamily = FontFamilyRequirementPriorityHeader,
                    Foreground = BrushRequirementPriorityHeader
                });
                curRequirementHeaderParagraph.Inlines.Add(new Run(functionalRequiremnt.Priority.ToString())
                {
                    FontSize = FontSizeRequirementPriority,
                    FontStyle = FontStyleRequirementPriority,
                    FontWeight = FontWeightRequirementPriority,
                    FontFamily = FontFamilyRequirementPriority,
                    Foreground = BrushRequirementPriority
                });

                Paragraph curRequirementDescriptionParagraph = new Paragraph();
                curRequirementDescriptionParagraph.Inlines.Add(new Run(functionalRequiremnt.Description)
                {
                    FontSize = FontSizeRequirementDescription,
                    FontStyle = FontStyleRequirementDescription,
                    FontWeight = FontWeightRequirementDescription,
                    FontFamily = FontFamilyRequirementDescription,
                    Foreground = BrushRequirementDescription
                });

                functionalRequiremntsSection.Blocks.Add(curRequirementHeaderParagraph);
                functionalRequiremntsSection.Blocks.Add(curRequirementDescriptionParagraph);
            }

            Section nonFunctionalRequiremntsSection = new Section(new Paragraph(new Run("NonFunctional Requirements")
            {
                FontSize = FontSizeRequirementSectionHeader,
                FontStyle = FontStyleRequirementSectionHeader,
                FontWeight = FontWeightRequirementSectionHeader,
                FontFamily = FontFamilyRequirementSectionHeader,
                Foreground = BrushRequirementSectionHeader
            }));

            foreach (var nonFunctionalRequirement in nonFunctionalRequirements)
            {
                Paragraph curRequirementHeaderParagraph = new Paragraph();
                curRequirementHeaderParagraph.Inlines.Add(new Run(nonFunctionalRequirement.Name)
                {
                    FontSize = FontSizeRequirementName,
                    FontStyle = FontStyleRequirementName,
                    FontWeight = FontWeightRequirementName,
                    FontFamily = FontFamilyRequirementName,
                    Foreground = BrushRequirementName
                });
                curRequirementHeaderParagraph.Inlines.Add(new LineBreak());
                curRequirementHeaderParagraph.Inlines.Add(new Run("Priority: ")
                {
                    FontSize = FontSizeRequirementPriorityHeader,
                    FontStyle = FontStyleRequirementPriorityHeader,
                    FontWeight = FontWeightRequirementPriorityHeader,
                    FontFamily = FontFamilyRequirementPriorityHeader,
                    Foreground = BrushRequirementPriorityHeader
                });
                curRequirementHeaderParagraph.Inlines.Add(new Run(nonFunctionalRequirement.Priority.ToString())
                {
                    FontSize = FontSizeRequirementPriority,
                    FontStyle = FontStyleRequirementPriority,
                    FontWeight = FontWeightRequirementPriority,
                    FontFamily = FontFamilyRequirementPriority,
                    Foreground = BrushRequirementPriority
                });

                Paragraph curRequirementDescriptionParagraph = new Paragraph();
                curRequirementDescriptionParagraph.Inlines.Add(new Run(nonFunctionalRequirement.Description)
                {
                    FontSize = FontSizeRequirementDescription,
                    FontStyle = FontStyleRequirementDescription,
                    FontWeight = FontWeightRequirementDescription,
                    FontFamily = FontFamilyRequirementDescription,
                    Foreground = BrushRequirementDescription
                });

                nonFunctionalRequiremntsSection.Blocks.Add(curRequirementHeaderParagraph);
                nonFunctionalRequiremntsSection.Blocks.Add(curRequirementDescriptionParagraph);
            }


            bool hasFunctionalRequirements = functionalRequirements.Count() > 0;
            bool hasNonFunctionalRequirements = nonFunctionalRequirements.Count() > 0;

            FlowDocument document = new FlowDocument();
            document.Blocks.Add(projectSection);
            if (hasFunctionalRequirements)
                document.Blocks.Add(functionalRequiremntsSection);
            if (hasNonFunctionalRequirements)
            {
                if (hasFunctionalRequirements)
                    nonFunctionalRequiremntsSection.BreakPageBefore = true;

                document.Blocks.Add(nonFunctionalRequiremntsSection);
            }

            return document;
        }
    }
}
