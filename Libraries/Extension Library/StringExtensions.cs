using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionLibrary
{
    public static class StringExtensions
    {

        private delegate bool ConverterDelegate<T>(string source, out T result);

        public static void Print(this string source)
        {
            Console.WriteLine(source);
        }

        public static void MessageBox(this string source)
        {
            System.Windows.MessageBox.Show(source);
        }

        /// <summary>
        /// Checks if the string is null or empty.
        /// </summary>
        public static bool IsEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        private static Nullable<T> ConvertToType<T>(string source, ConverterDelegate<T> converterFunction)
            where T : struct
        {
            if (source == null)
                return null;

            T result;
            if (converterFunction(source, out result))
                return result;

            return null;
        }

        public static int? ToInt(this string source)
        {
            return ConvertToType<int>(source, int.TryParse);
        }

        public static bool? ToBool(this string source)
        {
            return ConvertToType<bool>(source, bool.TryParse);
        }

        public static float? ToFloat(this string source)
        {
            return ConvertToType<float>(source, float.TryParse);
        }

        public static double? ToDouble(this string source)
        {
            return ConvertToType<double>(source, double.TryParse);
        }

        public static int ToInt(this string source, int defaultValue)
        {
            Nullable<int> result = ConvertToType<int>(source, int.TryParse);
            if (result.HasValue)
                return result.Value;

            return defaultValue;
        }

        public static bool ToBool(this string source, bool defaultValue)
        {
            Nullable<bool> result = ConvertToType<bool>(source, bool.TryParse);
            if (result.HasValue)
                return result.Value;

            return defaultValue;
        }

        public static float ToFloat(this string source, int defaultValue)
        {
            Nullable<float> result = ConvertToType<float>(source, float.TryParse);
            if (result.HasValue)
                return result.Value;

            return defaultValue;
        }

        public static double ToDouble(this string source, int defaultValue)
        {
            Nullable<double> result = ConvertToType<double>(source, double.TryParse);
            if (result.HasValue)
                return result.Value;

            return defaultValue;
        }

    }
}
