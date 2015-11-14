using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore
{

    public class ViewDataTypeAttribute : Attribute
    {
        public Type ViewDataType { get; set; }

        public ViewDataTypeAttribute(Type viewDataType)
        {
            ViewDataType = viewDataType;
        }
    }

}
