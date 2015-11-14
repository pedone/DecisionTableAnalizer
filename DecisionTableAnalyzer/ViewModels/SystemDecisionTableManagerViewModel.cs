using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(SystemDecisionTableManagerViewData))]
    public class SystemDecisionTableManagerViewModel : ViewModel<SystemDecisionTableManagerViewData>
    {

        private List<SystemDecisionTableViewModel> _DecisionTables;
        public List<SystemDecisionTableViewModel> DecisionTables
        {
            get { return _DecisionTables; }
            set
            {
                _DecisionTables = value;
                NotifyPropertyChanged<List<SystemDecisionTableViewModel>>(() => DecisionTables);
            }
        }

        public override void CopyToViewData(SystemDecisionTableManagerViewData viewData)
        {
        }

        public override void CopyFromViewData(SystemDecisionTableManagerViewData viewData)
        {
            DecisionTables = CopyViewModelsFromViewDatas<SystemDecisionTableViewData, SystemDecisionTableViewModel>(viewData.DecisionTables);
        }

    }
}
