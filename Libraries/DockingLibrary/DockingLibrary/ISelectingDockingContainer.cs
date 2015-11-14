using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingLibrary
{
    public interface ISelectingViewContainer
    {

        ViewGroup ViewGroup { get; }
        View SelectedView { get; }
        bool Show(View view);

    }
}
