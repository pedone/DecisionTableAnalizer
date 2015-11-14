using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using DTCore;

namespace Entities
{
    public class RequirementManager : Entity
    {

        public DTProject Project
        {
            get { return GetValue<DTProject>(() => Project); }
            set { SetValue<DTProject>(() => Project, value); }
        }

        public EntityList<Requirement> FunctionalRequirements
        {
            get { return GetValue<EntityList<Requirement>>(() => FunctionalRequirements); }
            set { SetValue<EntityList<Requirement>>(() => FunctionalRequirements, value); }
        }

        public EntityList<Requirement> NonFunctionalRequirements
        {
            get { return GetValue<EntityList<Requirement>>(() => NonFunctionalRequirements); }
            set { SetValue<EntityList<Requirement>>(() => NonFunctionalRequirements, value); }
        }

        public RequirementManager()
        {
            FunctionalRequirements = new EntityList<Requirement>();
            NonFunctionalRequirements = new EntityList<Requirement>();
        }

    }
}
