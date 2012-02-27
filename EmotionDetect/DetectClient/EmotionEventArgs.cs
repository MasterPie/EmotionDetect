// -----------------------------------------------------------------------
// <copyright file="EmotionEventArgs.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DetectClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmotionEventArgs : EventArgs
    {
        public Label Emotion{get;set;}
    }

    public enum Label { Neutral, Happy, Sad, Fear, Disgust, Contempt, Anger, Surprise}
}
