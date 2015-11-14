using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HelperLibrary
{
    public static class XmlHelper
    {

        /// <summary>
        /// Creates an XElement, but returns null if the value is empty.
        /// </summary>
        public static XElement CreateXElement(string elementName, string value, bool createWithEmptyValue = false)
        {
            if (String.IsNullOrEmpty(elementName))
                return null;

            if (String.IsNullOrEmpty(value))
            {
                if (createWithEmptyValue)
                    return new XElement(elementName);
                else
                    return null;
            }

            return new XElement(elementName, value);
        }

        async public static Task<Dictionary<string, string>> LoadDTDEntitiesAsync(string xmlfile)
        {
            return await Task.Run<Dictionary<string, string>>(() => LoadDTDEntities(xmlfile));
        }

        public static Dictionary<string, string> LoadDTDEntities(string xmlfile)
        {
            if (!File.Exists(xmlfile))
                return null;

            Task<Dictionary<string, string>> result = Task.Run<Dictionary<string, string>>(() =>
                            {
                                try
                                {
                                    using (FileStream fs = new FileStream(xmlfile, FileMode.Open))
                                    using (StreamReader reader = new StreamReader(fs))
                                    {
                                        Dictionary<string, string> entities = new Dictionary<string, string>();
                                        while (!reader.EndOfStream)
                                        {
                                            string line = reader.ReadLine();
                                            if (!line.Contains("<!ENTITY"))
                                                continue;

                                            line = line.Replace("<!ENTITY ", "").Replace("\">", "");
                                            string entity = line.Substring(0, line.IndexOf(" "));
                                            string entityVal = line.Substring(line.IndexOf("\"") + 1);
                                            entities.Add(entity, entityVal);
                                        }

                                        return entities;
                                    }
                                }
                                catch
                                {
                                    return null;
                                }
                            });

            return result.Result;
        }



    }
}
