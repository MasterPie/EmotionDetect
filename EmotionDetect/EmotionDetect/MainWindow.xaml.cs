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
using DetectClient;
using System.ComponentModel;

namespace EmotionDetect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            Client client = new Client();
            client.EmotionChanged += new EventHandler<EmotionEventArgs>(displayEmotion);
        }

        public void displayEmotion(object sender, EmotionEventArgs e)
        {
            CurrentEmotion = e.Emotion.ToString();
            this.OnPropertyChanged("CurrentEmotion");
        }

        private string emotion = "";
        public string CurrentEmotion
        {
            set
            {
                emotion = value;
            }
            get
            {
                return emotion;
            }
        }


        void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
