using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingLibrary
{

    public delegate void ValueChangedEventHandler<SourceType, ValueType>(object sender, ValueChangedEventArgs<SourceType, ValueType> e);

    public class ValueChangedEventArgs<SourceType, ValueType> : EventArgs
    {
        public SourceType Source { get; private set; }
        public ValueType NewValue { get; private set; }
        public ValueType OldValue { get; private set; }

        public ValueChangedEventArgs(SourceType source, ValueType oldValue, ValueType newValue)
        {
            Source = source;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

}
