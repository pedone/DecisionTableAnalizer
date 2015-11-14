using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ExtensionLibrary
{
    public static class XElementExtensions
    {

        public static string GetElementValue(this XElement source, string elementPath)
        {
            if (source == null)
                return null;

            if (elementPath.IsEmpty())
                return source.Value;

            XElement subElement = source.Element(elementPath);
            if (subElement != null)
                return subElement.Value;

            return null;
        }

        public static string GetAttributeValue(this XElement source, string attributePath)
        {
            if (source == null)
                return null;

            if (attributePath.IsEmpty())
                return source.Value;

            XAttribute attribute = source.Attribute(attributePath);
            if (attribute != null)
                return attribute.Value;

            return null;
        }

    }
}
