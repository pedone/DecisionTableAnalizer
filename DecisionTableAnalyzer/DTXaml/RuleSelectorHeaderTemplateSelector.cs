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
    public class RuleSelectorHeaderTemplateSelector : DataTemplateSelector
    {

        public static RuleSelectorHeaderTemplateSelector Instance { get; private set; }

        static RuleSelectorHeaderTemplateSelector()
        {
            Instance = new RuleSelectorHeaderTemplateSelector();
        }

        private RuleSelectorHeaderTemplateSelector()
        { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var rule = item as RuleViewModel;
            if (rule == null)
                return null;

            FrameworkElementFactory ruleHeaderFactory = new FrameworkElementFactory(typeof(CheckBox));
            Binding isCheckedBinding = new Binding("IsSelected")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            ruleHeaderFactory.SetBinding(CheckBox.IsCheckedProperty, isCheckedBinding);
            return new DataTemplate { VisualTree = ruleHeaderFactory };
        }

    }
}
