using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DockingLibrary.Converter
{

    [ValueConversion(typeof(DocumentGroup), typeof(IEnumerable<DocumentViewTabItemModel>))]
    internal class ViewToOpenViewDocumentCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DocumentGroup documentGroup = value as DocumentGroup;
            if (documentGroup == null || documentGroup.Items.Count == 0)
                return null;

            return from view in documentGroup.Items
                   select new DocumentViewTabItemModel(view, documentGroup);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
