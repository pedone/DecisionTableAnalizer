using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using DTEnums;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(ActionDialogData))]
    public class ActionDialogModel : ViewModel<ActionDialogData>
    {

        private EntityId _DecisionTableId;
        public EntityId DecisionTableId
        {
            get { return _DecisionTableId; }
            set
            {
                _DecisionTableId = value;
                NotifyPropertyChanged<EntityId>(() => DecisionTableId);
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

        public override void CopyFromViewData(ActionDialogData viewData)
        {
            DecisionTableId = viewData.DecisionTableId;
            Name = viewData.Name;
            Description = viewData.Description;
        }

        public override void CopyToViewData(ActionDialogData viewData)
        {
            if (DecisionTableId == null)
                throw new ArgumentNullException("DecisionTableId", "DecisionTableId is null.");

            viewData.DecisionTableId = DecisionTableId;
            viewData.Name = Name;
            viewData.Description = Description;
        }

        public override string Validate(string propertyName)
        {
            if (propertyName == "Name")
                return ValidateName();

            return string.Empty;
        }

        private string ValidateName()
        {
            if (string.IsNullOrEmpty(Name))
                return "The name must not be empty.";

            return string.Empty;
        }

    }
}
