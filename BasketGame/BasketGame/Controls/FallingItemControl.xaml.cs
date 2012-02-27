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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BasketGame
{
    /// <summary>
    /// Interaction logic for FallingItemControl.xaml
    /// </summary>
    public partial class FallingItemControl : UserControl
    {
        private MediaPlayer soundPlayer;
        private DispatcherTimer disposeTimer;
        private double dropInterval = 0.0;
        public FallingItemControl()
        {
            InitializeComponent();
            disposeTimer = new DispatcherTimer();
            disposeTimer.Interval = TimeSpan.FromSeconds(1);
            disposeTimer.Tick += new EventHandler(disposeTimer_Tick);

            soundPlayer = new MediaPlayer();
        }

        void disposeTimer_Tick(object sender, EventArgs e)
        {
            disposeTimer.Stop();
            ((Canvas)this.Parent).Children.Remove(this);
        }

        private IItem itemModel = null;
        public IItem ItemModel
        {
            set
            {
                itemModel = value;
                if (itemModel.DropSpeed >= 2)
                    dropInterval = itemModel.DropSpeed;
                else
                    dropInterval = 2;
                ItemImage.SetResourceReference(Image.SourceProperty, itemModel.AssignedColor.ToString() + "Item");
                if (itemModel.AssignedColor.Equals(Colors.Red))
                    Splat.Source = new BitmapImage(new Uri("pack://application:,,,/Images/rsplat.png"));
                if (itemModel.AssignedColor.Equals(Colors.Yellow))
                    Splat.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ysplat.png"));
                if (itemModel.AssignedColor.Equals(Colors.Orange))
                    Splat.Source = new BitmapImage(new Uri("pack://application:,,,/Images/osplat.png"));
                if (itemModel.AssignedColor.Equals(Colors.Blue))
                    Splat.Source = new BitmapImage(new Uri("pack://application:,,,/Images/bsplat.png"));
                if (itemModel.AssignedColor.Equals(Colors.Green))
                    Splat.Source = new BitmapImage(new Uri("pack://application:,,,/Images/gsplat.png"));
            
                Dummy.Fill = new SolidColorBrush(itemModel.AssignedColor);
                to = dropInterval;
                Fall(dropInterval);
            }
            get
            {
                return itemModel;
            }
        }

        private double to = 0;
        public void Fall(double fallTo)
        {
            BeginAnimation(Canvas.TopProperty, GetAnimation(fallTo, 50));
        }

        private DoubleAnimation GetAnimation(double newValue, int interval)
        {
            DoubleAnimation animation = new DoubleAnimation(newValue, new Duration(TimeSpan.FromMilliseconds(interval)));
            animation.Completed += new EventHandler(animation_Completed);
            return animation;
        }

        void animation_Completed(object sender, EventArgs e)
        {
            BasketControl basket = null;
            if (HitGround())
            {
                ((ViewModel)DataContext).ItemHitGround();
                soundPlayer.Open(new Uri("Music/splat.mp3",UriKind.RelativeOrAbsolute));
                soundPlayer.Volume = 1;
                soundPlayer.Play();
                ItemImage.Visibility = System.Windows.Visibility.Hidden;
                Splat.Visibility = System.Windows.Visibility.Visible;

                disposeTimer.Start();
            }
            else if ((basket = HitBasket()) != null && ((ViewModel)DataContext).NewCatch(this.itemModel, basket.BasketModel))
            {
                soundPlayer.Open(new Uri("Music/poof.mp3", UriKind.RelativeOrAbsolute));
                soundPlayer.Volume = 1;
                soundPlayer.Play();
                ItemImage.Visibility = System.Windows.Visibility.Hidden;
                Poof.Visibility = System.Windows.Visibility.Visible;
                disposeTimer.Interval = TimeSpan.FromMilliseconds(500);
                disposeTimer.Start();
            }
            else
            {
                to += dropInterval;
                Fall(to);
            }
        }

        private bool HitGround()
        {
            return (double)GetValue(Canvas.TopProperty) >= (App.Current.MainWindow.ActualHeight - 25);
        }

        private BasketControl HitBasket()
        {
            Point p = this.TranslatePoint(new Point(), (UIElement)this.Parent);
            
            for (int index = 0; index < VisualTreeHelper.GetChildrenCount((DependencyObject)this.Parent); index++)
            {
                Object visualObject = VisualTreeHelper.GetChild((DependencyObject)this.Parent, index);
                if (visualObject.GetType().Equals(typeof(BasketControl)))
                {
                    BasketControl basket = (BasketControl)visualObject;
                    
                    Point p_to = basket.TranslatePoint(new Point(), (UIElement)this.Parent);

                    if (Math.Abs(p.X - p_to.X ) - basket.ActualWidth <= 2 && Math.Abs(p.Y - p_to.Y ) - basket.ActualHeight /(3/2) <= 2)
                    {
                        return basket;
                    }
                }
            }
            return null;
        }
    }
}
