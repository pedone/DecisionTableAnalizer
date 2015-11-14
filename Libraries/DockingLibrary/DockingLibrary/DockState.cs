using System;
namespace DockingLibrary
{

    [Flags]
    public enum DockState
    {
        Dock = 1,
        TabbedDocument = 2,
        AutoHide = 4,
        Float = 8,
        Hide = 16
    }
}
