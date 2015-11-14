using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(DecisionTableInfoViewData))]
    public class DecisionTableInfoViewModel : ViewModel<DecisionTableInfoViewData>
    {

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

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                NotifyPropertyChanged<string>(() => Description);
            }
        }

        private int _ConditionCount;
        public int ConditionCount
        {
            get { return _ConditionCount; }
            set
            {
                _ConditionCount = value;
                NotifyPropertyChanged<int>(() => ConditionCount);
            }
        }

        private int _ActionCount;
        public int ActionCount
        {
            get { return _ActionCount; }
            set
            {
                _ActionCount = value;
                NotifyPropertyChanged<int>(() => ActionCount);
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

        public override void CopyToViewData(DecisionTableInfoViewData viewData)
        {
        }

        public override void CopyFromViewData(DecisionTableInfoViewData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
            ConditionCount = viewData.ConditionCount;
            ActionCount = viewData.ActionCount;
        }

    }
}
