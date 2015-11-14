using System.Windows;
using System;
using System.Windows.Markup;
using FunctionalTreeLibrary;

namespace DockingLibrary
{
    public class DockingContentControl : DockingContentControl<object>
    { }

    [ContentProperty("Content")]
    public class DockingContentControl<T> : DockingContentBase
        where T : class
    {

        #region Dependency Properties

        public static readonly DependencyProperty ContentProperty;

        public T Content
        {
            get { return (T)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        #endregion

        #region Property Handler

        private static void ContentChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
                ((DockingContentControl<T>)d).OnContentChangedInternal(e.OldValue as T, e.NewValue as T);
        }

        #endregion

        static DockingContentControl()
        {
            //Register Dependency Properties
            ContentProperty = DependencyProperty.Register("Content", typeof(T), typeof(DockingContentControl<T>), new UIPropertyMetadata(null, ContentChangedHandler));
        }
        
        public DockingContentControl()
        {
            AutoClose = false;
        }

        private void OnContentChangedInternal(T oldContent, T newContent)
        {
            DockingBase oldDockingContent = oldContent as DockingBase;
            DockingBase newDockingContent = newContent as DockingBase;

            if (oldDockingContent != null)
                this.RemoveFunctionalChild(oldDockingContent);
            if (newDockingContent != null)
                this.AddFunctionalChild(newDockingContent);

            OnContentChanged(oldContent, newContent);

            if (Content == null && AutoClose)
                Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            DockingBase dockingContent = Content as DockingBase;
            if (dockingContent != null)
                this.RemoveFunctionalChild(dockingContent);
        }

        protected override bool IsChild(DockingBase item)
        {
            return Content == item;
        }

        protected override bool ReplaceItemInternal(DockingBase oldItem, DockingBase newItem)
        {
            if (!IsChild(oldItem) || !(newItem is T))
                return false;

            Content = newItem as T;
            return true;
        }

        protected override bool RemoveInternal(DockingBase item)
        {
            if (Content != item)
                return false;

            Content = null;
            return true;
        }

        #region Virtuals

        public virtual void OnContentChanged(T oldContent, T newContent)
        { }

        #endregion

    }
}
