// -----------------------------------------------------------------------
// <copyright file="BodyState.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PostureClassification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Research.Kinect.Nui;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BodyStateEventArgs: EventArgs
    {
        public Posture Posture;
        private JointsCollection joints;
        //public BodyStateEventArgs(JointsCollection jointsCollection)
        //{
        //    joints = jointsCollection;
        //}


        public bool RightHandBehindNeck
        {
            get { return false; }
        }

        public bool Standing
        {
            get { return false; }
        }

        public bool Sitting
        {
            get { return false; }
        }
    }
}
