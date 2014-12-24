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
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void ShowNowPlay(object sender, NowPlay arg)
        {
            label1.Text = arg.ARTIST_NAME;
            label2.Text = arg.TRACK_SONG;
            pictureBox1.LoadAsync("http://eradio.ua/i/" + arg.PICTURE);
        }
    }
}
