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
    public interface IDockingContentElement : IDockingElement
    {
        bool ReplaceItem(DockingBase oldItem, DockingBase newItem);
        bool Remove(DockingBase item);
    }
}
