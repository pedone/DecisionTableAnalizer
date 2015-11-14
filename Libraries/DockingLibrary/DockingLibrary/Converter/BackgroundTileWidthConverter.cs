using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DockingLibrary.Converter
{

    [ValueConversion(typeof(float), typeof(float))]
    internal class BackgroundTileWidthConverter : IValueConverter
    {
        private const int TileWidth = 4;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;


            try
            {
                float width = (float)System.Convert.ToDouble(value);
                return TileWidth / width;
            }
            catch
            {
                return 0f;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
