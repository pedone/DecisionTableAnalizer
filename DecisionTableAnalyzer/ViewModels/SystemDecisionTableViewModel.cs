using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;
using System.Windows.Data;

namespace ViewModels
{
    [ViewDataType(typeof(SystemDecisionTableViewData))]
    public class SystemDecisionTableViewModel : ViewModel<SystemDecisionTableViewData>
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

        private List<SystemActionViewModel> _Actions;
        public List<SystemActionViewModel> Actions
        {
            get { return _Actions; }
            set
            {
                _Actions = value;
                NotifyPropertyChanged<List<SystemActionViewModel>>(() => Actions);
            }
        }

        private List<SystemConditionViewModel> _Conditions;
        public List<SystemConditionViewModel> Conditions
        {
            get { return _Conditions; }
            set
            {
                _Conditions = value;
                NotifyPropertyChanged<List<SystemConditionViewModel>>(() => Conditions);
            }
        }

        private List<SystemDecisionTableViewModel> _SubTables;
        public List<SystemDecisionTableViewModel> SubTables
        {
            get { return _SubTables; }
            set
            {
                _SubTables = value;
                NotifyPropertyChanged<List<SystemDecisionTableViewModel>>(() => SubTables);
            }
        }

        private CompositeCollection _ConditionsActionsSubTables;
        public CompositeCollection ConditionsActionsSubTables
        {
            get { return _ConditionsActionsSubTables; }
            set
            {
                _ConditionsActionsSubTables = value;
                NotifyPropertyChanged<CompositeCollection>(() => ConditionsActionsSubTables);
            }
        }

        private SystemSubDecisionTableContainerViewModel _SubTableContainer;
        public SystemSubDecisionTableContainerViewModel SubTableContainer
        {
            get { return _SubTableContainer; }
            set
            {
                _SubTableContainer = value;
                NotifyPropertyChanged<SystemSubDecisionTableContainerViewModel>(() => SubTableContainer);
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                NotifyPropertyChanged<string>(() => Name);
            }
        }

        public override void CopyToViewData(SystemDecisionTableViewData viewData)
        {
        }

        public override void CopyFromViewData(SystemDecisionTableViewData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
            Conditions = CopyViewModelsFromViewDatas<SystemConditionViewData, SystemConditionViewModel>(viewData.Conditions);
            Actions = CopyViewModelsFromViewDatas<SystemActionViewData, SystemActionViewModel>(viewData.Actions);
            SubTables = CopyViewModelsFromViewDatas<SystemDecisionTableViewData, SystemDecisionTableViewModel>(viewData.SubTables);
            SubTableContainer = new SystemSubDecisionTableContainerViewModel { SubTables = SubTables };

            ConditionsActionsSubTables = new CompositeCollection(3);
            if (SubTableContainer.SubTables.Count > 0)
                ConditionsActionsSubTables.Add(new CollectionContainer { Collection = new[] { SubTableContainer } });

            ConditionsActionsSubTables.Add(new CollectionContainer { Collection = Conditions });
            ConditionsActionsSubTables.Add(new CollectionContainer { Collection = Actions });
        }

    }
}
