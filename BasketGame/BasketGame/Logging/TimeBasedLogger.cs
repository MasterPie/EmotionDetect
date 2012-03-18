// -----------------------------------------------------------------------
// <copyright file="TimeBasedLogger.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TimeBasedLogger : ILogger
    {
        private Timer logWriteTimer;
        private StreamWriter fileWriter;
        private ILoggable provider;

        private const string LOG_DIRECTORY = "Log";
        
        public TimeBasedLogger()
        {
            logWriteTimer = new Timer();
            logWriteTimer.Interval = 1000;
            logWriteTimer.Elapsed += new ElapsedEventHandler(logWriteTimer_Elapsed);
        }

        public void Start(ILoggable observable)
        {
            provider = observable;
            string file_name = AppDomain.CurrentDomain.BaseDirectory + "\\" + LOG_DIRECTORY + "\\" + 
                observable.UniqueSessionID + "\\" + DateTime.Now.ToString("yyyy-MM-d_HHmmss") + ".txt";

            if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + LOG_DIRECTORY))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\" + LOG_DIRECTORY);
            }

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + LOG_DIRECTORY + "\\" +
                observable.UniqueSessionID))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\" + LOG_DIRECTORY + "\\" +
                observable.UniqueSessionID);
            }

            fileWriter = new StreamWriter(file_name,true);
            logWriteTimer.Start();
        }

        void logWriteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Write(provider.AssessState());
        }

        private void Write(string message)
        {
            string entry_time = DateTime.Now.ToString("HH:mm:ss");
            fileWriter.WriteLine(entry_time + "\t" + message);
            fileWriter.Flush();
        }

        public void Stop()
        {
            if (logWriteTimer.Enabled)
            {
                logWriteTimer.Stop();
                fileWriter.WriteLine();
                fileWriter.Close();
            }
        }
    }
}
