using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.ComponentModel;

namespace DetectClient
{
    public class Client
    {
        TcpClient clientSocket = new TcpClient();
        BackgroundWorker clientConnectionWorker;
        BackgroundWorker clientEmotionCheck;

        public Client()
        {
            clientConnectionWorker = new BackgroundWorker();
            clientConnectionWorker.DoWork += new DoWorkEventHandler(clientWorker_DoWork);
            clientConnectionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(clientConnectionWorker_RunWorkerCompleted);

            clientEmotionCheck = new BackgroundWorker();
            clientEmotionCheck.DoWork += new DoWorkEventHandler(clientEmotionCheck_DoWork);
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
            catch (Exception)
            {

            }
        }

        void clientEmotionCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            NetworkStream serverStream = clientSocket.GetStream();

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

                if(EmotionChanged != null)
                    EmotionChanged(this, new EmotionEventArgs() { 
                        Emotion = (EmotionEventArgs.Label)Enum.Parse(typeof(EmotionEventArgs.Label), returndata,true) });
            }
        }

        public EventHandler<EmotionEventArgs> EmotionChanged;
    }
}
