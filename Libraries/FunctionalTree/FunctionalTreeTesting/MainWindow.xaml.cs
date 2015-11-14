using System;
using System.Collections.Generic;
using System.Windows;
using FunctionalTreeLibrary;
using System.Windows.Controls;

namespace FunctionalTreeTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IReporter
    {

        public static IReporter Reporter { get; private set; }

        public TestingElement Root { get; private set; }
        private List<TestingElement> _elements;

        private Dictionary<int, int> _elementOwnerTable;

        public string Output
        {
            get { return (string)GetValue(OutputProperty); }
            set { SetValue(OutputProperty, value); }
        }
        public static readonly DependencyProperty OutputProperty =
            DependencyProperty.Register("Output", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty));

        public string EventTracingOutput
        {
            get { return (string)GetValue(EventTracingOutputProperty); }
            set { SetValue(EventTracingOutputProperty, value); }
        }
        public static readonly DependencyProperty EventTracingOutputProperty =
            DependencyProperty.Register("EventTracingOutput", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty));


        public MainWindow()
        {
            InitializeComponent();

            Reporter = this;
            
            TestingElement.BubblingTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.BubblingTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;
            TestingElement.TunnelingTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.TunnelingTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;
            TestingElement.ChildrenTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.ChildrenTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;
            TestingElement.DescendentsTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.DescendentsTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;
            TestingElement.ParentTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.ParentTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;
            TestingElement.SiblingsTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.SiblingsTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;
            TestingElement.SpreadTestEvent.GetEventTracer().EventRaising += EventTracer_EventRaising;
            TestingElement.SpreadTestEvent.GetEventTracer().EventRaised += EventTracer_EventRaised;

            InitElementOwnerTable();
            InitElements();

            //Location and Size
            Left = Properties.Settings.Default.X;
            Top = Properties.Settings.Default.Y;

            LocationChanged += new EventHandler(MainWindow_LocationChanged);
        }

        private void EventTracer_EventRaising(FunctionalEvent functionalEvent, FunctionalEventTracingArgs e)
        {
            TestingElement previous = e.PreviousElement as TestingElement;
            TestingElement next = e.NextElement as TestingElement;
            string previousName = previous != null ? previous.Name : "";
            string nextName = next != null ? next.Name : "";
            string currentName = (e.CurrentElement as TestingElement).Name;

            WriteEventTracingLine(string.Format("[EventRaising] Event: {0}   Previous: {1}  Current: {2}  Next: {3}", functionalEvent.Name, previousName, currentName, nextName));
        }

        private void EventTracer_EventRaised(FunctionalEvent functionalEvent, FunctionalEventTracingArgs e)
        {
            TestingElement previous = e.PreviousElement as TestingElement;
            TestingElement next = e.NextElement as TestingElement;
            string previousName = previous != null ? previous.Name : "";
            string nextName = next != null ? next.Name : "";
            string currentName = (e.CurrentElement as TestingElement).Name;

            WriteEventTracingLine(string.Format("[EventRaised] Event: {0}   Previous: {1}  Current: {2}  Next: {3}", functionalEvent.Name, previousName, currentName, nextName));
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.X = Left;
            Properties.Settings.Default.Y = Top;
            Properties.Settings.Default.Save();
        }

        private void InitElementOwnerTable()
        {
            _elementOwnerTable = new Dictionary<int, int>
            {
                {1, 0},
                {2, 0},
                {3, 1},
                {4, 1},
                {5, 2},
                {6, 2},
                {7, 2},
                {8, 2},
                {9, 3},
                {10, 3},
                {11, 3},
                {12, 4},
                {13, 5},
                {14, 6},
                {15, 6},
                {16, 8},
                {17, 8},
                {18, 8},
                {19, 9},
                {20, 9},
                {21, 11},
                {22, 11},
                {23, 12},
                {24, 12},
                {25, 12},
                {26, 21},
                {27, 25},
                {28, 25},
                {29, 26},
                {30, 26},
                {31, 27},
                {32, 27},
                {33, 27},
            };
        }

        private void InitElements()
        {
            Root = new TestingElement("Root");
            _elements = new List<TestingElement>
            {
            	Root,
            	new TestingElement("1"),
            	new TestingElement("2"),
            	new TestingElement("3"),
            	new TestingElement("4"),
            	new TestingElement("5"),
            	new TestingElement("6"),
            	new TestingElement("7"),
            	new TestingElement("8"),
            	new TestingElement("9"),
            	new TestingElement("10"),
            	new TestingElement("11"),
            	new TestingElement("12"),
            	new TestingElement("13"),
            	new TestingElement("14"),
            	new TestingElement("15"),
            	new TestingElement("16"),
            	new TestingElement("17"),
            	new TestingElement("18"),
            	new TestingElement("19"),
            	new TestingElement("20"),
            	new TestingElement("21"),
            	new TestingElement("22"),
            	new TestingElement("23"),
            	new TestingElement("24"),
            	new TestingElement("25"),
            	new TestingElement("26"),
            	new TestingElement("27"),
            	new TestingElement("28"),
            	new TestingElement("29"),
            	new TestingElement("30"),
            	new TestingElement("31"),
            	new TestingElement("32"),
            	new TestingElement("33")
            };
        }

        private void BuildTestingTree()
        {
            InitElements();

            //Build tree
            for (int i = 1; i < _elements.Count; i++)
                _elements[_elementOwnerTable[i]].Add(_elements[i]);
        }

        private void Raise_Click(object sender, RoutedEventArgs e)
        {
            TestingElement selectedElement = GetSelectedElement();
            string eventToRaise = ((ComboBoxItem)cbEvent.SelectedItem).Content.ToString();
            FunctionalStrategy eventStrategy = (FunctionalStrategy)Enum.Parse(typeof(FunctionalStrategy), eventToRaise);

            selectedElement.RaiseEvent(eventStrategy);
        }

        private TestingElement GetSelectedElement()
        {
            if (cbElements.SelectedIndex != -1)
                return _elements[cbElements.SelectedIndex];

            return null;
        }

        private void WriteEventTracingLine(string message)
        {
            EventTracingOutput += message + "\n";
        }

        public void WriteLine(string message)
        {
            Output += message + "\n";
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            Output = string.Empty;
        }

        private void SetProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestingElement selectedElement = GetSelectedElement();
                string property = ((ComboBoxItem)cbProperty.SelectedItem).Content.ToString();
                string value = tbPropertyValue.Text;

                selectedElement.SetProperty(property, value);
            }
            catch { }
        }

        private void ClearProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestingElement selectedElement = GetSelectedElement();
                string property = ((ComboBoxItem)cbProperty.SelectedItem).Content.ToString();
                string value = tbPropertyValue.Text;

                selectedElement.ClearProperty(property);
            }
            catch { }
        }

        private void GetProperty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestingElement selectedElement = GetSelectedElement();
                string property = ((ComboBoxItem)cbProperty.SelectedItem).Content.ToString();

                selectedElement.WriteProperty(property);
            }
            catch { }
        }

        private void BuildTree_Click(object sender, RoutedEventArgs e)
        {
            BuildTestingTree();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestingElement selectedElement = GetSelectedElement();
                selectedElement.DisconnectFromFunctionalTree();
            }
            catch { }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestingElement selectedElement = GetSelectedElement();
                int index = _elements.IndexOf(selectedElement);
                _elements[_elementOwnerTable[index]].Add(selectedElement);
            }
            catch { }
        }

        private void ResetTree_Click(object sender, RoutedEventArgs e)
        {
            InitElements();
        }

        private void ClearEventTracingOutput_Click(object sender, RoutedEventArgs e)
        {
            EventTracingOutput = string.Empty;
        }

    }
}
