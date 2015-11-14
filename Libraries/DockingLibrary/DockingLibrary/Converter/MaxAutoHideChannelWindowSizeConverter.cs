using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Documents;

namespace DockingLibrary.Converter
{

    public class MaxAutoHideChannelWindowSizeConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double windowSize = System.Convert.ToDouble(values[0]);
                double channelSize = System.Convert.ToDouble(values[1]);

                double minOffsetToBorder = System.Convert.ToDouble(parameter);
                return Math.Max(0, (windowSize - channelSize) - minOffsetToBorder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MaxAutoHideChannelWindowSizeConverter.Convert()\n" + ex.Message);
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }

}
