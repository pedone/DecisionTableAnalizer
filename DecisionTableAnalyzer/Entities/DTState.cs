using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ExtensionLibrary;
using DTCore;

namespace Entities
{
    public class DTState : Entity
    {

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

        //protected override string Validate()
        //{
        //    bool nameAlreadyExists = DecisionTable != null && DecisionTable.Actions.Any(cur => cur.EntityId.Equals(EntityId) == false && cur.Name == Name);
        //    if (nameAlreadyExists)
        //        return string.Format("An action with the name '{0}' already exists.", Name);

        //    return string.Empty;
        //}

    }
}
