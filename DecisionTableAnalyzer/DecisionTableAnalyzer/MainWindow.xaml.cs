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
using DTCore;
using ViewModels;

namespace DecisionTableAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            ViewService.Instance.Init(dmMainDockManager);
            App.Current.ApplicationViewModel.DockManager = dmMainDockManager;
            DataContext = App.Current.ApplicationViewModel;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Current.ApplicationViewModel.Settings.ShowStartPageOnStartup)
            {
                StartViewModel startViewModel = new StartViewModel { ApplicationViewModel = App.Current.ApplicationViewModel };
                ViewService.Instance.ShowView(startViewModel);
            }
        }

    }
}
