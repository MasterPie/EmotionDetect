// -----------------------------------------------------------------------
// <copyright file="IGameEngine.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGameEngine : ILoggable
    {
        void Initialize();
        void Cleanup();

        void Start();
        void Pause();
        void Stop();

        bool NewCatch(IItem item, IBasket basket);
        void NewFall();

        int CurrentScore { get; }
        int MaxScore { get; }
        int CurrentLevel { get; }

        void WinGame();

        Color[] SelectedColors { get; }
        DetectClient.Client EmotionClassifier { set; }
        IItemFactory ItemFactory { set; }
        ILevelManager LevelManager { set; }

        event EventHandler ScoreUpdated;
        event EventHandler GameStarted;
        event EventHandler GameEnded;
        event EventHandler<ItemSpawnEventArgs> ItemSpawned;
        event EventHandler<ChangeLevelEventArgs> LevelFailed;
        event EventHandler<ChangeLevelEventArgs> LevelCompleted;
        event EventHandler<DetectClient.EmotionEventArgs> NewEmotion;
    }
}
