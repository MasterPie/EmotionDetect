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
using Kinect.Toolbox;

namespace PostureRecognitionEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SkeletonDisplayManager displayManager;
        public MainWindow()
        {
            InitializeComponent();

            ((ViewModel)DataContext).SkeletonFrameUpdated += new EventHandler(MainWindow_SkeletonFrameUpdated);

            displayManager = new SkeletonDisplayManager(((ViewModel)DataContext).KinectService.Engine, Skeleton);

        }

        void MainWindow_SkeletonFrameUpdated(object sender, EventArgs e)
        {
            displayManager.Draw(((ViewModel)DataContext).SkeletonFrame);
        }
    }
}
