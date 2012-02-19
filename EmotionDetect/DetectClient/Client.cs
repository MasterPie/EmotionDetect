using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.ComponentModel;
using System.Timers;

namespace DetectClient
{
    public class Client
    {
        TcpClient clientSocket = new TcpClient();
        BackgroundWorker clientConnectionWorker;
        BackgroundWorker clientEmotionCheck;

        private Timer retryTimer = null;
        private Label currentEmotionExpressed;
        private Object emotionKey = new Object();
        private bool firstRun = true;

        List<Label> window = null;
        Dictionary<Label, double> windowPercentages = null;

        public Client()
        {
            window = new List<Label>();
            windowPercentages = new Dictionary<Label, double>();
            foreach (Label label in (Label[])Enum.GetValues(typeof(Label)))
            {
                windowPercentages.Add(label, 0);
            }

            retryTimer = new Timer(2000);
            retryTimer.Elapsed += new ElapsedEventHandler(retryTimer_Elapsed);

            clientConnectionWorker = new BackgroundWorker();
            clientConnectionWorker.WorkerSupportsCancellation = true;
            clientConnectionWorker.DoWork += new DoWorkEventHandler(clientWorker_DoWork);
            clientConnectionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(clientConnectionWorker_RunWorkerCompleted);

            clientEmotionCheck = new BackgroundWorker();
            clientEmotionCheck.DoWork += new DoWorkEventHandler(clientEmotionCheck_DoWork);

            Init();
        }

        

        void retryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            retryTimer.Stop();
            Init();
        }

        void clientConnectionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            clientEmotionCheck.RunWorkerAsync();
        }

        private void Init()
        {
            clientConnectionWorker.RunWorkerAsync();
        }

        void clientWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                clientSocket.Connect("127.0.0.1", 55555);
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("GET LABEL");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
            catch (SocketException)
            {
                clientConnectionWorker.CancelAsync();
            }
        }

        private Label lastEmotion;
        void clientEmotionCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            NetworkStream serverStream = null;
            try
            {
                serverStream = clientSocket.GetStream();
            }
            catch (InvalidOperationException ex)
            {
                retryTimer.Start();
                return;
            }

            while (true)
            {
                byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                byte[] trimmedWord;
                int i = inStream.Length - 1;
                while (inStream[i] == 0) --i;
                trimmedWord = new byte[i + 1];
                Array.Copy(inStream, trimmedWord, i+1);

                string returndata = System.Text.Encoding.ASCII.GetString(trimmedWord);

                try
                {
                    currentEmotionExpressed = (Label)Enum.Parse(typeof(Label), returndata, true);
                }
                catch (Exception f)
                {
                    continue;
                }

                if (currentEmotionExpressed != lastEmotion || firstRun)
                {
                    firstRun = false;

                    if(EmotionChanged != null){
                        EmotionChanged(this, new EmotionEventArgs()
                        {
                            Emotion = currentEmotionExpressed
                        });
                    }
                }
                lastEmotion = currentEmotionExpressed;

                CheckSustained(lastEmotion);
            }
        }

        private const double PERCENT_THRESHOLD = 0.3;
        private Label sustainedEmotion;
        private void CheckSustained(Label currentEmotion)
        {
            if (window.Count == 10)
                window.RemoveAt(0);

            //TODO: check for latency

            window.Add(currentEmotion);

            List<Label> keys = windowPercentages.Keys.ToList<Label>();
            for (int i = 0; i < keys.Count; i++)
            {
                windowPercentages[keys[i]] = 0.0;
            }

            foreach (Label label in window)
            {
                windowPercentages[label] += (1.0 / 10);
            }

            List<KeyValuePair<Label,double>> sortedPercentages = windowPercentages.ToList<KeyValuePair<Label, double>>();
            sortedPercentages.Sort((x, y) => y.Value.CompareTo(x.Value));

            if ((sortedPercentages[0].Value - sortedPercentages[1].Value) >= PERCENT_THRESHOLD && sustainedEmotion != sortedPercentages[0].Key)
            {
                sustainedEmotion = sortedPercentages[0].Key;

                if (SustainedEmotionChanged != null)
                    SustainedEmotionChanged(this, new EmotionEventArgs() { Emotion = sustainedEmotion });
            }
        }

        public EventHandler<EmotionEventArgs> EmotionChanged;
        public EventHandler<EmotionEventArgs> SustainedEmotionChanged;
    }
}
