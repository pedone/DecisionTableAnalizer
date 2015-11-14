using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCore;
using ViewDatas;

namespace ViewModels
{
    [ViewDataType(typeof(RecentProjectViewData))]
    public class RecentProjectViewModel : ViewModel<RecentProjectViewData>
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

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                NotifyPropertyChanged<string>(() => Id);
            }
        }

        public override void CopyToViewData(RecentProjectViewData viewData)
        {
        }

        public override void CopyFromViewData(RecentProjectViewData viewData)
        {
            Name = viewData.Name;
            Filename = viewData.Filename;
            Id = viewData.EntityId.Id;
        }
    }
}
