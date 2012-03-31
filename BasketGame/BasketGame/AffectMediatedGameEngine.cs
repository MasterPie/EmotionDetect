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
        private Label lastEmotion;

        protected override void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            if (positiveStreak >= STREAK_THRESHOLD)
                AdvanceLevel();
            else if (Negative())
            {
                if (!Negative(this.lastEmotion))
                {
                    lock (this.scoreLock)
                    {
                        if (this.CurrentScore >= STREAK_THRESHOLD)
                        {
                            //this.itemsCollected = STREAK_THRESHOLD * (CurrentLevel - 1) - STREAK_THRESHOLD;
                            this.itemsCollected = this.itemsCollected - STREAK_THRESHOLD;
                            if (this.itemsCollected < 0)
                                itemsCollected = 0;
                        }
                    }
                    RegressLevel();
                }
            }
            else if (negativeStreak >= STREAK_THRESHOLD)
            {
                RegressLevel();
            }

            lastEmotion = this.currentEmotion;

            SpawnItem();
        }

        private bool Negative()
        {
            return Negative(currentEmotion);
        }
        private bool Negative(Label emotion)
        {
            return (emotion == Label.Surprise || emotion == Label.Anger || emotion == Label.Disgust || emotion == Label.Fear);
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
