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
using System.Diagnostics;

namespace BasketGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer billyPlayer;
        public MainWindow()
        {
            InitializeComponent();
            billyPlayer = new MediaPlayer();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((ViewModel)DataContext).GameEnded += new EventHandler(MainWindow_GameEnded);
            ((ViewModel)DataContext).BillyConcerned += new EventHandler(MainWindow_BillyConcerned);
            ((ViewModel)DataContext).BillyHappy += new EventHandler(MainWindow_BillyHappy);
            ((ViewModel)DataContext).BillyInstructs += new EventHandler(MainWindow_BillyInstructs);
        }

        void MainWindow_BillyInstructs(object sender, EventArgs e)
        {
            //billyPlayer.Stop();
            //billyPlayer.Open(new Uri("Music/usebaskets.wav", UriKind.RelativeOrAbsolute));
            //billyPlayer.Volume = 1;
            //billyPlayer.Play();
        }

        void MainWindow_BillyHappy(object sender, EventArgs e)
        {
            //billyPlayer.Stop();
            //billyPlayer.Open(new Uri("Music/keepitup.wav", UriKind.RelativeOrAbsolute));
            //billyPlayer.Volume = 1;
            //billyPlayer.Play();
        }

        void MainWindow_BillyConcerned(object sender, EventArgs e)
        {
            //billyPlayer.Stop();
            //billyPlayer.Open(new Uri("Music/ohno.wav", UriKind.RelativeOrAbsolute));
            //billyPlayer.Volume = 1;
            //billyPlayer.Play();
        }

        void MainWindow_GameEnded(object sender, EventArgs e)
        {
            BackgroundMusic.Volume = 0.0;
            EndGame.Visibility = System.Windows.Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.D)
                ((ViewModel)DataContext).ToggleDebug();
            if(e.Key == Key.W)
                ((ViewModel)DataContext).WinGame();
            if(e.Key == Key.P || e.Key == Key.F3)
                ((ViewModel)DataContext).Pause();
            if (e.Key == Key.S || e.Key == Key.R)
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            if (e.Key == Key.Q)
            {
                ((ViewModel)DataContext).Cleanup();
                this.Close();
            }
        }
    }
}
