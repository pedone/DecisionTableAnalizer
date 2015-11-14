using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ExtensionLibrary;
using HelperLibrary;
using DTCore;
using DTEnums;

namespace Entities
{
    public class Requirement : Entity
    {

        public RequirementManager RequirementManager
        {
            get { return GetValue<RequirementManager>(() => RequirementManager); }
            set { SetValue<RequirementManager>(() => RequirementManager, value); }
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
        public RequirementKind Kind
        {
            get { return GetValue<RequirementKind>(() => Kind); }
            set { SetValue<RequirementKind>(() => Kind, value); }
        }
        public Priority Priority
        {
            get { return GetValue<Priority>(() => Priority); }
            set { SetValue<Priority>(() => Priority, value); }
        }

        protected override string Validate()
        {
            bool nameAlreadyExists = RequirementManager != null &&
                (Kind == RequirementKind.Functional && RequirementManager.FunctionalRequirements.Any(cur => cur.EntityId.Equals(EntityId) == false && cur.Name == Name) ||
                Kind == RequirementKind.NonFunctional && RequirementManager.NonFunctionalRequirements.Any(cur => cur.EntityId.Equals(EntityId) == false && cur.Name == Name));

            if (nameAlreadyExists)
                return string.Format("A requirement with the name '{0}' already exists.", Name);

            return string.Empty;
        }

    }
}
