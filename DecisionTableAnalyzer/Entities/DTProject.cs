using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using ExtensionLibrary;
using System.Xml.Schema;
using System.Xml;
using System.Reflection;
using System.Windows;
using DTCore;

namespace Entities
{
    public class DTProject : Entity
    {

        public string Filename
        {
            get { return GetValue<string>(() => Filename); }
            set { SetValue<string>(() => Filename, value); }
        }
        public string Name
        {
            get { return GetValue<string>(() => Name); }
            set { SetValue<string>(() => Name, value); }
        }
        public string Description
        {
            get { return GetValue<string>(() => Description); }
            set { SetValue<string>(() => Description, value); }
        }

        public DecisionTableManager DecisionTableManager
        {
            get { return GetValue<DecisionTableManager>(() => DecisionTableManager); }
            set { SetValue<DecisionTableManager>(() => DecisionTableManager, value); }
        }
        public RequirementManager RequirementManager
        {
            get { return GetValue<RequirementManager>(() => RequirementManager); }
            set { SetValue<RequirementManager>(() => RequirementManager, value); }
        }

    }
}
