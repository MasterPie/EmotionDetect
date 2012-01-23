// -----------------------------------------------------------------------
// <copyright file="SkeletonEventArgs.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PostureRecognitionEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SkeletonEventArgs :EventArgs
    {
        public Microsoft.Research.Kinect.Nui.SkeletonData Skeleton;
        public Microsoft.Research.Kinect.Nui.SkeletonFrame SkeletonFrame;
    }
}
