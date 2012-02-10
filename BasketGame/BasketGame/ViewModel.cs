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

            engine.Start();
        }

        void gameEngine_ScoreUpdated(object sender, EventArgs e)
        {
            System.Console.WriteLine("Score is now: " + engine.CurrentScore);
            this.OnPropertyChanged("Score");
            //throw new NotImplementedException();
        }

        void gameEngine_LevelFailed(object sender, ChangeLevelEventArgs e)
        {
            System.Console.WriteLine("Moving back one level!");
        }

        void gameEngine_LevelCompleted(object sender, ChangeLevelEventArgs e)
        {
            System.Console.WriteLine("Going up one level!");
        }

        void gameEngine_ItemSpawned(object sender, ItemSpawnEventArgs e)
        {
            if (ItemSpawned != null)
                ItemSpawned(this, e);
        }

        public int Score
        {
            get
            {
                return engine.CurrentScore;
            }
        }

        public void ItemHitGround()
        {
            engine.NewFall();
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

        public event EventHandler<ItemSpawnEventArgs> ItemSpawned;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
