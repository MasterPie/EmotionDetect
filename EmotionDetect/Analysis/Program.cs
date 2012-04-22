using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Analysis
{
    class Program
    {
        private static List<Record> data;

        static void Main(string[] args)
        {
            if(args.Length < 1){
               throw new ArgumentException("Missing input directory");
            }

            string inputDir = args[0];
            LoadFiles(inputDir);

            GenPairs();
            //AffectiveScoreImprovementTest();
            //TimeSpentInLevels();
            System.Console.WriteLine("\nAll:");
            PrintAll();
            System.Console.WriteLine("\nWinners:");
            PrintWins();
            System.Console.WriteLine("\nSingle Moods:");
            PrintSingleMood();
            System.Console.WriteLine("\nStreak deltas:");
            PrintVariabiltyInPosStreaks();
        }


        static void LoadFiles(string inDir)
        {
            if (!Directory.Exists(inDir))
            {
                throw new ArgumentException("Input directory does not exist");
            }

            string[] files = Directory.GetFiles(inDir, "*.txt", SearchOption.AllDirectories);

            data = new List<Record>();

            foreach (string file in files)
            {
                data.Add(new Record(file));
            }
        }

        private static Dictionary<String, Dictionary<ExperimentCondition, Record>> pairs;

        static void GenPairs()
        {
            pairs = new Dictionary<string, Dictionary<ExperimentCondition, Record>>();
            foreach (Record record in data)
            {
                if (!pairs.ContainsKey(record.Subject))
                {
                    pairs.Add(record.Subject, new Dictionary<ExperimentCondition, Record>());
                }

                if (!pairs[record.Subject].ContainsKey(record.Condition))
                {
                    pairs[record.Subject].Add(record.Condition, record);
                }
            }

        }

        static void AffectiveScoreImprovementTest()
        {
            System.Console.WriteLine("\n\nRunning Affective Score Improvement Test\n\n");

            foreach (string subject in pairs.Keys)
            {
                if (!pairs[subject].ContainsKey(ExperimentCondition.Affect) ||
                    !pairs[subject].ContainsKey(ExperimentCondition.Control))
                {
                    continue;
                }

                Record affectRecord = pairs[subject][ExperimentCondition.Affect];
                Record controlRecord = pairs[subject][ExperimentCondition.Control];

                double catchRatioDiff = affectRecord.CatchRatio - controlRecord.CatchRatio;

                System.Console.WriteLine("\nSubject {0}'s delta catch ratio improvement with affect: {1}",
                    subject, catchRatioDiff);
                System.Console.WriteLine("Time spent in levels");
                System.Console.WriteLine("Affect:");
                PrintTimeSpentInLevels(affectRecord);
                System.Console.WriteLine("Control:");
                PrintTimeSpentInLevels(controlRecord);
            }
        }

        static void TimeSpentInLevels()
        {
            System.Console.WriteLine("\n\nTime Spent In Each Level\n\n");

            foreach (Record record in data)
            {
                System.Console.WriteLine(record.Subject);
                System.Console.WriteLine("Level 1: {0}\nLevel 2: {1}\nLevel 3: {2}\n" +
                    "Level 4: {3}\nLevel 5: {4}", record.TimeSpentPerLevel[1], record.TimeSpentPerLevel[2],
                    record.TimeSpentPerLevel[3], record.TimeSpentPerLevel[4], record.TimeSpentPerLevel[5]);
            }
        }


        static void PrintTimeSpentInLevels(Record record)
        {
            if (record.WonGame)
            {
                System.Console.WriteLine("Level 1: {0}\nLevel 2: {1}\nLevel 3: {2}\n" +
                        "Level 4: {3}\nLevel 5: {4} WON", record.TimeSpentPerLevel[1], record.TimeSpentPerLevel[2],
                        record.TimeSpentPerLevel[3], record.TimeSpentPerLevel[4], record.TimeSpentPerLevel[5]);
            }
            else
            {
                System.Console.WriteLine("Level 1: {0}\nLevel 2: {1}\nLevel 3: {2}\n" +
                        "Level 4: {3}\nLevel 5: {4}", record.TimeSpentPerLevel[1], record.TimeSpentPerLevel[2],
                        record.TimeSpentPerLevel[3], record.TimeSpentPerLevel[4], record.TimeSpentPerLevel[5]);
            }
        }

        static void PrintVariabiltyInPosStreaks()
        {
            System.Console.WriteLine("Subject\tAge\tCond.\tCatches\tMisses\tC/T\tTime\tAvg\tAvg 2\tWon?\td1\td2\td3\td4\td5");
            foreach (IGrouping<string, Record> pair in data.GroupBy(x => x.Subject))
            {
                foreach (Record record in pair)
                {
                    StringBuilder s = new StringBuilder();
                    foreach (Int32 level in record.StreakDeltas.Keys)
                    {
                        s.Append(String.Format("\t{0}", (Math.Round((double)record.StreakDeltas[level]/(double)record.TotalStruggle,2))));
                    }
                    //s.Append(String.Format("\t{0}", record.TotalStruggle));
                    System.Console.WriteLine(record + s.ToString());
                }
            }
        }

        static void PrintAll()
        {
            PrintHeaders();
            
            foreach (IGrouping<string,Record> pair in data.GroupBy(x=>x.Subject))
            {
                foreach (Record record in pair)
                {
                    System.Console.WriteLine("{0}", record);
                }
            }
        }

        static void PrintWins()
        {
            PrintHeaders();
            foreach (Record record in data)
            {
                if (record.WonGame)
                {
                    System.Console.WriteLine("{0}", record);
                }

            }
        }

        static void PrintSingleMood()
        {
            PrintHeaders();
            foreach (Record record in data)
            {
                if (record.SingleMood)
                {
                    System.Console.WriteLine("{0}", record);
                }

            }
        }



        static void PrintHeaders()
        {
            System.Console.WriteLine("Subject\tAge\tCond.\tCatches\tMisses\tC/T\tTime\tAvg\tAvg 2\tWon?");
        }
    }
}
