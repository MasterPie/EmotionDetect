// -----------------------------------------------------------------------
// <copyright file="AffectMediatedGameEngine.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using DetectClient;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffectMediatedGameEngine : SimpleGameEngine
    {
        protected override void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            if (positiveStreak >= STREAK_THRESHOLD && !(currentEmotion == Label.Anger || currentEmotion == Label.Disgust))
                AdvanceLevel();
            else if (negativeStreak >= STREAK_THRESHOLD || (/*currentEmotion == Label.Surprise || */currentEmotion == Label.Anger || currentEmotion == Label.Disgust))
            {
                if((/*currentEmotion == Label.Surprise || */currentEmotion == Label.Anger || currentEmotion == Label.Disgust) && 
                    this.CurrentScore >= 15)
                {
                    this.itemsCollected = (this.itemsCollected - 15);
                }
                RegressLevel();
            }

            SpawnItem();
        }

        public override string UniqueSessionID
        {
            get { return base.UniqueSessionID + "_AFFECTMEDIATED"; }
        }
    }
}
