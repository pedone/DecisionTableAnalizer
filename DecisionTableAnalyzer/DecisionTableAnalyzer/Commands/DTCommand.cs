using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionTableAnalyzer.ViewModels;

namespace DecisionTableAnalyzer.Commands
{

    public abstract class DTCommand<T>
    {

        public abstract bool CanExecute(T contextViewModel);
        public abstract void Execute(T contextViewModel);

    }
}
