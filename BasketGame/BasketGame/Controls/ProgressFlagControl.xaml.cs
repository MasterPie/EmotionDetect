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
    /// Interaction logic for ProgressFlagControl.xaml
    /// </summary>
    public partial class ProgressFlagControl : UserControl
    {
        private DispatcherTimer hoverFlashTimer;
        public ProgressFlagControl()
        {
            InitializeComponent();
            hoverFlashTimer = new DispatcherTimer();
            hoverFlashTimer.Interval = TimeSpan.FromSeconds(1);
            hoverFlashTimer.Tick += new EventHandler(hoverFlashTimer_Tick);

            this.Loaded += new RoutedEventHandler(ProgressBasketControl_Loaded);
        }

        void ProgressBasketControl_Loaded(object sender, RoutedEventArgs e)
        {
            string progress_dir = ((ViewModel)DataContext).ThemeRootDirectory + "/ProgressBar/";
            ((ViewModel)DataContext).ItemCaught += new EventHandler(FallingItemControl_ItemCaught);
            ((ViewModel)DataContext).ItemMissed += new EventHandler(ProgressFlagControl_ItemMissed);
            this.MainBasket.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + progress_dir + "mainbasket.png"));
            this.BasketFlash.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + progress_dir + "mainbasket_hover.png"));
            this.BasketFlashBad.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + progress_dir + "mainbasket_hover_bad.png"));
        }

        void ProgressFlagControl_ItemMissed(object sender, EventArgs e)
        {
            BasketFlashBad.Visibility = System.Windows.Visibility.Visible;

            if (!hoverFlashTimer.IsEnabled)
                hoverFlashTimer.Start();
        }

        void hoverFlashTimer_Tick(object sender, EventArgs e)
        {
            hoverFlashTimer.Stop();
            BasketFlash.Visibility = System.Windows.Visibility.Hidden;
            BasketFlashBad.Visibility = System.Windows.Visibility.Hidden;
        }

        void FallingItemControl_ItemCaught(object sender, EventArgs e)
        {
            BasketFlash.Visibility = System.Windows.Visibility.Visible;

            if (!hoverFlashTimer.IsEnabled)
                hoverFlashTimer.Start();
        }
    }
}
