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

            }
            get
            {
                return basket;
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && basket != null)
            {
                Point screen = e.GetPosition(App.Current.MainWindow);
                System.Console.WriteLine("Moving to ({0},{1})", screen.X, screen.Y);
                //this.RenderTransform = new TranslateTransform() {X = screen.X, Y = screen.Y };
                //Canvas.SetLeft(this,screen.X);
                //Canvas.SetTop(this,screen.Y);
                // Package the data.
                DataObject data = new DataObject();
                data.SetData("Object", basket);
                data.SetData("Object", this);

                // Inititate the drag-and-drop operation.
                //DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            
        }
    }
}
