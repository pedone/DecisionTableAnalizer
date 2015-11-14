using System;
using System.Windows.Controls;

namespace DockingLibrary
{
    public class AutoHideChannelItem : ListBoxItem
    {

        #region Events

        public event ValueChangedEventHandler<AutoHideChannelItem, bool> IsMouseOverChanged;

        #endregion

        #region Variables

        public View View { get; private set; }

        #endregion

        public AutoHideChannelItem(View view)
        {
            View = view;
            Content = view.Header;

            MouseEnter += AutoHideChannelItem_MouseEnter;
            MouseLeave += AutoHideChannelItem_MouseLeave;
        }

        private void AutoHideChannelItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsMouseOverChanged != null)
                IsMouseOverChanged(this, new ValueChangedEventArgs<AutoHideChannelItem, bool>(this, true, false));
        }

        private void AutoHideChannelItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsMouseOverChanged != null)
                IsMouseOverChanged(this, new ValueChangedEventArgs<AutoHideChannelItem, bool>(this, false, true));
        }

    }
}
