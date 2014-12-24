using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace Request
{
    public partial class Form1 : Form
    {
        private BackThread _backThreadObj = new BackThread();

        public Form1()
        {
            InitializeComponent();
            _backThreadObj.StartThread();
            _backThreadObj.NowPlayChanged += (obj, arg) =>
            {
                if (this.InvokeRequired) this.BeginInvoke(new BackThread.NowPlayEventHandler(ShowNowPlay), obj, arg);
                else ShowNowPlay(obj, arg);
            };
            _backThreadObj.HistoryPlayChanged += (obj, arg) =>
            {
                if (this.InvokeRequired) this.BeginInvoke(new BackThread.HistoryPlayHandler(ShowHistoryPlay), obj, arg);
                else ShowHistoryPlay(obj, arg);
            };
            _backThreadObj.TopTenChanged += (obj, arg) =>
            {
                if (this.InvokeRequired) this.BeginInvoke(new BackThread.TopTenEventHandler(ShowTopTen), obj, arg);
                else ShowTopTen(obj, arg);
            };
        }

        private void ShowNowPlay(object sender, NowPlay arg)
        {
            label1.Text = "АРТИСТ:\n" + arg.ARTIST_NAME;
            label2.Text = "ПІСНЯ:\n" + arg.TRACK_SONG;
            pictureBox1.Image = WebContent.LoadPicture("http://eradio.ua/i/" + arg.PICTURE);
        }

        private void ShowHistoryPlay(object sender, HistoryPlay arg)
        {
            while (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.RemoveAt(0);
            foreach (var item in arg.LstHistory)
            {
                dataGridView1.Rows.Add(
                    item.ArtistName,
                    item.TrackName,
                    item.TimeVal,
                    item.ImageArtist);
            }
        }

        private void ShowTopTen(object sender, TopTen arg)
        {
            while (dataGridView2.Rows.Count > 0)
                dataGridView2.Rows.RemoveAt(0);
            foreach (var item in arg.LstHistory)
            {
                dataGridView2.Rows.Add(
                    item.ArtistName,
                    item.TrackName,
                    item.SrcFilePath,
                    item.ImageArtist);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _backThreadObj.StopThread();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri("http://eradio.ua/mp3/The Pretty Reckless-Messed Up World.mp3");

            WebClient wc = new WebClient();
            wc.DownloadFileAsync(uri, @"c:\1.mp3");
        }
    }
}
