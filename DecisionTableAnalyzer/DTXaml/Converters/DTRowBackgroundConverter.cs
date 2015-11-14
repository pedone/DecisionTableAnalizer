using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using ViewModels;

namespace DTXaml.Converters
{
    public class DTRowBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var row = value as RowViewModel;
            if (row == null)
                return Brushes.White;

            if (row.Header is ActionViewModel)
                return Brushes.LightGray;

            return Brushes.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
