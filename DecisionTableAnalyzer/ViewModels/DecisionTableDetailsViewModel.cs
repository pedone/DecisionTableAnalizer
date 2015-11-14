using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(DecisionTableDetailsViewData))]
    public class DecisionTableDetailsViewModel : ViewModel<DecisionTableDetailsViewData>
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

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged<string>(() => Name);
            }
        }

        private List<ConditionViewModel> _Conditions;
        public List<ConditionViewModel> Conditions
        {
            get { return _Conditions; }
            set
            {
                _Conditions = value;
                NotifyPropertyChanged<List<ConditionViewModel>>(() => Conditions);
            }
        }

        private List<ActionViewModel> _Actions;
        public List<ActionViewModel> Actions
        {
            get { return _Actions; }
            set
            {
                _Actions = value;
                NotifyPropertyChanged<List<ActionViewModel>>(() => Actions);
            }
        }

        private ConditionViewModel _SelectedCondition;
        public ConditionViewModel SelectedCondition
        {
            get { return _SelectedCondition; }
            set
            {
                if (value == null && SelectedElement == _SelectedCondition)
                    SelectedElement = null;

                _SelectedCondition = value;
                NotifyPropertyChanged<ConditionViewModel>(() => SelectedCondition);

                if (_SelectedCondition != null)
                    SelectedElement = _SelectedCondition;
            }
        }

        private ActionViewModel _SelectedAction;
        public ActionViewModel SelectedAction
        {
            get { return _SelectedAction; }
            set
            {
                if (value == null && SelectedElement == _SelectedAction)
                    SelectedElement = null;

                _SelectedAction = value;
                NotifyPropertyChanged<ActionViewModel>(() => SelectedAction);

                if (_SelectedAction != null)
                    SelectedElement = _SelectedAction;
            }
        }

        private ViewModel _SelectedElement;
        public ViewModel SelectedElement
        {
            get { return _SelectedElement; }
            set
            {
                _SelectedElement = value;
                NotifyPropertyChanged<ViewModel>(() => SelectedElement);
            }
        }

        private StateViewModel _SelectedConditionState;
        public StateViewModel SelectedConditionState
        {
            get { return _SelectedConditionState; }
            set
            {
                _SelectedConditionState = value;
                NotifyPropertyChanged<StateViewModel>(() => SelectedConditionState);
            }
        }

        private StateViewModel _SelectedActionState;
        public StateViewModel SelectedActionState
        {
            get { return _SelectedActionState; }
            set
            {
                _SelectedActionState = value;
                NotifyPropertyChanged<StateViewModel>(() => SelectedActionState);
            }
        }

        public override void CopyToViewData(DecisionTableDetailsViewData viewData)
        {
            viewData.Name = Name;
        }

        public override void CopyFromViewData(DecisionTableDetailsViewData viewData)
        {
            DecisionTableManagerId = viewData.DecisionTableManagerId;
            Name = viewData.Name;

            Conditions = CopyViewModelsFromViewDatas<ConditionViewData, ConditionViewModel>(viewData.Conditions);
            Actions = CopyViewModelsFromViewDatas<ActionViewData, ActionViewModel>(viewData.Actions);
        }
    }
}
