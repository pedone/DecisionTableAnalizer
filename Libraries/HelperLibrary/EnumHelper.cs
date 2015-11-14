using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelperLibrary
{
    public static class EnumHelper
    {

        public static T GetValue<T>(string value)
            where T : struct
        {
            return GetValue<T>(value, default(T));
        }

        public static T GetValue<T>(string value, T defaultValue)
            where T : struct
        {
            T result;
            if (Enum.TryParse<T>(value, out result))
                return result;

            return defaultValue;
        }

    }
}
