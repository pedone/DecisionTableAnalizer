using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Reflection;
using System.Windows;
using System.ComponentModel;

namespace DTCore
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class ViewModelCommandExtension : MarkupExtension
    {

        public string FullName { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var viewModelCommand = ViewModelService.Instance.GetViewModelCommand(FullName);
            if (viewModelCommand == null)
            {
                if (DesignerProperties.GetIsInDesignMode(Application.Current.MainWindow))
                    return null;
                else
                    throw new ArgumentException(string.Format("No command found of type '{0}'", FullName));
            }

            return new DelegateCommand(viewModelCommand);
        }
    }
}
