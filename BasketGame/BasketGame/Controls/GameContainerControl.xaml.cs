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

namespace BasketGame
{
    /// <summary>
    /// Interaction logic for GameContainerControl.xaml
    /// </summary>
    public partial class GameContainerControl : UserControl
    {
        private List<BasketControl> baskets;
        private Dictionary<BasketControl, double> defaultBasketLocations;
        public GameContainerControl()
        {
            InitializeComponent();
            baskets = new List<BasketControl>();
            defaultBasketLocations = new Dictionary<BasketControl, double>();
            this.Loaded += new RoutedEventHandler(GameContainerControl_Loaded);
        }

        void GameContainerControl_Loaded(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket() { Color = Colors.Red };
            Basket basket2 = new Basket() { Color = Colors.Blue };
            Basket basket3 = new Basket() { Color = Colors.Orange };
            Basket basket4 = new Basket() { Color = Colors.Green };
            Basket basket5 = new Basket() { Color = Colors.Yellow };

            Baskets = new List<IBasket>() { basket, basket2, basket3, basket4, basket5 };

            ((ViewModel)DataContext).ItemSpawned += new EventHandler<ItemSpawnEventArgs>(GameContainerControl_ItemSpawned);
            ((ViewModel)DataContext).SelectedColorsChanged += new EventHandler<SelectedColorChangeEventArgs>(GameContainerControl_SelectedColorsChanged);
            SetBasketEnables(((ViewModel)DataContext).SelectedColors);
        }

        void GameContainerControl_SelectedColorsChanged(object sender, SelectedColorChangeEventArgs e)
        {
            SetBasketEnables(e.SelectedColors);
        }

        private void SetBasketEnables(Color[] selectedColors)
        {
            if (selectedColors == null)
                return;

            foreach (BasketControl basket in baskets)
            {
                basket.Opacity = 0.1;

                foreach (Color color in selectedColors)
                {
                    if (basket.BasketModel.Color.Equals(color))
                        basket.Opacity = 1;
                }
            }
        }

        public List<IBasket> Baskets
        {
            set
            {
                LoadBaskets(value);
            }
        }

        private void LoadBaskets(List<IBasket> basketModels)
        {
            double basketSize = 100;
            double margin = 50;
            double leftOffset = (App.Current.MainWindow.ActualWidth - (basketModels.Count * basketSize + (basketModels.Count - 1) * margin))/2;
            foreach (Basket basket in basketModels)
            {
                BasketControl basketControl = new BasketControl() { BasketModel = basket};
                FallingRegion.Children.Add(basketControl);
                Canvas.SetBottom(basketControl, 50);
                Canvas.SetLeft(basketControl, leftOffset);
                baskets.Add(basketControl);
                defaultBasketLocations.Add(basketControl, leftOffset);
                leftOffset += (margin + basketSize); //TODO: actually calculate this properly
            }
        }

        void GameContainerControl_ItemSpawned(object sender, ItemSpawnEventArgs e)
        {
            FallingItemControl newItem = new FallingItemControl() { ItemModel = e.Item};
            FallingRegion.Children.Add(newItem);
            Canvas.SetTop(newItem, 0);
            Canvas.SetLeft(newItem, e.DropOffset*App.Current.MainWindow.ActualWidth - newItem.ActualWidth / 2); //TODO:make sure stuff doesn't go out of bounds
        }

        void basket_MouseMove(object sender, MouseEventArgs e)
        {
            BasketControl basket = (BasketControl)sender;
            Canvas canvas = (Canvas)basket.Parent;
            Point p = e.GetPosition(canvas);

            if (basket.IsMouseCaptured == true)
            {
                canvas.Children.Remove(basket);
                Canvas.SetTop(basket, p.Y - basket.ActualHeight / 2);
                Canvas.SetLeft(basket, p.X - basket.ActualWidth / 2);
                canvas.Children.Add(basket);
            }
        }

        private void FallingRegion_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            Point p = e.GetPosition(canvas);

            HitTestResult testResult = VisualTreeHelper.HitTest(canvas, p);
            //Console.WriteLine(p.ToString());
            foreach (BasketControl basket in baskets)
            {
                if (testResult != null && ((UIElement)testResult.VisualHit).IsDescendantOf(basket))
                {
                    basket.MouseMove += new MouseEventHandler(basket_MouseMove);
                    basket.CaptureMouse();
                }
            }
        }

        private void FallingRegion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            foreach (BasketControl basket in baskets) //TODO: change this out for something more efficient
            {
                Point p = e.GetPosition(canvas);

                if (basket.IsMouseCaptured)
                {
                    basket.MouseMove -= new MouseEventHandler(basket_MouseMove);
                    basket.ReleaseMouseCapture();
                    ResetBasketLocation(basket);
                }
            }
        }

        private void ResetBasketLocation(BasketControl basket)
        {
            Canvas canvas = (Canvas)basket.Parent;
            canvas.Children.Remove(basket);
            Canvas.SetTop(basket, App.Current.MainWindow.ActualHeight - basket.Height - 50);
            Canvas.SetLeft(basket, defaultBasketLocations[basket]);
            canvas.Children.Add(basket);
        }


    }
}
