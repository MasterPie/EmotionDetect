// -----------------------------------------------------------------------
// <copyright file="Classifier.cs" company="">
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
    public class Classifier
    {
        private List<SkeletonData> buffer;

        public Classifier()
        {
            buffer = new List<SkeletonData>();
        }

        public void TrackPosture(SkeletonData skeleton)
        {
            //CheckPostures(skeleton);

            ScanPastPositions(skeleton);

            buffer.Add(skeleton);
        }

        private void ScanPastPositions(SkeletonData skeleton)
        {
            bool emotionFound = false;
            //check if posture details an emotion
            Dictionary<Label, double> emotionalDist = EmotionalStateEventArgs.DefaultDistribution;

            if (HandBehindNeck(skeleton, JointID.HandLeft) || HandBehindNeck(skeleton, JointID.HandRight))
            {
                emotionalDist[Label.Apprehensive]++;
                emotionFound = true;
            }
            
            if (emotionFound)
                RaiseNewEmotionEvent(Normalize(emotionalDist));

            buffer.Clear();
        }

        private void CheckPostures(SkeletonData skeleton)
        {
            List<Posture> postures = new List<Posture>();
         
            if (HandBehindNeck(skeleton, JointID.HandLeft) || HandBehindNeck(skeleton, JointID.HandRight))
                RaiseNewPosturesEvent(postures);
        }

        public void RaiseNewPosturesEvent(List<Posture> postures)
        {
            if (NewPostures != null)
                NewPostures(this, new PosturesEventArgs() { Postures = postures });
        }

        public void RaiseNewEmotionEvent(Dictionary<Label, double> emotionDistribution)
        {
            if(NewEmotion != null)
                NewEmotion(this, new EmotionalStateEventArgs(){ LabelDistribution = emotionDistribution});
        }

        private Dictionary<Label, double> Normalize(Dictionary<Label, double> dist)
        {
            Dictionary<Label, double> retDist = dist;

            double sum = dist.Sum(x=>x.Value);
            Label[] keys = dist.Keys.ToArray();
            for(int i = 0; i < dist.Keys.Count; i++){
                Label label = keys[i];
                retDist[label] = retDist[label] / sum;
            }

            return retDist;
        }

        #region Postures

        public bool HandBehindNeck(SkeletonData skeleton, JointID handType)
        {
            var hand = skeleton.Joints[handType].Position;
            var head = skeleton.Joints[JointID.Head].Position;

            return (hand.Z > head.Z && (hand.Z - head.Z) < 0.2f 
                && Overlapping(hand.X, head.X, 0.15f) 
                && Overlapping(hand.Y, head.Y, 0.2f) && hand.Y <= head.Y);
        }


        private bool Overlapping(float j1, float j2)
        {
            return Overlapping(j1,j2,0.1f);
        }

        private bool Overlapping(float j1, float j2, float threshold)
        {
            return Math.Abs(j1 - j2) < threshold;
        }

        #endregion

        public event EventHandler<PosturesEventArgs> NewPostures;
        public event EventHandler<EmotionalStateEventArgs> NewEmotion;
    }
}
