using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using DTEnums;
using ViewDatas;

namespace ViewModels
{
    public class CheckForContradictionDialogModel : ViewModel
    {

        private int _ContradictionCount;
        public int ContradictionCount
        {
            get { return _ContradictionCount; }
            set
            {
                _ContradictionCount = value;
                NotifyPropertyChanged<int>(() => ContradictionCount);
            }
        }

    }
}
