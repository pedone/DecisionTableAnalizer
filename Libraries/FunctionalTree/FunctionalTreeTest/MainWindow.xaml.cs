using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FunctionalTreeLibrary;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace FunctionalTreeTest
{

    public class FooTesting : Window
    {


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(FooTesting), new UIPropertyMetadata("DefaultTitle"));


    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IFunctionalTreeElement
    {

        List<Foo> Elements = new List<Foo> { new Foo("1"), new Foo("2"), new Foo("3"), new Foo("4"), new Foo("5"), new Foo("6"), new Foo("7"), new Foo("8"), new Foo("9") };
        List<NoFoo> OtherElements = new List<NoFoo> { new NoFoo("1"), new NoFoo("2"), new NoFoo("3"), new NoFoo("4"), new NoFoo("5"), new NoFoo("6"), new NoFoo("7"), new NoFoo("8"), new NoFoo("9") };


        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(MainWindow));
            //new UIPropertyMetadata(0, MyPropertyChanged, MyPropertyCoerce),
            //MyPropertyValidate);


        private static bool MyPropertyValidate(object value)
        {
            if (value.ToString() == "5")
                return false;

            return true;
        }
        private static object MyPropertyCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }
        private static void MyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public string Header
        {
            get { return (string)this.GetFunctionalValue(HeaderProperty); }
            set { this.SetFunctionalValue(HeaderProperty, value); }
        }
        public static FunctionalProperty HeaderProperty = FunctionalProperty.Register("Header", typeof(string), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();

            SetValue(FooTesting.TitleProperty, "TestTitle");

            //SetValue(MyPropertyProperty, "test");
            this.AddFunctionalHandler(Foo.TestSpreadEvent, new FunctionalEventHandler(TestEventHandler), true);
            this.AddFunctionalHandler(Foo.TestBubbleEvent, new FunctionalEventHandler(TestEventHandler), true);
            this.AddFunctionalHandler(Foo.TestTunnelEvent, new FunctionalEventHandler(TestEventHandler), true);
            
            //this.RemoveFunctionalHandler(Foo.TestTunnelEvent, new FunctionalEventHandler(TestEventHandler));
        }

        private void TestEventHandler(IFunctionalTreeElement sender, FunctionalEventArgs e)
        {
            string senderString = (sender is Foo ? ((Foo)sender).Header : "Root");
            string sourceString = (e.Source is Foo ? ((Foo)e.Source).Header : "Root");
            Debug.WriteLine(String.Format("Root | Id: Root | sender: {0} | e.Source: {1}", senderString, sourceString));
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void EmptyList_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in this.GetFunctionalChildren().ToList())
                this.RemoveFunctionalChild(child);
        }

        private void FillList_Click(object sender, RoutedEventArgs e)
        {
            //Tree #1
            //this.AddFunctionalChild(Elements[0]);
            //Elements[0].AddFunctionalChild(Elements[1]);
            //Elements[0].AddFunctionalChild(Elements[2]);
            //Elements[1].AddFunctionalChild(Elements[6]);
            //Elements[1].AddFunctionalChild(Elements[7]);
            //Elements[2].AddFunctionalChild(Elements[8]);

            //this.AddFunctionalChild(Elements[3]);
            //Elements[3].AddFunctionalChild(Elements[4]);
            //Elements[3].AddFunctionalChild(Elements[5]);

            //Tree #2
            this.AddFunctionalChild(Elements[0]);
            Elements[0].AddFunctionalChild(Elements[1]);
            Elements[0].AddFunctionalChild(Elements[2]);
            Elements[1].AddFunctionalChild(OtherElements[1]);
            Elements[1].AddFunctionalChild(OtherElements[0]);

            this.AddFunctionalChild(Elements[3]);
            Elements[3].AddFunctionalChild(OtherElements[3]);
            Elements[3].AddFunctionalChild(OtherElements[4]);

            OtherElements[4].AddFunctionalChild(Elements[4]);
            Elements[4].AddFunctionalChild(Elements[5]);
            Elements[5].AddFunctionalChild(OtherElements[5]);
            OtherElements[5].AddFunctionalChild(Elements[6]);

            
        }

        private void RaiseSpreadEvent_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("----------- Spread Start ----------------");

            string selectedContent = (ElementSelectionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedContent == "Root")
                this.RaiseFunctionalEvent(new FunctionalEventArgs(Foo.TestSpreadEvent));
            else
            {
                int elementIndex = Convert.ToInt32(selectedContent)-1;
                Elements[elementIndex].RaiseSpreadEvent();
            }
        }

        private void RaiseBubbleEvent_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("----------- Bubble Start ----------------");

            string selectedContent = (ElementSelectionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedContent == "Root")
                this.RaiseFunctionalEvent(new FunctionalEventArgs(Foo.TestBubbleEvent));
            else
            {
                int elementIndex = Convert.ToInt32(selectedContent)-1;
                Elements[elementIndex].RaiseBubbleEvent();
            }
        }

        private void RaiseTunnelEvent_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("----------- Tunnel Start ----------------");

            string selectedContent = (ElementSelectionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedContent == "Root")
                this.RaiseFunctionalEvent(new FunctionalEventArgs(Foo.TestTunnelEvent));
            else
            {
                int elementIndex = Convert.ToInt32(selectedContent)-1;
                Elements[elementIndex].RaiseTunnelEvent();
            }
        }

        private void SetHeader_Click(object sender, RoutedEventArgs e)
        {
            string selectedContent = (ElementSelectionComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            if (selectedContent != "Root")
            {
                int elementIndex = Convert.ToInt32(selectedContent) - 1;
                Elements[elementIndex].Header = NewHeader.Text;
            }
        }

    }

    public class NoFoo : IFunctionalTreeElement
    {

        public string Header
        {
            get { return (string)this.GetFunctionalValue(Foo.HeaderProperty); }
            set { this.SetFunctionalValue(Foo.HeaderProperty, value); }
        }
        public string Id { get; set; }

        public NoFoo(string header)
        {
            //Header = header;
            Id = header;

            this.AddFunctionalHandler(Foo.TestSpreadEvent, new FunctionalEventHandler(TestHandler));
            this.AddFunctionalHandler(Foo.TestBubbleEvent, new FunctionalEventHandler(TestHandler));
            this.AddFunctionalHandler(Foo.TestTunnelEvent, new FunctionalEventHandler(TestHandler));
        }

        private void TestHandler(IFunctionalTreeElement sender, FunctionalEventArgs e)
        {
            Debug.WriteLine(String.Format("{0} | Id: {1} | NoFoo", Header, Id));
        }

    }

    public class Foo : UIElement, IFunctionalTreeElement
    {

        public static FunctionalEvent TestSpreadEvent = FunctionalEventManager.RegisterEvent("TestSpread", FunctionalStrategy.Spread, typeof(FunctionalEventHandler), typeof(Foo));
        public static FunctionalEvent TestBubbleEvent = FunctionalEventManager.RegisterEvent("TestBubble", FunctionalStrategy.Bubble, typeof(FunctionalEventHandler), typeof(Foo));
        public static FunctionalEvent TestTunnelEvent = FunctionalEventManager.RegisterEvent("TestTunnel", FunctionalStrategy.Tunnel, typeof(FunctionalEventHandler), typeof(Foo));


        public string Header
        {
            get { return (string)this.GetFunctionalValue(HeaderProperty); }
            set { this.SetFunctionalValue(HeaderProperty, value); }
        }
        public static FunctionalProperty HeaderProperty = FunctionalProperty.Register("Header", typeof(string), typeof(Foo),
            new FunctionalPropertyMetadata("DefaultHeader", FunctionalPropertyMetadataOptions.Inherits, HeaderChanged, HeaderCoerce), HeaderValidate);

        public string Id { get; set; }

        private static void HeaderChanged(IFunctionalTreeElement element, FunctionalPropertyChangedEventArgs e)
        {
            
        }
        private static object HeaderCoerce(IFunctionalTreeElement element, object baseValue)
        {
            return baseValue;
        }
        private static bool HeaderValidate(object value)
        {
            return true;
        }

        public Foo(string header)
        {
            Debug.WriteLine(String.Format("Default Header: {0}", Header));

            //Header = header;
            Id = header;

            this.AddFunctionalHandler(TestSpreadEvent, new FunctionalEventHandler(TestHandler));
            this.AddFunctionalHandler(TestBubbleEvent, new FunctionalEventHandler(TestHandler));
            this.AddFunctionalHandler(TestTunnelEvent, new FunctionalEventHandler(TestHandler));
        }

        private void TestHandler(IFunctionalTreeElement sender, FunctionalEventArgs e)
        {
            string senderString = (sender is Foo ? ((Foo)sender).Header : "Root");
            string sourceString = (e.Source is Foo ? ((Foo)e.Source).Header : "Root");
            Debug.WriteLine(String.Format("{0} | Id: {1} | sender: {2} | e.Source: {3}", Header, Id, senderString, sourceString));
        }

        public void RaiseSpreadEvent()
        {
            this.RaiseFunctionalEvent(new FunctionalEventArgs(TestSpreadEvent));
        }
        public void RaiseBubbleEvent()
        {
            this.RaiseFunctionalEvent(new FunctionalEventArgs(TestBubbleEvent));
        }
        public void RaiseTunnelEvent()
        {
            this.RaiseFunctionalEvent(new FunctionalEventArgs(TestTunnelEvent));
        }

    }

}
