using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using HelperLibrary;
using UICore.Resources.Controls;
using ViewModels;

namespace DTXaml
{
    public class ElementAndSubTableTemplateSelector : DataTemplateSelector
    {

        public DataTemplate ConditionActionDataTemplate { get; set; }
        public Style DTElementItemStyle { get; set; }
        public Style DecisionTableItemStyle { get; set; }
        public bool AreSubTablesSelectable { get; set; }

        private HierarchicalDataTemplate DecisionTableItemTemplate { get; set; }
        private HierarchicalDataTemplate SubTableContainerDataTemplate { get; set; }
        private bool _isInitialized;

        private void Init()
        {
            if (_isInitialized)
                return;

            if (AreSubTablesSelectable)
                BuildSelectableDecisionTableItemTemplate();
            else
                BuildDecisionTableItemTemplate();

            BuildSubTableContainerDataTemplate();

            _isInitialized = true;
        }

        private void BuildSelectableDecisionTableItemTemplate()
        {
            FrameworkElementFactory checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));
            checkBoxFactory.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsSelected"));

            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Name"));
            textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(3, 0, 0, 0));

            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            stackPanelFactory.AppendChild(checkBoxFactory);
            stackPanelFactory.AppendChild(textBlockFactory);

            DecisionTableItemTemplate = new HierarchicalDataTemplate
            {
                ItemsSource = new Binding("ConditionsActionsSubTables"),
                ItemTemplateSelector = this,
                ItemContainerStyle = DTElementItemStyle,
                VisualTree = stackPanelFactory
            };
        }

        private void BuildDecisionTableItemTemplate()
        {
            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Name"));
            DecisionTableItemTemplate = new HierarchicalDataTemplate
            {
                ItemsSource = new Binding("ConditionsActionsSubTables"),
                ItemTemplateSelector = this,
                ItemContainerStyle = DTElementItemStyle,
                VisualTree = textBlockFactory
            };
        }

        private void BuildSubTableContainerDataTemplate()
        {
            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetValue(TextBlock.TextProperty, "Decision Tables");
            SubTableContainerDataTemplate = new HierarchicalDataTemplate
            {
                ItemsSource = new Binding("SubTables"),
                ItemTemplate = DecisionTableItemTemplate,
                ItemContainerStyle = DecisionTableItemStyle,
                VisualTree = textBlockFactory
            };
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Init();

            if (item is SystemConditionViewModel || item is SystemActionViewModel)
                return ConditionActionDataTemplate;
            if (item is SystemSubDecisionTableContainerViewModel)
                return SubTableContainerDataTemplate;
            
            return null;
        }

    }
}
