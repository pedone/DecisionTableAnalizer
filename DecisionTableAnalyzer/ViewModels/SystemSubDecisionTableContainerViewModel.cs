using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;
using System.Windows.Data;

namespace ViewModels
{
    public class SystemSubDecisionTableContainerViewModel : ViewModel
    {

        private List<SystemDecisionTableViewModel> _SubTables;
        public List<SystemDecisionTableViewModel> SubTables
        {
            get { return _SubTables; }
            set
            {
                _SubTables = value;
                NotifyPropertyChanged<List<SystemDecisionTableViewModel>>(() => SubTables);
            }
        }

    }
}
