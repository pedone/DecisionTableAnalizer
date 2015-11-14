using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections;

namespace DTXaml.Converters
{
    public class SumUpConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
                return 0;

            var addendList = values.OfType<Int64>();
            if (addendList.Count() == 0)
                return 0;

            return addendList.Aggregate((sum, next) => sum + next);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
