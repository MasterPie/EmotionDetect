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

namespace BasketGame
{
    /// <summary>
    /// Interaction logic for FallingItemControl.xaml
    /// </summary>
    public partial class FallingItemControl : UserControl
    {
        public FallingItemControl()
        {
            InitializeComponent();
            Fall(10);
        }

        private IItem itemModel = null;
        public IItem ItemModel
        {
            set
            {
                itemModel = value;
                Dummy.Fill = new SolidColorBrush(itemModel.AssignedColor);
            }
            get
            {
                return itemModel;
            }
        }

        private double to = 10;
        public void Fall(double fallTo)
        {
            BeginAnimation(Canvas.TopProperty, GetAnimation(fallTo, 200));
        }

        private DoubleAnimation GetAnimation(double newValue, int interval)
        {
            DoubleAnimation animation = new DoubleAnimation(newValue, new Duration(TimeSpan.FromMilliseconds(interval)));
            animation.Completed += new EventHandler(animation_Completed);
            return animation;
        }

        void animation_Completed(object sender, EventArgs e)
        {
            if (HitGround())
            {
                ((ViewModel)DataContext).ItemHitGround();
                System.Console.WriteLine("Hit Ground!");
                ((Canvas)this.Parent).Children.Remove(this);
            }
            else
            {
                to += 10;
                Fall(to);
            }
        }

        private bool HitGround()
        {
            return (double)GetValue(Canvas.TopProperty) >= (App.Current.MainWindow.ActualHeight - 50);
        }
    }
}
