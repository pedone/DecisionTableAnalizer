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
    public class ContradictionSelectionHeaderTemplateSelector : DataTemplateSelector
    {

        public static ContradictionSelectionHeaderTemplateSelector Instance { get; private set; }

        static ContradictionSelectionHeaderTemplateSelector()
        {
            Instance = new ContradictionSelectionHeaderTemplateSelector();
        }

        private ContradictionSelectionHeaderTemplateSelector()
        { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var rule = item as RuleViewModel;
            if (rule == null)
                return null;

            FrameworkElementFactory checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));
            Binding isCheckedBinding = new Binding("IsSelected")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            checkBoxFactory.SetBinding(CheckBox.IsCheckedProperty, isCheckedBinding);

            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            textBlockFactory.SetValue(TextBlock.TextProperty, (rule.Index + 1).ToString());
            textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(0, 5, 0, 0));

            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.AppendChild(checkBoxFactory);
            stackPanelFactory.AppendChild(textBlockFactory);

            return new DataTemplate { VisualTree = stackPanelFactory };
        }

    }
}
