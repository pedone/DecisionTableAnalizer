#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections;
#endregion

namespace HelperLibrary
{
    public static class ListHelper
    {

        public static string AggregateItems(IEnumerable<string> items, string seperator, Func<string, string> processItem = null)
        {
            if (items == null || items.ToList().Count == 0)
                return null;

            if (processItem == null)
                processItem = (value) => value;

            string result = string.Empty;
            foreach (string item in items)
                result += seperator + processItem(item);

            return result.Substring(seperator.Length);
        }

        /// <summary>
        /// Clones an IList object and returns a list with the same type as the original list.
        /// </summary>
        public static IList CloneList(IList list)
        {
            if (list == null)
                return null;

            var newList = Activator.CreateInstance(list.GetType()) as IList;
            foreach (var item in list)
                newList.Add(item);

            return newList;
        }

        public static string NumberList(IEnumerable<string> items, int startIndex = 1)
        {
            if (items.Count() == 1)
                return items.First();

            string numberedSenses = string.Empty;
            for (int i = 0; i < items.Count(); i++)
                numberedSenses += String.Format("{0}. {1}  ", i + 1, items.ElementAt(i));

            return numberedSenses.TrimEnd();
        }

    }
}
