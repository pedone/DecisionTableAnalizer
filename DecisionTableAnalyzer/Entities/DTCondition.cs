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
    public class DTCondition : Entity
    {

        public DecisionTable DecisionTable
        {
            get { return GetValue<DecisionTable>(() => DecisionTable); }
            set { SetValue<DecisionTable>(() => DecisionTable, value); }
        }
        public SubDecisionTable ReferenceSubDecisionTable
        {
            get { return GetValue<SubDecisionTable>(() => ReferenceSubDecisionTable); }
            set { SetValue<SubDecisionTable>(() => ReferenceSubDecisionTable, value); }
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
        public EntityList<DTState> ValidStates
        {
            get { return GetValue<EntityList<DTState>>(() => ValidStates); }
            set { SetValue<EntityList<DTState>>(() => ValidStates, value); }
        }

        public DTCondition()
        {
            ValidStates = new EntityList<DTState>();
        }

        protected override string Validate()
        {
            bool nameAlreadyExists = DecisionTable != null && DecisionTable.Actions.Any(cur => cur.EntityId.Equals(EntityId) == false && cur.Name == Name);
            if (nameAlreadyExists)
                return string.Format("A condition with the name '{0}' already exists.", Name);

            return string.Empty;
        }

    }
}
