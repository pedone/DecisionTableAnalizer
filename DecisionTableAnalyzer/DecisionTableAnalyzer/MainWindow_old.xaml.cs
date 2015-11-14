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
using Microsoft.Win32;

namespace DecisionTableAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow_old : Window
    {

        //public DTCondition CurrentCondition { get; private set; }
        //public DTAction CurrentAction { get; private set; }
        //public DTState CurrentState { get; private set; }
        //public DecisionTableViewModel DecisionTableViewModel { get; set; }

        public GridView TableView { get; private set; }

        public MainWindow_old()
        {
            //CurrentAction = new DTAction();
            //CurrentCondition = new DTCondition();
            //CurrentState = new DTState();

            //DecisionTableViewModel = new DecisionTableViewModel();
            TableView = new GridView();

            InitializeComponent();
        }

        private void AddCondition_Click(object sender, RoutedEventArgs e)
        {
            //Manually (for now) set the valid states
            //string[] validStates = tbValidStates.Text.Split(';');
            //CurrentCondition.SetValidStates(validConditionStates);

            //if (DecisionTableViewModel.Add(new DTCondition(CurrentCondition)))
            //    CurrentCondition.Reset();
        }

        private void AddAction_Click(object sender, RoutedEventArgs e)
        {
            //if (DecisionTableViewModel.Add(new DTAction(CurrentAction)))
            //    CurrentAction.Reset();
        }

        private void ConditionsViewSource_Filter(object sender, FilterEventArgs e)
        {
            //e.Accepted = e.Item is DTCondition;
        }

        private void ActionsViewSource_Filter(object sender, FilterEventArgs e)
        {
            //e.Accepted = e.Item is DTAction;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (saveDialog.ShowDialog() == true)
            {
                //DecisionTableViewModel.Save(saveDialog.FileName);
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                CheckFileExists = true
            };

            if (openDialog.ShowDialog() == true)
            {
                //DecisionTableViewModel.Load(openDialog.FileName);
                //if (DecisionTableViewModel.IsTableInitialized)
                //    dtDecisionTable.ResetTable();
            }
        }

        private void CreateTable_Click(object sender, RoutedEventArgs e)
        {
            //dtDecisionTable.ResetTable();
            //DecisionTable.InitializeRows();
            //TableView.Columns.Clear();

            //TableView.Columns.Add(new GridViewColumn { Width = 40, DisplayMemberBinding = new Binding("Symbol") });
            //TableView.Columns.Add(new GridViewColumn { Header = "Description", Width = 200, DisplayMemberBinding = new Binding("Element.Description") });

            //DataTemplate cellTemplate = TryFindResource("RowTemplate") as DataTemplate;

            //for (int i = 0; i < DecisionTable.ColumnCount; i++)
            //{
            //    GridViewColumn gridColumn = new GridViewColumn
            //    {
            //        Header = "R" + (i + 1),
            //        Width = 40,
            //        //DisplayMemberBinding = new Binding(string.Format("States[{0}]", i)),
            //        CellTemplateSelector = new DTCellTemplateSelector()
            //    };

            //    TableView.Columns.Add(gridColumn);
            //}
        }

        private void AddState_Click(object sender, RoutedEventArgs e)
        {
        //    if (DecisionTableViewModel.Add(new DTState(CurrentState)))
        //        CurrentState.Reset();
        }

        private void RemoveState_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveCondition_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveAction_Click(object sender, RoutedEventArgs e)
        {

        }

        //List<DTState> validConditionStates = new List<DTState>();
        private void State_Checked(object sender, RoutedEventArgs e)
        {
            //ListViewItem item = TreeHelper.FindAncestor<ListViewItem>(sender as DependencyObject);
            //if (item == null)
            //    return;

            //DTState state = item.Content as DTState;
            //validConditionStates.Add(state);
        }

        private void State_Unchecked(object sender, RoutedEventArgs e)
        {
            //ListViewItem item = TreeHelper.FindAncestor<ListViewItem>(sender as DependencyObject);
            //if (item == null)
            //    return;

            //DTState state = item.Content as DTState;
            //validConditionStates.Remove(state);
        }

    }
}
