using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingLibrary
{

    public enum ViewUsage
    {
        Single,
        Multiple
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewUsageAttribute : Attribute
    {

        #region Variables

        public ViewUsage ViewUsage { get; private set; }
        public int MaximumInstanceCount { get; set; }
        public ExtendedDockDirection DefaultDockDirection { get; set; }

        #endregion

        public ViewUsageAttribute(ViewUsage viewUsage)
        {
            ViewUsage = viewUsage;
            DefaultDockDirection = ExtendedDockDirection.Right;
        }

    }

}
