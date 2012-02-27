// -----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame

{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Windows.Media;

    using DetectClient;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {

        private IGameEngine engine = null;
        private ILogger log = null;

        public ViewModel(IGameEngine gameEngine, ILogger logger)
        {
            engine = gameEngine;
            log = logger;

            gameEngine.ItemSpawned += new EventHandler<ItemSpawnEventArgs>(gameEngine_ItemSpawned);
            gameEngine.LevelCompleted += new EventHandler<ChangeLevelEventArgs>(gameEngine_LevelCompleted);
            gameEngine.LevelFailed += new EventHandler<ChangeLevelEventArgs>(gameEngine_LevelFailed);
            gameEngine.ScoreUpdated += new EventHandler(gameEngine_ScoreUpdated);
            gameEngine.GameStarted += new EventHandler(gameEngine_GameStarted);
            gameEngine.GameEnded += new EventHandler(gameEngine_GameEnded);
            gameEngine.NewEmotion += new EventHandler<DetectClient.EmotionEventArgs>(gameEngine_NewEmotion);

            engine.Start();
        }

        void gameEngine_GameEnded(object sender, EventArgs e)
        {
            if (GameEnded != null)
                GameEnded(this, new EventArgs());
        }

        void gameEngine_NewEmotion(object sender, DetectClient.EmotionEventArgs e)
        {
            lastLabel = e.Emotion;
            this.OnPropertyChanged("Emotion");
        }


        void gameEngine_GameStarted(object sender, EventArgs e)
        {
            if (SelectedColorsChanged != null)
                SelectedColorsChanged(this, new SelectedColorChangeEventArgs() { SelectedColors = engine.SelectedColors });
        }

        void gameEngine_ScoreUpdated(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Score");
        }

        void gameEngine_LevelFailed(object sender, ChangeLevelEventArgs e)
        {
            System.Console.WriteLine("Going down one level!");
            if (SelectedColorsChanged != null)
                SelectedColorsChanged(this, new SelectedColorChangeEventArgs() { SelectedColors = engine.SelectedColors });
        }

        void gameEngine_LevelCompleted(object sender, ChangeLevelEventArgs e)
        {
            System.Console.WriteLine("Going up one level!");
            if (SelectedColorsChanged != null)
                SelectedColorsChanged(this, new SelectedColorChangeEventArgs() {SelectedColors = engine.SelectedColors });
        }

        void gameEngine_ItemSpawned(object sender, ItemSpawnEventArgs e)
        {
            if (ItemSpawned != null)
                ItemSpawned(this, e);
        }

        public bool NewCatch(IItem item, IBasket basket)
        {
            if (ItemCaught != null)
                ItemCaught(this, new EventArgs());
            return engine.NewCatch(item, basket);
        }

        public void ItemHitGround()
        {
            engine.NewFall();
        }

        private Label lastLabel;
        public string Emotion
        {
            get
            {
                return lastLabel.ToString();
            }
        }

        public Color[] SelectedColors
        {
            get
            {
                return engine.SelectedColors;
            }
        }

        public int Score
        {
            get
            {
                return engine.CurrentScore;
            }
        }

        public void Cleanup()
        {
            engine.Stop();
            engine.Cleanup();
        }

        void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event EventHandler<SelectedColorChangeEventArgs> SelectedColorsChanged;
        public event EventHandler<ItemSpawnEventArgs> ItemSpawned;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ItemCaught;
        public event EventHandler GameEnded;
    }
}
