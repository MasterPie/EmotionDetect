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
    using System.Windows;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private IGameEngine engine = null;
        private ILogger log = null;

        private int itemsCaught = 0;
        private int itemsDropped = 0;

        public ViewModel(IGameEngine gameEngine, ILogger logger)
        {
            engine = gameEngine;
            log = logger;

            engine.ItemSpawned += new EventHandler<ItemSpawnEventArgs>(gameEngine_ItemSpawned);
            engine.LevelCompleted += new EventHandler<ChangeLevelEventArgs>(gameEngine_LevelCompleted);
            engine.LevelFailed += new EventHandler<ChangeLevelEventArgs>(gameEngine_LevelFailed);
            engine.ScoreUpdated += new EventHandler(gameEngine_ScoreUpdated);
            engine.GameStarted += new EventHandler(gameEngine_GameStarted);
            engine.GameEnded += new EventHandler(gameEngine_GameEnded);
            engine.NewEmotion += new EventHandler<DetectClient.EmotionEventArgs>(gameEngine_NewEmotion);

            engine.Start();
            log.Start(engine);
        }

        private bool debugOn = false;
        public void ToggleDebug()
        {
            debugOn = !debugOn;
            this.OnPropertyChanged("ShowDebugging");
        }

        public string EmotionLetter
        {
            get
            {
                switch (lastLabel)
                {
                    case Label.Anger: return "A";
                    case Label.Contempt: return "C";
                    case Label.Disgust: return "D";
                    case Label.Fear: return "F";
                    case Label.Happy: return "H";
                    case Label.Neutral: return "N";
                    case Label.Sad: return "S";
                    case Label.Surprise: return "U";
                }
                return "";
            }
        }

        public void Pause()
        {
            engine.Pause();
        }

        public void WinGame()
        {
            log.Stop();
            engine.WinGame();
        }

        public Visibility ShowDebugging
        {
            get
            {
                if (debugOn)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        void gameEngine_GameEnded(object sender, EventArgs e)
        {
            if (GameEnded != null)
                GameEnded(this, new EventArgs());
            log.Stop();
        }

        void gameEngine_NewEmotion(object sender, DetectClient.EmotionEventArgs e)
        {
            lastLabel = e.Emotion;
            this.OnPropertyChanged("Emotion");
            this.OnPropertyChanged("EmotionLetter");
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
            this.OnPropertyChanged("CurrentLevel");
            if (SelectedColorsChanged != null)
                SelectedColorsChanged(this, new SelectedColorChangeEventArgs() { SelectedColors = engine.SelectedColors });
        }

        void gameEngine_LevelCompleted(object sender, ChangeLevelEventArgs e)
        {
            System.Console.WriteLine("Going up one level!");
            this.OnPropertyChanged("CurrentLevel");
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
            if (!engine.NewCatch(item, basket))
                return false;

            if (ItemCaught != null)
                ItemCaught(this, new EventArgs());
            itemsCaught++;
            if (itemsDropped != 0)
                itemsDropped--;

            if (itemsCaught == 15)
            {
                itemsCaught = itemsDropped = 0;
                if (BillyHappy != null)
                    BillyHappy(this, new EventArgs());
            }

            return true;
        }

        private bool instructional = true;
        public void ItemHitGround()
        {
            if (ItemMissed != null)
                ItemMissed(this, new EventArgs());
            
            itemsDropped++;
            if (itemsCaught != 0)
                itemsCaught--;

            if (itemsDropped == 5)
            {
                itemsCaught = itemsDropped = 0;
                /*if (instructional && BillyInstructs != null)
                    BillyInstructs(this, new EventArgs());*/
                if (BillyConcerned != null)
                    BillyConcerned(this, new EventArgs());
                //instructional = !instructional;
            }

            engine.NewFall();
        }

        public int currentLevel;
        public int CurrentLevel
        {
            get
            {
                return engine.CurrentLevel;
            }
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

        public Visibility FruitTheme
        {
            get
            {
                if (ThemeRootDirectory.Equals("Fruit"))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility VegetableTheme
        {
            get
            {
                if (ThemeRootDirectory.Equals("Vegetable"))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public string ThemeRootDirectory
        {
            get
            {
                return "Vegetable";//"Fruit"
            }
        }

        public void Cleanup()
        {
            log.Stop();
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
        public event EventHandler ItemMissed;
        public event EventHandler GameEnded;
        public event EventHandler BillyConcerned;
        public event EventHandler BillyHappy;
        public event EventHandler BillyInstructs;
    }
}
