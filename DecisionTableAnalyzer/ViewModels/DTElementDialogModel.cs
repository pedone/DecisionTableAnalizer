using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;

namespace ViewModels
{
    public class DTElementDialogModel : ViewModel
    {

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private DTElementKind _Kind;
        public DTElementKind Kind
        {
            get { return _Kind; }
            set
            {
                _Kind = value;
                NotifyPropertyChanged("Kind");
            }
        }


    }
}
