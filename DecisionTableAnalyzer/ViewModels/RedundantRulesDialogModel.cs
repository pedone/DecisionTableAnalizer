using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;

namespace ViewModels
{
    public class RedundantRulesDialogModel : ViewModel
    {

        public IEnumerable<DTRule> RedundantRules { get; set; }

    }
}
