using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using FunctionalTreeLibrary;

namespace DockingLibrary
{

    [ContentProperty("Items")]
    public abstract class DockingItemsControl<T> : DockingContentBase
        where T : DockingBase
    {

        #region Dependency Properties

        public static readonly DependencyPropertyKey ItemsProperty;

        public ObservableCollection<T> Items
        {
            get { return (ObservableCollection<T>)GetValue(ItemsProperty.DependencyProperty); }
            private set { SetValue(ItemsProperty, value); }
        }
        
        #endregion

        #region Property Handlers

        private static void ItemsChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            DockingItemsControl<T> control = (DockingItemsControl<T>)d;

            ObservableCollection<T> oldCollection = e.OldValue as ObservableCollection<T>;
            if (oldCollection != null)
                oldCollection.CollectionChanged -= control.OnItemsChangedInternal;

            ObservableCollection<T> newCollection = (e.NewValue as ObservableCollection<T>) ?? new ObservableCollection<T>();
            if (newCollection != null)
                newCollection.CollectionChanged += control.OnItemsChangedInternal;
        }

        #endregion

        #region Variables

        /// <summary>
        /// True, if the itemsControl should replace itself when there's only one child left
        /// </summary>
        public bool PromoteLastItem { get; private set; }

        #endregion

        static DockingItemsControl()
        {
            //Register Dependency Properties
            ItemsProperty = DependencyProperty.RegisterReadOnly("Items", typeof(ObservableCollection<T>), typeof(DockingItemsControl<T>), new UIPropertyMetadata(null, ItemsChangedHandler));
        }

        public DockingItemsControl()
            : this(promoteLastItem: true)
        { }

        public DockingItemsControl(bool promoteLastItem)
        {
            AutoClose = true;

            Items = new ObservableCollection<T>();
            PromoteLastItem = promoteLastItem;
        }

        public bool ReplaceItem(T oldItem, T newItem)
        {
            if (!IsChild(oldItem) || IsChild(newItem) || newItem == null)
                return false;

            int oldContentIndex = Items.IndexOf(oldItem);
            Items.Remove(oldItem);
            Items.Insert(oldContentIndex, newItem);

            return true;
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        public void Add(T item)
        {
            if (!Items.Contains(item))
                Items.Add(item);
        }

        public void Move(T content, int targetIndex)
        {
            if (content == null || targetIndex < 0 || !Items.Contains(content))
                return;

            Items.Move(Items.IndexOf(content), targetIndex);
        }

        private void OnItemsChangedInternal(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<T> oldItems = e.OldItems != null ? e.OldItems.OfType<T>() : null;
            IEnumerable<T> newItems = e.NewItems != null ? e.NewItems.OfType<T>() : null;

            if (oldItems != null)
            {
                foreach (var item in oldItems)
                    this.RemoveFunctionalChild(item);
            }

            if (newItems != null)
            {
                foreach (var item in newItems)
                    this.AddFunctionalChild(item);
            }

            OnItemsChanged(oldItems, newItems);
        }

        public override void OnDockingChildClosed(DockingBase child)
        {
            //If there's only one item left, promote it
            if (Items.Count == 1 && PromoteLastItem)
                PromoteItem(Items.First());

            if (Items.Count == 0 && AutoClose)
                Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            foreach (T item in Items.ToList())
                Remove(item);
        }

        protected override bool IsChild(DockingBase item)
        {
            return Items.Contains(item);
        }

        protected override bool ReplaceItemInternal(DockingBase oldItem, DockingBase newItem)
        {
            return ReplaceItem(oldItem as T, newItem as T);
        }

        protected override bool RemoveInternal(DockingBase item)
        {
            return Remove(item as T);
        }

        #region Virtuals

        protected virtual void OnItemsChanged(IEnumerable<T> oldItems, IEnumerable<T> newItems)
        { }

        #endregion

    }

}
