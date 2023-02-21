using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using WMPLib;
using Xabe.FFmpeg;

namespace Game_Co_Caro
{
    public partial class MusicForm : Form
    {
        MediaPlayer simpleSound = new MediaPlayer();
        bool music = true;
        List<string> filteredFiles = new List<string>();
        OpenFileDialog browser = new OpenFileDialog();
        int numberOfSong = 0, currentSong = 0;
        public bool show = false;

        public MusicForm()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            LoadFolder();
            playSong(filteredFiles[currentSong]);

        }
        private void addSong()
        {
            DialogResult result = browser.ShowDialog();

            if (result == DialogResult.OK)
            {
                foreach (string file in browser.FileNames)
                    if (file.ToLower().EndsWith("wav") || file.ToLower().EndsWith("mp3") || file.ToLower().EndsWith("flac"))
                    {
                        filteredFiles.Add(file);
                        numberOfSong++;
                    }
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            DateTime dt = new DateTime();
            while (true)
            {
                if (currentSongDuration == 0) continue;
                value = Convert.ToDouble(timer.Elapsed.TotalSeconds.ToString())/ currentSongDuration;
                if (value > 1)
                {
                    value = 0;
                    //nextSong();
                }
                System.Threading.Thread.Sleep(500);
                worker.ReportProgress(Convert.ToInt32(value*100));


                if (show)
                label2.Invoke((MethodInvoker)delegate
                {
                    int min = timer.Elapsed.Minutes;
                    int sec = timer.Elapsed.Seconds;
                    string buildText = "";
                    if (min < 10) buildText += "0" + min; else buildText += min;
                    buildText += ":";
                    if (sec < 10) buildText += "0" + sec; else buildText += sec;
                    label2.Text = buildText + " / " + dur;
                });
            }
            
        }

        public void close(object sender, FormClosingEventArgs e)
        {
            show = false;
        }

        double value;
        Stopwatch timer = new Stopwatch();
        int currentSongDuration;
        string dur; 
        
        private void next(object sender, EventArgs e)
        {
            nextSong(); 
        }
        Timer MyTimer;
        private void playSong(string path)
        {
            if (MyTimer != null) MyTimer.Stop();
            MyTimer = new Timer();
            if (show)
                label1.Invoke((MethodInvoker)delegate
                {
                    label1.Text = "Playing " + Path.GetFileName(path);
                });
            Uri u = new Uri(path);
            if (backgroundWorker1.WorkerSupportsCancellation)
            {
                backgroundWorker1.CancelAsync();
            }
            timer.Stop();
            timer.Reset();
            simpleSound.Stop();
            simpleSound.Open(u);
            timer.Start();
            simpleSound.Play();
                
            WindowsMediaPlayerClass wmp = new WindowsMediaPlayerClass();
            IWMPMedia mediaInfo = wmp.newMedia(path);
            Int32.TryParse(mediaInfo.duration.ToString(), out currentSongDuration);
            dur = mediaInfo.durationString;
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
            activeTimer(currentSongDuration);
        }

        private void activeTimer(int duration)
        {
            MyTimer.Interval = ((duration + 2) * 1000);
            Console.WriteLine(MyTimer.Interval);
            MyTimer.Tick += new EventHandler(next);
            MyTimer.Start();
        }

        private bool Music
        {
            get { return music; }
            set
            {
                music = value;
                if (music)
                {
                    button1.BackgroundImage = Properties.Resources.pause;
                    button1.BackgroundImageLayout = ImageLayout.Stretch;
                    simpleSound.Play();
                    activeTimer(currentSongDuration - Convert.ToInt32(timer.Elapsed.TotalSeconds));
                    timer.Start();
                }
                else
                {
                    button1.BackgroundImage = Properties.Resources.play;
                    button1.BackgroundImageLayout = ImageLayout.Stretch;
                    simpleSound.Pause();
                    MyTimer.Stop();
                    //prevTimer += Int32.Parse(timer.Elapsed.TotalSeconds.ToString());
                    timer.Stop();
                }
            }
        }

        private void nextSong()
        {
            currentSong++;
            if (currentSong >= numberOfSong) currentSong = 0;
            playSong(filteredFiles[currentSong]);
        }
        private void prevSong()
        {
            currentSong--;
            if (currentSong < 0) currentSong = numberOfSong - 1;
            playSong(filteredFiles[currentSong]);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            nextSong();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            prevSong();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Music = !Music;
        }

        private void addsongg_Click(object sender, EventArgs e)
        {
            addSong();
        }

        private void LoadFolder()
        {

            if (filteredFiles.Count > 1)
            {
                filteredFiles.Clear();
                filteredFiles = null;
            }

            //DialogResult result = browser.ShowDialog();

            //if (result == DialogResult.OK)
            //{
            //    filteredFiles = Directory.GetFiles(browser.SelectedPath, "*.*").Where(file => file.ToLower().EndsWith("webm") || file.ToLower().EndsWith("mp4") || file.ToLower().EndsWith("wmv") || file.ToLower().EndsWith("mkv") || file.ToLower().EndsWith("avi")).ToList();
            //    
            //}

            foreach (string file in Directory.GetFiles(@"../Music/music/", "*.*"))
            {
                if (file.ToLower().EndsWith("wav") || file.ToLower().EndsWith("mp3") || file.ToLower().EndsWith("flac"))
                {
                    filteredFiles.Add(Path.GetFullPath(file));
                    numberOfSong++;
                }
            }
        }
    }

}
