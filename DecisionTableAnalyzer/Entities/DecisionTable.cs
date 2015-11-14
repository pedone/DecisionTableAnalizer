using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using ExtensionLibrary;
using System.Collections.Specialized;
using System.Diagnostics;
using DTCore;

namespace Entities
{

    public class DecisionTable : Entity
    {

        public DecisionTableManager DecisionTableManager
        {
            get { return GetValue<DecisionTableManager>(() => DecisionTableManager); }
            set { SetValue<DecisionTableManager>(() => DecisionTableManager, value); }
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
        public EntityList<DTCondition> Conditions
        {
            get { return GetValue<EntityList<DTCondition>>(() => Conditions); }
            set { SetValue<EntityList<DTCondition>>(() => Conditions, value); }
        }
        public EntityList<DTAction> Actions
        {
            get { return GetValue<EntityList<DTAction>>(() => Actions); }
            set { SetValue<EntityList<DTAction>>(() => Actions, value); }
        }
        public EntityList<DTRule> Rules
        {
            get { return GetValue<EntityList<DTRule>>(() => Rules); }
            set { SetValue<EntityList<DTRule>>(() => Rules, value); }
        }

        public EntityList<SubDecisionTable> SubTables
        {
            get { return GetValue<EntityList<SubDecisionTable>>(() => SubTables); }
            set { SetValue<EntityList<SubDecisionTable>>(() => SubTables, value); }
        }

        public DecisionTable()
        {
            Conditions = new EntityList<DTCondition>();
            Actions = new EntityList<DTAction>();
            Rules = new EntityList<DTRule>();
            SubTables = new EntityList<SubDecisionTable>();
        }

        protected override string Validate()
        {
            bool nameAlreadyExists = DecisionTableManager != null && DecisionTableManager.DecisionTables.Any(cur => cur.EntityId.Equals(EntityId) == false && cur.Name == Name);
            if (nameAlreadyExists)
                return string.Format("A decision table with the name '{0}' already exists.", Name);

            return string.Empty;
        }

    }
}
