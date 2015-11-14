using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(ActionViewData))]
    public class ActionViewModel : ViewModel<ActionViewData>
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

        private EntityId _ReferenceSubTableId;
        public EntityId ReferenceSubTableId
        {
            get { return _ReferenceSubTableId; }
            private set
            {
                _ReferenceSubTableId = value;
                NotifyPropertyChanged<EntityId>(() => ReferenceSubTableId);
            }
        }

        private bool _HasReferenceSubTable;
        public bool HasReferenceSubTable
        {
            get { return _HasReferenceSubTable; }
            private set
            {
                _HasReferenceSubTable = value;
                NotifyPropertyChanged<bool>(() => HasReferenceSubTable);
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

        private List<StateViewModel> _ValidStates;
        public List<StateViewModel> ValidStates
        {
            get { return _ValidStates; }
            set
            {
                _ValidStates = value;
                NotifyPropertyChanged<List<StateViewModel>>(() => ValidStates);
            }
        }

        private List<StateViewModel> _ValidStatesWithEmptyState;
        public List<StateViewModel> ValidStatesWithEmptyState
        {
            get { return _ValidStatesWithEmptyState; }
            set
            {
                _ValidStatesWithEmptyState = value;
                NotifyPropertyChanged<List<StateViewModel>>(() => ValidStatesWithEmptyState);
            }
        }

        public override void CopyFromViewData(ActionViewData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
            ReferenceSubTableId = viewData.ReferenceSubTableId;
            HasReferenceSubTable = ReferenceSubTableId != null;

            ValidStates = CopyViewModelsFromViewDatas<StateViewData, StateViewModel>(viewData.ValidStates);
            var actionEmptyState = CopyViewModelFromViewData<StateViewData, StateViewModel>(viewData.ActionEmptyState);
            ValidStatesWithEmptyState = ValidStates.ToList();
            ValidStatesWithEmptyState.Add(actionEmptyState);
        }

        public override void CopyToViewData(ActionViewData viewData)
        {
        }

    }
}
