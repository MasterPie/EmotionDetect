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
        public enum Label {Happy, Neutral}

        public Label Emotion{get;set;}
    }
}
