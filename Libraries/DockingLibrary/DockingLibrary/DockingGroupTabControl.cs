using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DockingLibrary
{
    public class DockingGroupTabControl : TabControl
    {

        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new DockingGroupTabItem();
        }

    }
}
