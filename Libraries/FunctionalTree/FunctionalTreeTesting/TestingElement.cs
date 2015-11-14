using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunctionalTreeLibrary;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FunctionalTreeTesting
{
    public class TestingElement : IFunctionalTreeElement
    {

        #region Variables

        public string Name { get; private set; }

        #endregion

        #region Events

        public static readonly FunctionalEvent BubblingTestEvent;
        public static readonly FunctionalEvent TunnelingTestEvent;
        public static readonly FunctionalEvent ParentTestEvent;
        public static readonly FunctionalEvent SiblingsTestEvent;
        public static readonly FunctionalEvent DescendentsTestEvent;
        public static readonly FunctionalEvent ChildrenTestEvent;
        public static readonly FunctionalEvent SpreadTestEvent;

        #endregion

        #region Functional Properties

        public static readonly FunctionalProperty InfoProperty;

        public string Info
        {
            get { return (string)this.GetFunctionalValue(InfoProperty); }
            set { this.SetFunctionalValue(InfoProperty, value); }
        }

        #endregion

        static TestingElement()
        {
            // Functional Properties
            InfoProperty = FunctionalProperty.Register("Info", typeof(string), typeof(TestingElement),
                new FunctionalPropertyMetadata("Empty", FunctionalPropertyMetadataOptions.Inherits, InfoChangedHandler, InfoCoercingHandler), InfoValidateHandler);
            
            // Functional Events
            BubblingTestEvent = FunctionalEventManager.RegisterEvent("BubblingTest", FunctionalStrategy.Bubble, typeof(FunctionalEventHandler), typeof(TestingElement));
            TunnelingTestEvent = FunctionalEventManager.RegisterEvent("TunnelingTest", FunctionalStrategy.Tunnel, typeof(FunctionalEventHandler), typeof(TestingElement));
            ParentTestEvent = FunctionalEventManager.RegisterEvent("ParentTest", FunctionalStrategy.Parent, typeof(FunctionalEventHandler), typeof(TestingElement));
            SiblingsTestEvent = FunctionalEventManager.RegisterEvent("SiblingsTest", FunctionalStrategy.Siblings, typeof(FunctionalEventHandler), typeof(TestingElement));
            DescendentsTestEvent = FunctionalEventManager.RegisterEvent("DescendentsTest", FunctionalStrategy.Descendents, typeof(FunctionalEventHandler), typeof(TestingElement));
            ChildrenTestEvent = FunctionalEventManager.RegisterEvent("ChildrenTest", FunctionalStrategy.Children, typeof(FunctionalEventHandler), typeof(TestingElement));
            SpreadTestEvent = FunctionalEventManager.RegisterEvent("SpreadTest", FunctionalStrategy.Spread, typeof(FunctionalEventHandler), typeof(TestingElement));
        }

        public TestingElement(string name)
        {
            Name = name;

            // Event Handler
            this.AddFunctionalHandler(BubblingTestEvent, new FunctionalEventHandler(EventTestHandler));
            this.AddFunctionalHandler(TunnelingTestEvent, new FunctionalEventHandler(EventTestHandler));
            this.AddFunctionalHandler(ParentTestEvent, new FunctionalEventHandler(EventTestHandler));
            this.AddFunctionalHandler(SiblingsTestEvent, new FunctionalEventHandler(EventTestHandler));
            this.AddFunctionalHandler(DescendentsTestEvent, new FunctionalEventHandler(EventTestHandler));
            this.AddFunctionalHandler(ChildrenTestEvent, new FunctionalEventHandler(EventTestHandler));
            this.AddFunctionalHandler(SpreadTestEvent, new FunctionalEventHandler(EventTestHandler));

            this.AddAttachedToFunctionalTreeHandler(new FunctionalTreeEventHandler(AttachedToTreeHandler));
            this.AddDetachedFromFunctionalTreeHandler(new FunctionalTreeEventHandler(DetachedFromTreeHandler));
        }

        public void Add(TestingElement item)
        {
            if (item != null)
                this.AddFunctionalChild(item);
        }

        public void Remove(TestingElement item)
        {
            this.RemoveFunctionalChild(item);
        }

        #region Property Handler

        private static bool InfoValidateHandler(object value)
        {
            MainWindow.Reporter.WriteLine(string.Format("[Property] [Validate] Info  Value: {0}", value));

            return true;
        }

        private static object InfoCoercingHandler(IFunctionalTreeElement element, object baseValue)
        {
            MainWindow.Reporter.WriteLine(string.Format("[Property] [Coerced] Info   Element: {0}  BaseValue: {1}", ((TestingElement)element).Name, baseValue));
            return baseValue;
        }

        private static void InfoChangedHandler(IFunctionalTreeElement element, FunctionalPropertyChangedEventArgs e)
        {
            MainWindow.Reporter.WriteLine(string.Format("[Property] [PropertyChanged] Info   Element: {0}  OldValue: {1}  NewValue: {2}", ((TestingElement)element).Name, e.OldValue, e.NewValue));
        }

        #endregion

        private void AttachedToTreeHandler(FunctionalTree functionalTree)
        {
            MainWindow.Reporter.WriteLine(string.Format("[Attached To Tree] Element: {0}", Name));
        }

        private void DetachedFromTreeHandler(FunctionalTree functionalTree)
        {
            MainWindow.Reporter.WriteLine(string.Format("[Detached From Tree] Element: {0}", Name));
        }

        private void EventTestHandler(IFunctionalTreeElement sender, FunctionalEventArgs e)
        {
            MainWindow.Reporter.WriteLine(string.Format("[Event] Event: {0}  Sender: {1}  Source: {2}", e.FunctionalEvent.Name, ((TestingElement)sender).Name, ((TestingElement)e.Source).Name));
        }

        public void RaiseEvent(FunctionalStrategy eventStrategy)
        {
            if (eventStrategy == FunctionalStrategy.Bubble)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(BubblingTestEvent));
            else if (eventStrategy == FunctionalStrategy.Tunnel)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(TunnelingTestEvent));
            else if (eventStrategy == FunctionalStrategy.Parent)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(ParentTestEvent));
            else if (eventStrategy == FunctionalStrategy.Children)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(ChildrenTestEvent));
            else if (eventStrategy == FunctionalStrategy.Siblings)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(SiblingsTestEvent));
            else if (eventStrategy == FunctionalStrategy.Descendents)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(DescendentsTestEvent));
            else if (eventStrategy == FunctionalStrategy.Spread)
                this.RaiseFunctionalEvent(new FunctionalEventArgs(SpreadTestEvent));
        }

        public void SetProperty(string property, string value)
        {
            if (property == "Info")
                Info = value;
        }

        public void ClearProperty(string property)
        {
            if (property == "Info")
                this.ClearFunctionalValue(InfoProperty);
        }

        public void WriteProperty(string property)
        {
            if (property == "Info")
                MainWindow.Reporter.WriteLine(string.Format("[Property Value] Property: {0}  Element: {1}  Value: {2}", property, Name, Info));
        }

    }
}
