using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;

namespace ViewModels
{

    [ViewDataType(typeof(SystemProjectViewData))]
    public class SystemProjectViewModel : ViewModel<SystemProjectViewData>
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

        public override void CopyToViewData(SystemProjectViewData viewData)
        {
            viewData.Name = Name;
        }

        public override void CopyFromViewData(SystemProjectViewData viewData)
        {
            Name = viewData.Name;
        }
    }
}
