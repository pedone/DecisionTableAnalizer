using System.Windows.Controls;
using System.Windows;
using System;
using System.ComponentModel;
using HelperLibrary;
using System.Windows.Markup;
using System.Diagnostics;
using FunctionalTreeLibrary;

namespace DockingLibrary
{
    public interface IDockingElement : IFunctionalTreeElement
    {
        DockManager DockManager { get; set; }
        IDockingContentElement ParentContent { get; }
        string Id { get; }
    }
}
