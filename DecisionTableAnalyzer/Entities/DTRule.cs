using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ExtensionLibrary;
using System.Diagnostics;
using DTCore;

namespace Entities
{

    public class DTRule : Entity
    {

        public Dictionary<DTCondition, DTState> ConditionStates
        {
            get { return GetValue<Dictionary<DTCondition, DTState>>(() => ConditionStates); }
            set { SetValue<Dictionary<DTCondition, DTState>>(() => ConditionStates, value); }
        }
        public Dictionary<DTAction, DTState> ActionStates
        {
            get { return GetValue<Dictionary<DTAction, DTState>>(() => ActionStates); }
            set { SetValue<Dictionary<DTAction, DTState>>(() => ActionStates, value); }
        }
        public int Index
        {
            get { return GetValue<int>(() => Index); }
            set { SetValue<int>(() => Index, value); }
        }

        public DTRule()
        {
            ConditionStates = new Dictionary<DTCondition, DTState>();
            ActionStates = new Dictionary<DTAction, DTState>();
        }

    }
}
