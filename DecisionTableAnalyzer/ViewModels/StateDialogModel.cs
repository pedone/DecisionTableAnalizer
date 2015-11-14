using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using DTEnums;
using ViewDatas;

namespace ViewModels
{
    public class StateDialogModel : ViewModel
    {

        private EntityId _DecisionTableManagerId;
        public EntityId DecisionTableManagerId
        {
            get { return _DecisionTableManagerId; }
            set
            {
                _DecisionTableManagerId = value;
                NotifyPropertyChanged<EntityId>(() => DecisionTableManagerId);
            }
        }

        private StateViewModel _NewState;
        public StateViewModel NewState
        {
            get { return _NewState; }
            set
            {
                _NewState = value;
                NotifyPropertyChanged<StateViewModel>(() => NewState);
            }
        }

        private List<StateViewModel> _ExistingStates;
        public List<StateViewModel> ExistingStates
        {
            get { return _ExistingStates; }
            set
            {
                _ExistingStates = value;
                NotifyPropertyChanged<List<StateViewModel>>(() => ExistingStates);
            }
        }

        public StateDialogModel()
        {
            ResetNewStateViewModel();
        }

        public void ResetNewStateViewModel()
        {
            NewState = new StateViewModel();
        }

    }
}
