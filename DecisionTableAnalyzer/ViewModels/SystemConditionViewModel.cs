using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;
using DTEnums;

namespace ViewModels
{
    [ViewDataType(typeof(SystemConditionViewData))]
    public class SystemConditionViewModel : ViewModel<SystemConditionViewData>
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

        private string _Kind;
        public string Kind
        {
            get { return _Kind; }
            set
            {
                _Kind = value;
                NotifyPropertyChanged<string>(() => Kind);
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

        public override void CopyFromViewData(SystemConditionViewData viewData)
        {
            Name = viewData.Name;
            Description = viewData.Description;
            Kind = "Condition";
        }

        public override void CopyToViewData(SystemConditionViewData viewData)
        {
        }

    }
}
