// -----------------------------------------------------------------------
// <copyright file="EmotionalStateEventArgs.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PostureClassification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmotionalStateEventArgs : EventArgs
    {
        public Dictionary<Label, double> LabelDistribution;

        public static Dictionary<Label, double> DefaultDistribution
        {
            get
            {
                Dictionary<Label, double> dist = new Dictionary<Label, double>();
                foreach (Label label in System.Enum.GetValues(typeof(Label)))
                {
                    dist[label] = 0.0;
                }

                return dist;
            }
        }
    }
}
