using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using DTEnums;
using ViewDatas;

namespace ViewModels
{
    public class CheckForRedundancyDialogModel : ViewModel
    {

        private int _RedundantRulesCount;
        public int RedundantRulesCount
        {
            get { return _RedundantRulesCount; }
            set
            {
                _RedundantRulesCount = value;
                NotifyPropertyChanged<int>(() => RedundantRulesCount);
            }
        }

    }
}
