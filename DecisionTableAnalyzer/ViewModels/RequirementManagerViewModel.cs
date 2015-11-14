using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(RequirementManagerViewData))]
    public class RequirementManagerViewModel : ViewModel<RequirementManagerViewData>
    {

        private List<RequirementViewModel> _FunctionalRequirements;
        public List<RequirementViewModel> FunctionalRequirements
        {
            get { return _FunctionalRequirements; }
            set
            {
                _FunctionalRequirements = value;
                NotifyPropertyChanged<List<RequirementViewModel>>(() => FunctionalRequirements);
            }
        }

        private List<RequirementViewModel> _NonFunctionalRequirements;
        public List<RequirementViewModel> NonFunctionalRequirements
        {
            get { return _NonFunctionalRequirements; }
            set
            {
                _NonFunctionalRequirements = value;
                NotifyPropertyChanged<List<RequirementViewModel>>(() => NonFunctionalRequirements);
            }
        }

        private RequirementViewModel _SelectedRequirement;
        public RequirementViewModel SelectedRequirement
        {
            get { return _SelectedRequirement; }
            set
            {
                _SelectedRequirement = value;
                NotifyPropertyChanged<RequirementViewModel>(() => SelectedRequirement);
            }
        }

        public override void CopyFromViewData(RequirementManagerViewData viewData)
        {
            FunctionalRequirements = CopyViewModelsFromViewDatas<RequirementViewData, RequirementViewModel>(viewData.FunctionalRequirements);
            NonFunctionalRequirements = CopyViewModelsFromViewDatas<RequirementViewData, RequirementViewModel>(viewData.NonFunctionalRequirements);
        }

        public override void CopyToViewData(RequirementManagerViewData viewData)
        {
            
        }

    }
}
