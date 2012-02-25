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
            if (positiveStreak >= STREAK_THRESHOLD && (currentEmotion == Label.Happy || currentEmotion == Label.Neutral))
                AdvanceLevel();
            else if (negativeStreak >= STREAK_THRESHOLD || currentEmotion == Label.Sad)
            {
                //if (currentEmotion == Label.Sad)
                //    ite - 15;
                //TODO: have to decrease score count too because the score was not decreased
                RegressLevel();
            }

            SpawnItem();
        }
    }
}
