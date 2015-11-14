using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ExtensionLibrary;

namespace ViewModels
{
    public class ContradictionGroup
    {
        public RuleViewModel RuleA { get; set; }
        public RuleViewModel RuleB { get; set; }

        public bool IsEquivalent(ContradictionGroup group)
        {
            return (RuleA == group.RuleA || RuleA == group.RuleB) &&
                (RuleB == group.RuleA || RuleB == group.RuleB);
        }

    }
}
