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
    [ViewDataType(typeof(PrintDecisionTablesDialogData))]
    public class PrintDecisionTablesDialogModel : ViewModel<PrintDecisionTablesDialogData>
    {

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

        public override void CopyFromViewData(PrintDecisionTablesDialogData viewData)
        {
            DecisionTables = CopyViewModelsFromViewDatas<SystemDecisionTableViewData, SystemDecisionTableViewModel>(viewData.DecisionTables);
        }

        public override void CopyToViewData(PrintDecisionTablesDialogData viewData)
        {
        }

    }
}
