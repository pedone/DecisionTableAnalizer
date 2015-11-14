using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(SystemRequirementManagerViewData))]
    public class SystemRequirementManagerViewModel : ViewModel<SystemRequirementManagerViewData>
    {

        private List<SystemRequirementViewModel> _FunctionalRequirements;
        public List<SystemRequirementViewModel> FunctionalRequirements
        {
            get { return _FunctionalRequirements; }
            set
            {
                _FunctionalRequirements = value;
                NotifyPropertyChanged<List<SystemRequirementViewModel>>(() => FunctionalRequirements);
            }
        }

        private List<SystemRequirementViewModel> _NonFunctionalRequirements;
        public List<SystemRequirementViewModel> NonFunctionalRequirements
        {
            get { return _NonFunctionalRequirements; }
            set
            {
                _NonFunctionalRequirements = value;
                NotifyPropertyChanged<List<SystemRequirementViewModel>>(() => NonFunctionalRequirements);
            }
        }

        private int _RequirementCount;
        public int RequirementCount
        {
            get { return _RequirementCount; }
            set
            {
                _RequirementCount = value;
                NotifyPropertyChanged<int>(() => RequirementCount);
            }
        }

        public override void CopyToViewData(SystemRequirementManagerViewData viewData)
        {
        }

        public override void CopyFromViewData(SystemRequirementManagerViewData viewData)
        {
            FunctionalRequirements = CopyViewModelsFromViewDatas<SystemRequirementViewData, SystemRequirementViewModel>(viewData.FunctionalRequirements);
            NonFunctionalRequirements = CopyViewModelsFromViewDatas<SystemRequirementViewData, SystemRequirementViewModel>(viewData.NonFunctionalRequirements);
            RequirementCount = FunctionalRequirements.Count + NonFunctionalRequirements.Count;
        }

    }
}
