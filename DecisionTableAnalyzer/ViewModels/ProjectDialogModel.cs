using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using System.ComponentModel;

namespace ViewModels
{
    [ViewDataType(typeof(ProjectDialogData))]
    public class ProjectDialogModel : ViewModel<ProjectDialogData>
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

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                NotifyPropertyChanged<string>(() => Description);
            }
        }

        public override void CopyToViewData(ProjectDialogData viewData)
        {
            viewData.Name = Name;
            viewData.Description = Description;
        }

        public override void CopyFromViewData(ProjectDialogData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
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
