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
        private int checkRegressTime = 0;

        protected override void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            if (positiveStreak >= STREAK_THRESHOLD) //no longer requiring positiveness to win
                AdvanceLevel();
            else if (negativeStreak >= STREAK_THRESHOLD)
            {
                checkRegressTime = 5;
                RegressLevel();
            }
            else if (Negative())
            {
                if (checkRegressTime <= 0)
                {
                    lock (this.scoreLock)
                    {
                        if (this.CurrentScore >= STREAK_THRESHOLD)
                        {
                            this.itemsCollected = STREAK_THRESHOLD * (CurrentLevel - 1) - STREAK_THRESHOLD;
                        }
                    }
                    RegressLevel();
                    checkRegressTime = 5;
                }
                else
                    checkRegressTime--;
            }

            if (Positive())
                checkRegressTime = 0;

            SpawnItem();
        }

        private bool Negative()
        {
            return (currentEmotion == Label.Surprise || currentEmotion == Label.Anger || currentEmotion == Label.Disgust || currentEmotion == Label.Fear);
        }

        private bool Positive()
        {
            return !Negative();
        }

        public override string UniqueSessionID
        {
            get { return base.UniqueSessionID + "_AFFECTMEDIATED"; }
        }
    }
}
