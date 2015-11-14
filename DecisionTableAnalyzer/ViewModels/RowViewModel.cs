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

    public class RowViewModel : ViewModel
    {

        private ViewModel _Header;
        public ViewModel Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
                NotifyPropertyChanged<ViewModel>(() => Header);
            }
        }

        private ObservableCollection<StateViewModel> _States;
        public ObservableCollection<StateViewModel> States
        {
            get { return _States; }
            set
            {
                _States = value;
                NotifyPropertyChanged<ObservableCollection<StateViewModel>>(() => States);
            }
        }

    }
}
