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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SimpleGameEngine : IGameEngine
    {
        private ILevelManager levelManager = null;
        private ILevel currentLevel = null;
        private IItemFactory itemFactory = null;
        private DispatcherTimer gameLoopTimer = null;

        private const int STREAK_THRESHOLD = 15;
        private int negativeStreak = 0;
        private int positiveStreak = 0;
        private int itemsCollected = 0;
        private int maxItemScore = 0;

        private double[] spawnLocations = null;
        private Color[] spawnVariety = null;
        private Color[] randomColors = null;

        private Object scoreLock = new Object();
        private System.Random spawnRandomizer;

        public SimpleGameEngine()
        {
            spawnRandomizer = new Random();

            gameLoopTimer = new DispatcherTimer();
            gameLoopTimer.Tick += new EventHandler(gameLoopTimer_Tick);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(1500);

            spawnLocations = new double[1];
            spawnVariety = new Color[1];
            spawnVariety[0] = Colors.Red; //set to some default value

            System.Random rand = new Random();
            Dictionary<int,Color> allColors = new Dictionary<int,Color>();
            allColors[rand.Next()] = Colors.Red;
            allColors[rand.Next()] = Colors.Green;
            allColors[rand.Next()] = Colors.Yellow;
            allColors[rand.Next()] = Colors.Orange;
            allColors[rand.Next()] = Colors.Blue;

            randomColors = new Color[allColors.Keys.Count];

            int i = 0;
            foreach (int key in allColors.Keys.OrderBy(x=>x))
            {
                randomColors[i++] = allColors[key];
            }

        }


        public void Initialize()
        {
            
        }

        public ILevelManager LevelManager
        {
            set { levelManager = value; }
        }

        public IItemFactory ItemFactory
        {
            set { itemFactory = value; }
        }

        public void Start()
        {
            if (levelManager == null || itemFactory == null)
                throw new ArgumentNullException("Please set an ILevelManager and an IItemFactory before starting the engine");

            levelManager.Reset();
            AdvanceLevel();
            if (GameStarted != null)
                GameStarted(this, new EventArgs());
        }

        public void Stop()
        {
            gameLoopTimer.Stop();
        }

        void gameLoopTimer_Tick(object sender, EventArgs e)
        {
            if (positiveStreak >= STREAK_THRESHOLD)
                AdvanceLevel();
            else if (negativeStreak >= STREAK_THRESHOLD)
                RegressLevel();

            SpawnItem();
        }

        private void SpawnItem()
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
                return false;

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

        private void AdvanceLevel()
        {
            gameLoopTimer.Stop();

            currentLevel = levelManager.Next();
            LoadLevel(currentLevel);

            if (LevelCompleted != null)
                LevelCompleted(this, new ChangeLevelEventArgs() { SelectedLevel = currentLevel });

            gameLoopTimer.Start();
        }

        private void RegressLevel()
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

        }

        public string AssessState()
        {
            return "";
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
    }
}
