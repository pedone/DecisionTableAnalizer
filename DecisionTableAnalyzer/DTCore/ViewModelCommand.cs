using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCore
{

    public abstract class ViewModelCommand
    {

        internal abstract bool CanExecute(ViewModel contextViewModel);
        internal abstract void Execute(ViewModel contextViewModel);

    }

    public abstract class ViewModelCommand<T> : ViewModelCommand
        where T : ViewModel
    {

        public abstract bool CanExecute(T contextViewModel);
        public abstract void Execute(T contextViewModel);

        internal override bool CanExecute(ViewModel contextViewModel)
        {
            if (contextViewModel != null && !(contextViewModel is T))
                throw new ArgumentException("Invalid context view model.");

            return CanExecute((T)contextViewModel);
        }

        internal override void Execute(ViewModel contextViewModel)
        {
            if (!(contextViewModel is T))
                throw new ArgumentException("Invalid context view model.");

            Execute((T)contextViewModel);
        }

    }
}
