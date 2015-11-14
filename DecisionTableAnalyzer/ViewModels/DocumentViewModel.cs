using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using DTCore;

namespace ViewModels
{
    public class DocumentViewModel : ViewModel
    {

        private string _Header;
        public string Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
                NotifyPropertyChanged<string>(() => Header);
            }
        }

        private FlowDocument _Document;
        public FlowDocument Document
        {
            get { return _Document; }
            set
            {
                if (_Document != value)
                {
                    _Document = value;
                    NotifyPropertyChanged<FlowDocument>(() => Document);
                }
            }
        }

    }
}
