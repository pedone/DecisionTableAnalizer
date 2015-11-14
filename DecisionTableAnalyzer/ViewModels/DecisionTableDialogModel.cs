using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(DecisionTableDialogData))]
    public class DecisionTableDialogModel : ViewModel<DecisionTableDialogData>
    {

        private EntityId _DecisionTableManagerId;
        public EntityId DecisionTableManagerId
        {
            get { return _DecisionTableManagerId; }
            set
            {
                _DecisionTableManagerId = value;
                NotifyPropertyChanged<EntityId>(() => DecisionTableManagerId);
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

        public override void CopyFromViewData(DecisionTableDialogData viewData)
        {
            DecisionTableManagerId = viewData.DecisionTableManagerId;
            Name = viewData.Name;
            Description = viewData.Description;
        }

        public override void CopyToViewData(DecisionTableDialogData viewData)
        {
            if (DecisionTableManagerId == null)
                throw new ArgumentNullException("DecisionTableManagerId", "DecisionTableManagerId is null.");

            viewData.DecisionTableManagerId = DecisionTableManagerId;
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
