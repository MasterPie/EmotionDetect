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
        public GameContainerControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GameContainerControl_Loaded);
        }

        void GameContainerControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((ViewModel)DataContext).ItemSpawned += new EventHandler<ItemSpawnEventArgs>(GameContainerControl_ItemSpawned);

            BasketControl basket = new BasketControl() { BasketModel = new Basket() { Color = Colors.Red } };
            FallingRegion.Children.Add(basket);
            Canvas.SetBottom(basket, 0);
            

        }

        void GameContainerControl_ItemSpawned(object sender, ItemSpawnEventArgs e)
        {
            FallingItemControl newItem = new FallingItemControl() { ItemModel = e.Item};
            FallingRegion.Children.Add(newItem);
            Canvas.SetTop(newItem, 0);
            Canvas.SetLeft(newItem, e.DropOffset);
            System.Console.WriteLine("a {0} item spawned at: {1}", e.Item.AssignedColor.ToString(), e.DropOffset);
        }
    }
}
