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
using System.Windows.Threading;

namespace BasketGame
{
    /// <summary>
    /// Interaction logic for EndGameSplashControl.xaml
    /// </summary>
    public partial class EndGameSplashControl : UserControl
    {
        private DispatcherTimer soundStartTimer;
        private MediaPlayer soundPlayer;
        public EndGameSplashControl()
        {
            InitializeComponent();

            soundPlayer = new MediaPlayer();
            soundPlayer.Volume = 1.0;
            soundStartTimer = new DispatcherTimer();
            soundStartTimer.Interval = TimeSpan.FromSeconds(2);
            soundStartTimer.Tick += new EventHandler(soundStartTimer_Tick);
            this.Loaded += new RoutedEventHandler(EndGameSplashControl_Loaded);
        }

        void soundStartTimer_Tick(object sender, EventArgs e)
        {
            soundStartTimer.Stop();
            soundPlayer.Open(new Uri("Music/thanks.wav", UriKind.RelativeOrAbsolute));
            soundPlayer.Play();
        }

        void EndGameSplashControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((ViewModel)DataContext).GameEnded += new EventHandler(EndGameSplashControl_GameEnded);
        }

        void EndGameSplashControl_GameEnded(object sender, EventArgs e)
        {
            soundStartTimer.Start();
        }
    }
}
