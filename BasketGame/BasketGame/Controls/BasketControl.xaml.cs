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
    /// Interaction logic for BasketControl.xaml
    /// </summary>
    public partial class BasketControl : UserControl
    {
        public BasketControl()
        {
            InitializeComponent();
        }

        private IBasket basket;
        public IBasket BasketModel
        {
            set
            {
                basket = value;
                BasketImage.SetResourceReference(Image.SourceProperty, basket.Color.ToString() + "Basket");
                IconImage.SetResourceReference(Image.SourceProperty, basket.Color.ToString() + "Item");
            }
            get
            {
                return basket;
            }

        }

        public SolidColorBrush Color
        {
            get
            {
                return new SolidColorBrush(basket.Color);
            }
        }
    }
}
