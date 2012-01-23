// -----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PostureRecognitionEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PostureClassification;
    using System.ComponentModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewModel: INotifyPropertyChanged
    {
        private KinectService kinect;
        private Classifier classifier;

        public ViewModel() 
        {
            System.Console.WriteLine("Starting up Posture Recognizer");
            classifier = new Classifier();
            classifier.NewEmotion += new EventHandler<EmotionalStateEventArgs>(classifier_NewEmotion);
            classifier.NewPostures += new EventHandler<PosturesEventArgs>(classifier_NewPostures);

            kinect = new KinectService();
            kinect.PostureChanged += new EventHandler<SkeletonEventArgs>(kinect_PostureChanged);
            kinect.SkeletonFrameUpdated += new EventHandler<SkeletonEventArgs>(kinect_SkeletonFrameUpdated);
        }

        void kinect_SkeletonFrameUpdated(object sender, SkeletonEventArgs e)
        {
            frame = e.SkeletonFrame;
            if (SkeletonFrameUpdated != null)
                SkeletonFrameUpdated(this, new EventArgs());
        }

        public KinectService KinectService
        {
            get { return kinect; }
        }

        private Microsoft.Research.Kinect.Nui.SkeletonFrame frame;
        public Microsoft.Research.Kinect.Nui.SkeletonFrame SkeletonFrame
        {
            get { return frame; }
        }

        void kinect_PostureChanged(object sender, SkeletonEventArgs e)
        {
            classifier.TrackPosture(e.Skeleton);
        }

        void classifier_NewPostures(object sender, PosturesEventArgs e)
        {
            return;
        }

        void classifier_NewEmotion(object sender, EmotionalStateEventArgs e)
        {
            System.Console.WriteLine("Emotion detected! " + DateTime.Now.ToLongTimeString());
            PrintEmotionalDist(e.LabelDistribution);
        }

        void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void PrintEmotionalDist(Dictionary<Label, double> dist)
        {
            Label[] keys = dist.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
                System.Console.Write(keys[i] + ": " + dist[keys[i]] + " ");

            System.Console.WriteLine();
            System.Console.WriteLine();
        }

        public void Cleanup()
        {

        }

        public event EventHandler SkeletonFrameUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
