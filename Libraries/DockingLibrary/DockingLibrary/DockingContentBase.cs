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
    public abstract class DockingContentBase : DockingBase, IDockingContentElement
    {

        #region Events

        private static readonly FunctionalEvent ItemPromotedEvent;

        #endregion

        #region Variables

        /// <summary>
        /// True, if the dockingContent should automatically close when there are no children left
        /// </summary>
        public bool AutoClose { get; protected set; }

        #endregion

        static DockingContentBase()
        {
            //Register Events
            ItemPromotedEvent = FunctionalEventManager.RegisterEvent("ItemPromoted", FunctionalStrategy.Bubble, typeof(FunctionalDockingBaseEventHandler), typeof(DockingContentBase));
        }

        public DockingContentBase()
        {
            this.AddFunctionalHandler(ItemPromotedEvent, new FunctionalDockingBaseEventHandler(OnDockingContentPromotedInternal));
        }

        private void OnDockingContentPromotedInternal(IFunctionalTreeElement sender, FunctionalDockingBaseEventArgs e)
        {
            if (IsChild(e.Source as DockingBase))
            {
                ReplaceItemInternal(e.Source as DockingBase, e.Target as DockingBase);

                //If the parent of the itemParent was found, don't let it bubble any higher
                e.Handled = true;
            }
        }

        public override void OnDockingDescendentClosed(DockingBase item)
        {
            if (IsChild(item))
            {
                //We have to remove the item ourselves, the item can't do it
                RemoveInternal(item);
                OnDockingChildClosed(item);
            }
        }

        public bool ReplaceItem(DockingBase oldItem, DockingBase newItem)
        {
            return ReplaceItemInternal(oldItem, newItem);
        }

        public bool Remove(DockingBase item)
        {
            return RemoveInternal(item);
        }

        protected void PromoteItem(DockingBase child)
        {
            //Disconnect the child
            RemoveInternal(child);

            this.RaiseFunctionalEvent(new FunctionalDockingBaseEventArgs(ItemPromotedEvent, child));
        }

        #region Virtuals/Abstracts

        protected abstract bool IsChild(DockingBase item);
        protected abstract bool ReplaceItemInternal(DockingBase oldItem, DockingBase newItem);
        protected abstract bool RemoveInternal(DockingBase item);

        public virtual void OnDockingChildClosed(DockingBase child)
        { }

        #endregion

    }
}