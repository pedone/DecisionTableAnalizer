using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(SystemRequirementViewData))]
    public class SystemRequirementViewModel : ViewModel<SystemRequirementViewData>
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

        public override void CopyFromViewData(SystemRequirementViewData viewData)
        {
            Name = viewData.Name;
            Kind = viewData.Kind;
        }

        public override void CopyToViewData(SystemRequirementViewData viewData)
        {
        }

    }
}
