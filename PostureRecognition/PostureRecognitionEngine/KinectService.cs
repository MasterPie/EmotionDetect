// -----------------------------------------------------------------------
// <copyright file="KinectService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PostureRecognitionEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Research.Kinect.Nui;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KinectService: IDisposable
    {
        private Runtime kinect;
        public KinectService()
        {
            while (Runtime.Kinects.Count <= 0 || ((kinect = Runtime.Kinects[0]) == null) || kinect.Status != KinectStatus.Connected)
            {
                System.Console.WriteLine("Waiting to establish connection with Kinect.");
                System.Threading.Thread.Sleep(2000);
            }

            System.Console.WriteLine("Kinect initialized.");

            kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);
            kinect.Initialize(RuntimeOptions.UseSkeletalTracking);
            //kinect.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);

            kinect.SkeletonEngine.TransformSmooth = true;
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 1.0f,
                Correction = 0.1f,
                Prediction = 0.1f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.05f
            };
            kinect.SkeletonEngine.SmoothParameters = parameters;
        }

        public SkeletonEngine Engine
        {
            get { return kinect.SkeletonEngine; }
        }

        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonData skeleton = e.SkeletonFrame.Skeletons.Where(x => x.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();

            if (skeleton == null)
                return;

            if (PostureChanged != null)
                PostureChanged(this, new SkeletonEventArgs() {Skeleton = skeleton });

            if (SkeletonFrameUpdated != null)
                SkeletonFrameUpdated(this, new SkeletonEventArgs() { SkeletonFrame = e.SkeletonFrame });
        }

        public void Dispose()
        {
            kinect.Uninitialize();
        }

        public event EventHandler<SkeletonEventArgs> PostureChanged;
        public event EventHandler<SkeletonEventArgs> SkeletonFrameUpdated;
    }
}
