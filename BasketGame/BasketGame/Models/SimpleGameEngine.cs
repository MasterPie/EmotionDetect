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

        private int[] spawnLocations = null;
        private Color[] spawnVariety = null;

        private Object scoreLock = new Object();
        private System.Random spawnRandomizer;

        public SimpleGameEngine()
        {
            spawnRandomizer = new Random();

            gameLoopTimer = new DispatcherTimer();
            gameLoopTimer.Tick += new EventHandler(gameLoopTimer_Tick);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(1500);

            spawnLocations = new int[1];
            spawnVariety = new Color[1];
            spawnVariety[0] = Colors.Red; //set to some default value
        }


        public void Initialize()
        {
            
        }

        public ILevelManager LevelManager
        {
            set { levelManager = value; }
        }

        public void Start()
        {
            if (levelManager == null || itemFactory == null)
                throw new ArgumentNullException("Please set an ILevelManager and an IItemFactory before starting the engine");

            levelManager.Reset();
            AdvanceLevel();
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

            int randomOffset = spawnLocations[spawnRandomizer.Next(0, spawnLocations.Length)];
            Color randomColor = spawnVariety[spawnRandomizer.Next(0, spawnVariety.Length)];
        
            IItem blah = itemFactory.Create(randomColor);

            if (ItemSpawned != null)
                ItemSpawned(this, new ItemSpawnEventArgs() { Item = blah, DropOffset = randomOffset, FallSpeed = currentLevel.Speed});
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

        public bool NewCatch(IItem item, IBasket basket)
        {
            if (item.AssignedColor != basket.Color)
                return false;

            IncreaseScore();

            return true;
        }

        public int CurrentScore
        {
            get { return itemsCollected; }
        }

        public int MaxScore
        {
            get { return maxItemScore; }
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

        private void AdvanceLevel()
        {
            gameLoopTimer.Stop();
            
            currentLevel = levelManager.Next();
            LoadLevel(currentLevel);

            if (LevelCompleted != null)
                LevelCompleted(this, new ChangeLevelEventArgs() { SelectedLevel = currentLevel });
            
            gameLoopTimer.Start();
        }

        private void LoadLevel(ILevel level)
        {
            positiveStreak = negativeStreak = 0;

            spawnLocations = new int[currentLevel.LocationRandomness];
            spawnVariety = new Color[currentLevel.VarietyRandomness];

            System.Random rand = new Random();
            for (int i = 0; i < spawnLocations.Length; i++)
                spawnLocations[i] = (rand).Next(0,800);

            
            Color[] allColors = new Color[]{Colors.Red, Colors.Green, Colors.Yellow, Colors.Orange, Colors.Blue}; //TODO: this should be set dynamically

            rand = new Random();
            for (int i = 0; i < spawnVariety.Length; i++)
                spawnVariety[i] = allColors[(rand).Next(0, allColors.Length)];

        }

        public event EventHandler ScoreUpdated;
        public event EventHandler<ItemSpawnEventArgs> ItemSpawned;
        public event EventHandler<ChangeLevelEventArgs> LevelFailed;
        public event EventHandler<ChangeLevelEventArgs> LevelCompleted;

        public string AssessState()
        {
            return "";
        }

        public void Cleanup()
        {
            gameLoopTimer.Stop();
        }


        public IItemFactory ItemFactory
        {
            set { itemFactory = value; }
        }


        public void NewFall()
        {
            IncreaseScore();
            //DecreaseScore();
        }
    }
}
