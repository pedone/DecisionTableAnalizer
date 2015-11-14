using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace DockingLibrary
{
    internal class DockingRoute
    {

        #region Variables

        private List<DockDirection> _route;

        #endregion

        #region Properties

        public int RouteLength
        {
            get { return _route.Count; }
        }

        #endregion

        public DockingRoute()
        {
            _route = new List<DockDirection>();
        }

        public void Add(DockDirection direction)
        {
            _route.Insert(0, direction);
        }

        public DockDirection GetDirection(int index)
        {
            if (index < _route.Count)
                return _route[index];

            return DockDirection.None;
        }

    }
}
