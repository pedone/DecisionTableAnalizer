using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using DTCore;

namespace Entities
{
    public class DecisionTableManager : Entity
    {

        public DTProject Project
        {
            get { return GetValue<DTProject>(() => Project); }
            set { SetValue<DTProject>(() => Project, value); }
        }
        public EntityList<DecisionTable> DecisionTables
        {
            get { return GetValue<EntityList<DecisionTable>>(() => DecisionTables); }
            set { SetValue<EntityList<DecisionTable>>(() => DecisionTables, value); }
        }
        public EntityList<DTState> States
        {
            get { return GetValue<EntityList<DTState>>(() => States); }
            set { SetValue<EntityList<DTState>>(() => States, value); }
        }
        public DTState EmptyState
        {
            get { return GetValue<DTState>(() => EmptyState); }
            set { SetValue<DTState>(() => EmptyState, value); }
        }
        public DTState NoPreferenceState
        {
            get { return GetValue<DTState>(() => NoPreferenceState); }
            set { SetValue<DTState>(() => NoPreferenceState, value); }
        }

        public DecisionTableManager()
        {
            DecisionTables = new EntityList<DecisionTable>();
            States = new EntityList<DTState>();

            EmptyState = EntityService.CreateNew<DTState>();
            NoPreferenceState = EntityService.CreateNew<DTState>();
            NoPreferenceState.Name = "-";
        }

    }
}
