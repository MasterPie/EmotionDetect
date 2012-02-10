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

            engine.Start();
        }

        void gameEngine_GameStarted(object sender, EventArgs e)
        {
            if (SelectedColorsChanged != null)
                SelectedColorsChanged(this, new SelectedColorChangeEventArgs() { SelectedColors = engine.SelectedColors });
        }

        void gameEngine_ScoreUpdated(object sender, EventArgs e)
        {
            this.OnPropertyChanged("Score");
            //throw new NotImplementedException();
        }

        void gameEngine_LevelFailed(object sender, ChangeLevelEventArgs e)
        {
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
            return engine.NewCatch(item, basket);
        }

        public void ItemHitGround()
        {
            engine.NewFall();
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
    }
}
