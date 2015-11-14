using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(ProjectViewData))]
    public class ProjectViewModel : ViewModel<ProjectViewData>
    {

        public ApplicationViewModel ApplicationViewModel { get; set; }

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

        private string _Filename;
        public string Filename
        {
            get { return _Filename; }
            set
            {
                _Filename = value;
                NotifyPropertyChanged<string>(() => Filename);
            }
        }

        public override void CopyToViewData(ProjectViewData viewData)
        {
        }

        public override void CopyFromViewData(ProjectViewData viewData)
        {
            Name = viewData.Name;
            Filename = viewData.Filename;
        }
    }
}
