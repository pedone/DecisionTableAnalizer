using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using DTEnums;
using ViewDatas;

namespace ViewModels
{
    public class CheckForCompletenessDialogModel : ViewModel
    {

        private int _MissingRulesCount;
        public int MissingRulesCount
        {
            get { return _MissingRulesCount; }
            set
            {
                _MissingRulesCount = value;
                NotifyPropertyChanged<int>(() => MissingRulesCount);
            }
        }

    }
}
