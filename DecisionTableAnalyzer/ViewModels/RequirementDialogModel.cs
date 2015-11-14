using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using DTEnums;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(RequirementDialogData))]
    public class RequirementDialogModel : ViewModel<RequirementDialogData>
    {

        private EntityId _RequirementManagerId;
        public EntityId RequirementManagerId
        {
            get { return _RequirementManagerId; }
            set
            {
                _RequirementManagerId = value;
                NotifyPropertyChanged<EntityId>(() => RequirementManagerId);
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

        private RequirementKind _Kind;
        public RequirementKind Kind
        {
            get { return _Kind; }
            set
            {
                _Kind = value;
                NotifyPropertyChanged<string>(() => Name);
            }
        }

        private Priority _Priority;
        public Priority Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
                NotifyPropertyChanged<string>(() => Name);
            }
        }

        public override void CopyFromViewData(RequirementDialogData viewData)
        {
            RequirementManagerId = viewData.RequirementManagerId;
            Name = viewData.Name;
            Description = viewData.Description;
            Priority = viewData.Priority;
            Kind = viewData.Kind;
        }

        public override void CopyToViewData(RequirementDialogData viewData)
        {
            if (RequirementManagerId == null)
                throw new ArgumentNullException("RequirementManagerId", "RequirementManagerId is null.");

            viewData.RequirementManagerId = RequirementManagerId;
            viewData.Name = Name;
            viewData.Description = Description;
            viewData.Priority = Priority;
            viewData.Kind = Kind;
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
