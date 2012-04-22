// -----------------------------------------------------------------------
// <copyright file="Record.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Record
    {
        private string subject;
        private ExperimentCondition condition;
        private string[] lines;

        private bool oldLogs = false;

        private double totalTimeSpent;
        private int totalCatches;
        private int totalMisses;
        private int totalItemSpawn;
        private int totalLevelDrops;
        private bool wonGame;
        private int secondsToWin;

        private Dictionary<Int32, Double> timeSpentInLevels;
        private Dictionary<String, Double> timeSpentInEmotion;
        private Dictionary<Int32, Int32> streakDeltaFreqInLevels;


        public Record(string file)
        {
            if (!File.Exists(file))
            {
                throw new ArgumentException(file + " does not exist.");
            }

            timeSpentInLevels = new Dictionary<Int32, Double>();
            timeSpentInLevels.Add(1, 0); timeSpentInLevels.Add(2, 0);
            timeSpentInLevels.Add(3, 0); timeSpentInLevels.Add(4, 0);
            timeSpentInLevels.Add(5, 0);

            streakDeltaFreqInLevels = new Dictionary<Int32, Int32>();
            streakDeltaFreqInLevels.Add(1, 0); streakDeltaFreqInLevels.Add(2, 0);
            streakDeltaFreqInLevels.Add(3, 0); streakDeltaFreqInLevels.Add(4, 0);
            streakDeltaFreqInLevels.Add(5, 0);

            timeSpentInEmotion = new Dictionary<string, double>();

            //System.Console.WriteLine("\nReading log: {0}", file);
            ParseExperimentType(file);
            ParseData(file);
        }

        public string Subject
        {
            get { return this.subject; }
        }

        public bool FiveYearOld
        {
            get { return Int32.Parse(this.subject.TrimStart('S')) <= 11; }
        }

        public ExperimentCondition Condition
        {
            get { return this.condition; }
        }

        public double TotalTimeSpent
        {
            get { return this.totalTimeSpent; }
        }

        public double CatchRatio
        {
            get { return (double)this.totalCatches / (double)totalItemSpawn; }
        }

        public double MissRatio
        {
            get { return (double)this.totalMisses / (double)totalItemSpawn; }
        }

        public Dictionary<Int32, Double> TimeSpentPerLevel
        {
            get { return this.timeSpentInLevels; }
        }

        public Dictionary<Int32, Int32> StreakDeltas
        {
            get { return this.streakDeltaFreqInLevels; }
        }

        public int TotalStruggle
        {
            get { return this.streakDeltaFreqInLevels.Sum(x => x.Value); }
        }

        public int LevelDrops
        {
            get { return this.totalLevelDrops; }
        }

        public bool WonGame
        {
            get { return this.wonGame; }
        }

        public bool OldLogStyle
        {
            get { return this.oldLogs; }
        }

        public string MostCommonEmotion
        {
            get
            {
                return this.timeSpentInEmotion.Keys.OrderByDescending(x => timeSpentInEmotion[x]).First();
            }
        }

        public string SecondMostCommonEmotion
        {
            get
            {
                return this.timeSpentInEmotion.Keys.OrderByDescending(x => timeSpentInEmotion[x]).ElementAtOrDefault(1) ?? MostCommonEmotion;
            }
        }

        public string AverageEmotion
        {
            get
            {
                Dictionary<String, double> percentages = new Dictionary<string, double>();
                foreach (String key in timeSpentInEmotion.Keys)
                {
                    if (!percentages.ContainsKey(key))
                    {
                        percentages.Add(key, 0.0);
                    }
                    percentages[key] += (1.0*timeSpentInEmotion[key]) / this.totalTimeSpent;
                }

                return percentages.Keys.OrderByDescending(k => percentages[k]).First();
            }
        }

        public bool SingleMood
        {
            get { return MostCommonEmotion.Equals(SecondMostCommonEmotion); }
        }

        private void ParseExperimentType(string file)
        {
            string filePart = file.Split('\\').Last();
            string[] codeParts = filePart.Split('_');
            this.subject = codeParts[0];
            char conditionLetter = codeParts[1][0];

            if (conditionLetter == 'C')
            {
                condition = ExperimentCondition.Control;
            }
            else if (conditionLetter == 'A')
            {
                condition = ExperimentCondition.Affect;
            }
            else
            {
                throw new ArgumentException("Unidentified experiment condition " + conditionLetter);
            }
        }

        private void ParseData(string file)
        {
            lines = File.ReadAllLines(file);

            DateTime time = new DateTime();
            string emotion = "";
            int level = 1, score = 0, positiveStreak = 0, negativeStreak = 0,
                mismatches, totalCatches = -1, totalMisses = -1;

            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line))
                {
                    UpdateModelLastLine(time, level, emotion, score, positiveStreak, negativeStreak,
                        totalCatches, totalMisses, oldLogs);
                    continue;
                }


                string[] fields = line.Split('\t');
                time = DateTime.Parse(fields[0]);
                emotion = fields[1];
                level = Int32.Parse(fields[2]);
                score = Int32.Parse(fields[3]);
                positiveStreak = Int32.Parse(fields[4]);
                negativeStreak = Int32.Parse(fields[5]);
                
                if (fields.Length == 7) //first log headers with mismatch
                {
                    mismatches = Int32.Parse(fields[6]);
                    oldLogs = true;
                }
                else if (fields.Length == 8)
                {
                    totalCatches = Int32.Parse(fields[6]);
                    totalMisses = Int32.Parse(fields[7]);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Too many fields");
                }
                UpdateModel(time, level, emotion, score, positiveStreak, negativeStreak,
                    totalCatches, totalMisses, oldLogs);
            }
            totalItemSpawn = this.totalCatches + this.totalMisses;
        }

        private int lastPosStreak = 0,lastNegStreak = 0;
        private int lastLevel = 0;
        private DateTime lastDateTime;
        private bool firstEntry = true;
        private bool posStreakIncreasing = false;
        private void UpdateModel(DateTime time, int level, string emotion, int score, 
            int positiveStreak, int negativeStreak, int totalCatches, int totalMisses, bool oldLog)
        {
            if ((positiveStreak - lastPosStreak > 0))
            {
                this.totalCatches += (positiveStreak - lastPosStreak);
            }
            else if (negativeStreak - lastNegStreak > 0)
            {
                this.totalMisses += (negativeStreak - lastNegStreak);
            }

            if (positiveStreak < lastPosStreak && posStreakIncreasing && lastLevel >= level)
            {
                streakDeltaFreqInLevels[level]++;
            }

            if (level < lastLevel)
            {
                totalLevelDrops++;
            }

            if (!firstEntry && !lastDateTime.Equals(time))
            {
                timeSpentInLevels[level] += (time - lastDateTime).TotalSeconds;

                if (!timeSpentInEmotion.ContainsKey(emotion))
                {
                    timeSpentInEmotion.Add(emotion, 0);
                }

                timeSpentInEmotion[emotion] += (time - lastDateTime).TotalSeconds;
                totalTimeSpent += (time - lastDateTime).TotalSeconds;
            }

            lastLevel = level;
            lastDateTime = time;
            lastPosStreak = positiveStreak;
            lastNegStreak = negativeStreak;
            posStreakIncreasing = positiveStreak >= lastPosStreak;
            firstEntry = false;
        }

        private void UpdateModelLastLine(DateTime time, int level, string emotion, int score,
            int positiveStreak, int negativeStreak, int totalCatches, int totalMisses, bool oldLog)
        {
            if (score >= 49 && level == 5)
            {
                this.wonGame = true;
            }
        }

        private string AgeString()
        {
            return FiveYearOld ? "5yrs" : "4yrs";
        }

        public override string ToString()
        {
            string recordString = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}",
                this.subject, this.AgeString(), condition.ToString(), this.totalCatches, this.totalMisses, 
                Math.Round(this.CatchRatio,2),Math.Round(this.totalTimeSpent / 60.0, 1),
                this.AverageEmotion.PadRight(7).Substring(0, 7), this.SecondMostCommonEmotion.PadRight(7).Substring(0, 7), 
                this.wonGame ? "WON" : "");

            return recordString;
        }
    }
}
