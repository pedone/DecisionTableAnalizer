using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FunctionalTreeLibrary;

namespace DockingLibrary
{

    public delegate void FunctionalDockingBaseEventHandler(IFunctionalTreeElement sender, FunctionalDockingBaseEventArgs e);

    public class FunctionalDockingBaseEventArgs : FunctionalEventArgs
    {

        public DockingBase Target { get; private set; }

        public FunctionalDockingBaseEventArgs(FunctionalEvent functionalEvent, DockingBase target)
            : base(functionalEvent)
        {
            Target = target;
        }

    }


}
