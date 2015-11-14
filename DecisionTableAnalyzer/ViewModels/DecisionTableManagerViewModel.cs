using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(DecisionTableManagerViewData))]
    public class DecisionTableManagerViewModel : ViewModel<DecisionTableManagerViewData>
    {

        private List<DecisionTableInfoViewModel> _DecisionTables;
        public List<DecisionTableInfoViewModel> DecisionTables
        {
            get { return _DecisionTables; }
            set
            {
                _DecisionTables = value;
                NotifyPropertyChanged<List<DecisionTableInfoViewModel>>(() => DecisionTables);
            }
        }

        private DecisionTableInfoViewModel _SelectedDecisionTable;
        public DecisionTableInfoViewModel SelectedDecisionTable
        {
            get { return _SelectedDecisionTable; }
            set
            {
                _SelectedDecisionTable = value;
                NotifyPropertyChanged<DecisionTableInfoViewModel>(() => SelectedDecisionTable);
            }
        }

        public override void CopyToViewData(DecisionTableManagerViewData viewData)
        {
            
        }

        public override void CopyFromViewData(DecisionTableManagerViewData viewData)
        {
            DecisionTables = CopyViewModelsFromViewDatas<DecisionTableInfoViewData, DecisionTableInfoViewModel>(viewData.DecisionTables);
        }
    }
}
