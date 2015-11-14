using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelperLibrary
{
    public static class TypeHelper
    {

        public static bool IsSubclassOfRawGeneric(Type genericBaseType, Type genericSubType)
        {
            var genericBaseTypeDefinition = genericBaseType.IsGenericType ? genericBaseType.GetGenericTypeDefinition() : genericBaseType;
            while (genericSubType != typeof(object))
            {
                var cur = genericSubType.IsGenericType ? genericSubType.GetGenericTypeDefinition() : genericSubType;
                if (genericBaseTypeDefinition == cur)
                    return true;

                genericSubType = genericSubType.BaseType;
            }
            return false;
        }


    }
}
