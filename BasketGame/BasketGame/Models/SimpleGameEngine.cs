// -----------------------------------------------------------------------
// <copyright file="SimpleGameEngine.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Threading;
    using System.Windows.Media;
using DetectClient;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleGameEngine : IGameEngine
    {
        private ILevelManager levelManager = null;
        private ILevel currentLevel = null;
        private IItemFactory itemFactory = null;
        private DispatcherTimer gameLoopTimer = null;

        private string loggingSessionID = "";

        private const int MAX_COLLECTION = 70;
        protected const int STREAK_THRESHOLD = 12;
        protected int negativeStreak = 0;
        protected int positiveStreak = 0;
        protected int itemsCollected = 0;
        private int maxItemScore = 0;
        private int mismatches = 0;

        private double[] spawnLocations = null;
        private Color[] spawnVariety = null;
        private Color[] randomColors = null;

        protected Object scoreLock = new Object();
        private System.Random spawnRandomizer;

        public SimpleGameEngine()
        {
            gameLoopTimer = new DispatcherTimer();
            gameLoopTimer.Tick += new EventHandler(gameLoopTimer_Tick);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(1500);   
        }


        public void Initialize()
        {
            
        }

        private void Setup()
        {
            loggingSessionID = DateTime.Now.ToString("yyyy-MM-d_HHmm");

            spawnRandomizer = new Random();

            spawnLocations = new double[1];
            spawnVariety = new Color[1];
            spawnVariety[0] = Colors.Red; //set to some default value

            System.Random rand = new Random();
            Dictionary<int, Color> allColors = new Dictionary<int, Color>();
            allColors[rand.Next()] = Colors.Red;
            allColors[rand.Next()] = Colors.Green;
            allColors[rand.Next()] = Colors.Yellow;
            allColors[rand.Next()] = Colors.Orange;
            allColors[rand.Next()] = Colors.Blue;

            randomColors = new Color[allColors.Keys.Count];

            int i = 0;
            foreach (int key in allColors.Keys.OrderBy(x => x))
            {
                randomColors[i++] = allColors[key];
            }
        }

        public ILevelManager LevelManager
        {
            set { levelManager = value; }
        }

        public IItemFactory ItemFactory
        {
            set { itemFactory = value; }
        }

        private DetectClient.Client emotionClassifier = null;
        public DetectClient.Client EmotionClassifier
        {
            set
            {
                emotionClassifier = value;
                emotionClassifier.SustainedEmotionChanged += new EventHandler<EmotionEventArgs>(emotionClassifier_SustainedEmotionChanged);
            }
        }

        protected Label currentEmotion;
        protected virtual void emotionClassifier_SustainedEmotionChanged(object sender, DetectClient.EmotionEventArgs e)
        {
            currentEmotion = e.Emotion;
            System.Console.WriteLine("CURRENT EMOTION is {0}", currentEmotion);

            if (NewEmotion != null)
            {
                NewEmotion(this, e);
            }
            //logging?
            //TODO: log
        }

        public void Start()
        {
            if (levelManager == null || itemFactory == null)
                throw new ArgumentNullException("Please set an ILevelManager and an IItemFactory before starting the engine");
            Setup();
            levelManager.Reset();
            AdvanceLevel();
            if (GameStarted != null)
                GameStarted(this, new EventArgs());
        }

        public void Stop()
        {
            gameLoopTimer.Stop();
        }

        protected virtual void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            if (positiveStreak >= STREAK_THRESHOLD)
                AdvanceLevel();
            else if (negativeStreak >= STREAK_THRESHOLD)
                RegressLevel();

            SpawnItem();
        }

        protected void SpawnItem()
        {
            if (itemFactory == null)
                throw new ArgumentNullException("An IItemFactory must be created and initialized");

            double randomOffset = spawnLocations[spawnRandomizer.Next(0, spawnLocations.Length)];
            Color randomColor = spawnVariety[spawnRandomizer.Next(0, spawnVariety.Length)];
        
            IItem blah = itemFactory.Create(randomColor, currentLevel.Speed);

            if (ItemSpawned != null)
                ItemSpawned(this, new ItemSpawnEventArgs() { Item = blah, DropOffset = randomOffset, FallSpeed = currentLevel.Speed});
        }

        public bool NewCatch(IItem item, IBasket basket)
        {
            if (item.AssignedColor != basket.Color)
            {
                mismatches++;
                return false;
            }

            IncreaseScore();

            return true;
        }

        public void NewFall()
        {
            DecreaseScore();
        }

        public int CurrentScore
        {
            get { return itemsCollected; }
        }

        public int MaxScore
        {
            get { return maxItemScore; }
        }

        public Color[] SelectedColors
        {
            get
            {
                return spawnVariety;
            }
        }

        private void IncreaseScore()
        {
            lock (scoreLock)
            {
                itemsCollected++;
                positiveStreak++;

                if (negativeStreak > 0)
                    negativeStreak--;

                if (ScoreUpdated != null)
                    ScoreUpdated(this, new EventArgs());

                CheckWinGame();
            }
        }

        private void DecreaseScore()
        {
            lock (scoreLock)
            {
                if (itemsCollected > 0)
                    itemsCollected--;

                if (positiveStreak > 0)
                    positiveStreak--;

                negativeStreak++;

                if (ScoreUpdated != null)
                    ScoreUpdated(this, new EventArgs());
            }
        }

        protected void AdvanceLevel()
        {
            gameLoopTimer.Stop();

            currentLevel = levelManager.Next();
            LoadLevel(currentLevel);

            if (LevelCompleted != null)
                LevelCompleted(this, new ChangeLevelEventArgs() { SelectedLevel = currentLevel });

            gameLoopTimer.Start();
        }

        protected void RegressLevel()
        {
            gameLoopTimer.Stop();

            currentLevel = levelManager.Prev();
            LoadLevel(currentLevel);
            
            if (LevelFailed != null)
                LevelFailed(this, new ChangeLevelEventArgs() { SelectedLevel = currentLevel });
        
            gameLoopTimer.Start();
        }

        private void LoadLevel(ILevel level)
        {
            positiveStreak = negativeStreak = 0;

            spawnLocations = new double[currentLevel.LocationRandomness];
            spawnVariety = new Color[currentLevel.VarietyRandomness];

            System.Random rand = new Random();
            for (int i = 0; i < spawnLocations.Length; i++)
                spawnLocations[i] = (rand).NextDouble();

            for (int i = 0; i < spawnVariety.Length; i++)
                spawnVariety[i] = randomColors[i];

            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(-375 * currentLevel.Speed + 3375);
        }

        private void CheckWinGame()
        {
            if (itemsCollected >= MAX_COLLECTION)
            {
                gameLoopTimer.Stop();
                if (GameEnded != null)
                {
                    GameEnded(this, new EventArgs());
                }
            }
        }

        public string AssessState()
        {
            return currentEmotion + "\t" + currentLevel.ID + "\t" + CurrentScore  + "\t" + mismatches; //TODO: Figure out what to track EMOTION LEVEL SCORE MISMATCH
        }

        public void Cleanup()
        {
            gameLoopTimer.Stop();
        }


        public event EventHandler ScoreUpdated;
        public event EventHandler<ItemSpawnEventArgs> ItemSpawned;
        public event EventHandler<ChangeLevelEventArgs> LevelFailed;
        public event EventHandler<ChangeLevelEventArgs> LevelCompleted;
        public event EventHandler GameStarted;
        public event EventHandler GameEnded;
        public event EventHandler<DetectClient.EmotionEventArgs> NewEmotion;

        public void WinGame()
        {
            itemsCollected = MAX_COLLECTION;
            CheckWinGame();
        }


        public void Pause()
        {
            if (gameLoopTimer.IsEnabled)
                gameLoopTimer.Stop();
            else
                gameLoopTimer.Start();
        }


        public virtual string UniqueSessionID
        {
            get { return loggingSessionID; }
        }


        public int CurrentLevel
        {
            get { return this.currentLevel.ID; }
        }
    }
}
