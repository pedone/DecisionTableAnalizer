using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DTCore;

namespace ViewModels
{

    public class StatusViewModel : ViewModel
    {

        private ObservableCollection<string> _Messages;
        public ObservableCollection<string> Messages
        {
            get { return _Messages; }
            private set
            {
                _Messages = value;
                NotifyPropertyChanged<ObservableCollection<string>>(() => Messages);
            }
        }

        public StatusViewModel()
        {
            Messages = new ObservableCollection<string>();
            StatusReporter.Instance.StatusMessage += Instance_StatusMessage;
        }

        private void Instance_StatusMessage(string message)
        {
            Messages.Add(message);
        }

    }
}
