using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;
using System.Collections.ObjectModel;

namespace ViewModels
{

    public class ContradictionViewModel : ViewModel
    {

        private List<RuleViewModel> _Rules;
        public List<RuleViewModel> Rules
        {
            get { return _Rules; }
            set
            {
                _Rules = value;
                NotifyPropertyChanged<List<RuleViewModel>>(() => Rules);
            }
        }

        private RuleViewModel _Header;
        public RuleViewModel Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
                NotifyPropertyChanged<RuleViewModel>(() => Header);
            }
        }

        private List<bool?> _ContradictionStates;
        public List<bool?> ContradictionStates
        {
            get { return _ContradictionStates; }
            set
            {
                _ContradictionStates = value;
                NotifyPropertyChanged<List<bool?>>(() => ContradictionStates);
            }
        }

    }
}
