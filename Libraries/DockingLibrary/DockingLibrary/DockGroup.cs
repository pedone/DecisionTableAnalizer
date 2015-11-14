using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DockingLibrary
{

    public class DockGroup : DockingItemsControl<DockingBase>
    {

        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty;

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        #region Variables

        /// <summary>
        /// Returns the top item in a vertical DockGroup and the left item in a horizontal DockGroup.
        /// </summary>
        public DockingBase First
        {
            get { return Items.FirstOrDefault(); }
            set
            {
                if (Items.Count > 0)
                    Items.RemoveAt(0);

                Items.Insert(0, value);
            }
        }

        /// <summary>
        /// Returns the bottom item in a vertical DockGroup and the right item in a horizontal DockGroup.
        /// </summary>
        public DockingBase Last
        {
            get
            {
                if (Items.Count < 2)
                    return null;

                return Items.LastOrDefault();
            }
            set
            {
                if (Items.Count > 1)
                    Items.RemoveAt(1);
                else if (Items.Count == 0)
                    Items.Add(null);

                Items.Insert(1, value);
            }
        }

        #endregion

        static DockGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockGroup), new FrameworkPropertyMetadata(typeof(DockGroup)));

            //Register Dependency Properties
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DockGroup), new UIPropertyMetadata(Orientation.Horizontal));
        }

        protected override void OnItemsChanged(IEnumerable<DockingBase> oldItems, IEnumerable<DockingBase> newItems)
        {
            if (Items.Count > 2)
                throw new ArgumentException("Only two items allowed in a DockGroup.");

            NotifyPropertyChanged("First");
            NotifyPropertyChanged("Last");
        }

    }
}
