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

    public class SubDecisionTable : DecisionTable
    {

        public DTAction ReferenceAction
        {
            get { return GetValue<DTAction>(() => ReferenceAction); }
            set { SetValue<DTAction>(() => ReferenceAction, value); }
        }

        public DTCondition ReferenceCondition
        {
            get { return GetValue<DTCondition>(() => ReferenceCondition); }
            set { SetValue<DTCondition>(() => ReferenceCondition, value); }
        }

        protected override string Validate()
        {
            if (ReferenceAction == null && ReferenceCondition == null)
                return "A sub decision table must be referencing an action or a condition.";

            return string.Empty;
        }

        protected override void OnDelete()
        {
            if (ReferenceAction != null)
                ReferenceAction.ReferenceSubDecisionTable = null;
            if (ReferenceCondition != null)
                ReferenceCondition.ReferenceSubDecisionTable = null;
        }

    }
}
