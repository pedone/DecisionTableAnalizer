using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(RequirementViewData))]
    public class RequirementViewModel : ViewModel<RequirementViewData>
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

        private Priority _Priority;
        public Priority Priority
        {
            get { return _Priority; }
            set
            {
                _Priority = value;
                NotifyPropertyChanged<Priority>(() => Priority);
            }
        }

        private RequirementKind _Kind;
        public RequirementKind Kind
        {
            get { return _Kind; }
            set
            {
                _Kind = value;
                NotifyPropertyChanged<RequirementKind>(() => Kind);
            }
        }

        public override void CopyFromViewData(RequirementViewData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
            Priority = viewData.Priority;
            Kind = viewData.Kind;
        }

        public override void CopyToViewData(RequirementViewData viewData)
        {
        }

    }
}
