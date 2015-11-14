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

namespace DecisionTableAnalyzer.Commands
{
    public class ApplicationViewPrintRequirementsCommand : DTCommand<ApplicationViewModel>
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
            return contextViewModel != null && contextViewModel.Project != null;
        }

        public override void Execute(ApplicationViewModel contextViewModel)
        {
            if (contextViewModel.Project.RequirementManager.Requirements.Count() == 0)
            {
                MessageBox.Show("The project does not contain any requirements.", "No requirements");
                return;
            }

            try
            {
                PrintRequirements(contextViewModel);
            }
            catch
            {
                MessageBox.Show("An error occured while printing the requirements.", "Error");
            }
        }

        public void PrintRequirements(ApplicationViewModel contextViewModel)
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

            Section functionalRequiremntsSection = new Section(new Paragraph(new Run("Functional Requirements")
            {
                FontSize = FontSizeRequirementSectionHeader,
                FontStyle = FontStyleRequirementSectionHeader,
                FontWeight = FontWeightRequirementSectionHeader,
                FontFamily = FontFamilyRequirementSectionHeader,
                Foreground = BrushRequirementSectionHeader
            }));
            if (project.RequirementManager.FunctionalRequirements.Count() > 0)
            {
                foreach (var functionalRequiremnt in project.RequirementManager.FunctionalRequirements)
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
            }
            else
            {
                functionalRequiremntsSection.Blocks.Add(new Paragraph(new Run("There are no functional requirements.") { FontStyle = FontStyles.Italic }));
            }

            Section nonFunctionalRequiremntsSection = new Section(new Paragraph(new Run("NonFunctional Requirements")
            {
                FontSize = FontSizeRequirementSectionHeader,
                FontStyle = FontStyleRequirementSectionHeader,
                FontWeight = FontWeightRequirementSectionHeader,
                FontFamily = FontFamilyRequirementSectionHeader,
                Foreground = BrushRequirementSectionHeader
            }));
            if (project.RequirementManager.NonFunctionalRequirements.Count() > 0)
            {
                foreach (var nonFunctionalRequiremnt in project.RequirementManager.NonFunctionalRequirements)
                {
                    Paragraph curRequirementHeaderParagraph = new Paragraph();
                    curRequirementHeaderParagraph.Inlines.Add(new Run(nonFunctionalRequiremnt.Name)
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
                    curRequirementHeaderParagraph.Inlines.Add(new Run(nonFunctionalRequiremnt.Priority.ToString())
                    {
                        FontSize = FontSizeRequirementPriority,
                        FontStyle = FontStyleRequirementPriority,
                        FontWeight = FontWeightRequirementPriority,
                        FontFamily = FontFamilyRequirementPriority,
                        Foreground = BrushRequirementPriority
                    });

                    Paragraph curRequirementDescriptionParagraph = new Paragraph();
                    curRequirementDescriptionParagraph.Inlines.Add(new Run(nonFunctionalRequiremnt.Description)
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
            }
            else
            {
                nonFunctionalRequiremntsSection.Blocks.Add(new Paragraph(new Run("There are no nonfunctional requirements.") { FontStyle = FontStyles.Italic }));
            }


            nonFunctionalRequiremntsSection.BreakPageBefore = true;

            FlowDocument document = new FlowDocument();
            document.Blocks.Add(projectSection);
            document.Blocks.Add(functionalRequiremntsSection);
            document.Blocks.Add(nonFunctionalRequiremntsSection);

            PrintRequirementsDialogModel dialogModel = new PrintRequirementsDialogModel { RequirementsDocument = document };
            PrintRequirementsDialog dialog = new PrintRequirementsDialog { Owner = App.Current.MainWindow, DataContext = dialogModel };
            dialog.ShowDialog();
        }
    }
}
