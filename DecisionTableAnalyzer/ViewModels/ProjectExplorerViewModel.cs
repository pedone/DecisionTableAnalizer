using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DockingLibrary;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(ProjectExplorerViewData))]
    public class ProjectExplorerViewModel : ViewModel<ProjectExplorerViewData>
    {

        public SystemProjectViewModel Project { get; private set; }
        public SystemDecisionTableManagerViewModel DecisionTableManager { get; private set; }
        public SystemRequirementManagerViewModel RequirementManager { get; private set; }

        private int _RequirementCount;
        public int RequirementCount
        {
            get { return _RequirementCount; }
            private set
            {
                _RequirementCount = value;
                NotifyPropertyChanged<int>(() => RequirementCount);
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

        public override void CopyFromViewData(ProjectExplorerViewData viewData)
        {
            Project = CopyViewModelFromViewData<SystemProjectViewData, SystemProjectViewModel>(viewData.Project);
            DecisionTableManager = CopyViewModelFromViewData<SystemDecisionTableManagerViewData, SystemDecisionTableManagerViewModel>(viewData.DecisionTableManager);
            RequirementManager = CopyViewModelFromViewData<SystemRequirementManagerViewData, SystemRequirementManagerViewModel>(viewData.RequirementManager);
        }

        public override void CopyToViewData(ProjectExplorerViewData viewData)
        {
        }

    }
}
