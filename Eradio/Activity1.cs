using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using System.Net;
using System.IO;
using Android.Media;
using System.IO.Compression;

namespace Eradio
{
    [Activity(Label = "Є! Радіо", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        private MediaPlayer mPlayer;
        private ImageButton btnPlay;
        private ImageButton btnUa;
        private ImageButton btnRock;
        private ImageButton btnHit;
        private ImageButton btnDance;
        private TextView tViewCurRadio;

        private ProgressDialog prDlg;
        private Global.RadioKind curRadio;
        private Global.RadioKind CurRadio
        {
            get { return curRadio; }
            set
            {
                this.curRadio = value;
                this.PlayMedia();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.Main);
            this.Title = Global.MsgTitle;

            #region OnError
            Global.OnError += (obj, arg) =>
            {
                Global.SendOnLoadEnded();
                RunOnUiThread(delegate
                {
                    Toast toast = Toast.MakeText(this, (arg as Global.MessageEventArgs).Message, ToastLength.Short);
                    toast.SetGravity(GravityFlags.Bottom, 0, 0);
                    LinearLayout toastContainer = (LinearLayout)toast.View;
                    ImageView imageView = new ImageView(this);
                    imageView.SetImageResource(Resource.Drawable.Alert);
                    toastContainer.AddView(imageView, 0);
                    toast.Show();
                });
            };
            #endregion

            #region OnLoadStarted
            Global.OnLoadStart += delegate
            {
                RunOnUiThread(delegate
                {
                    prDlg = ProgressDialog.Show(this, Global.MsgTitle, Global.MsgLoading + Global.RadioList[(int)curRadio], true, false);
                    prDlg.Show();
                });
            };
            #endregion

            #region OnLoadEnded
            Global.OnLoadEnd += delegate
            {
                RunOnUiThread(delegate
                {
                    if (prDlg != null) prDlg.Dismiss();
                });
            };
            #endregion

            #region OnMediaStateChanged
            Global.OnMediaStateChanged += delegate
            {
                this.btnPlay.SetImageResource(mPlayer.IsPlaying ? Resource.Drawable.Stop : Resource.Drawable.Play);
                this.tViewCurRadio.Text = Global.RadioList[(int)curRadio];                
            };
            #endregion

            #region Buttons
            this.btnPlay = FindViewById<ImageButton>(Resource.Id.btnPlay);
            this.btnPlay.Click += btnPlay_Click;

            this.btnUa = FindViewById<ImageButton>(Resource.Id.btnUaRadio);
            this.btnUa.Click += delegate { CurRadio = Global.RadioKind.UaRadio; };

            this.btnRock = FindViewById<ImageButton>(Resource.Id.btnRockRadio);
            this.btnRock.Click += delegate { CurRadio = Global.RadioKind.RockRadio; };

            this.btnHit = FindViewById<ImageButton>(Resource.Id.btnHitRadio);
            this.btnHit.Click += delegate { CurRadio = Global.RadioKind.HitRadio; };

            this.btnDance = FindViewById<ImageButton>(Resource.Id.btnDanceRadio);
            this.btnDance.Click += delegate { CurRadio = Global.RadioKind.DanceRadio; };

            this.tViewCurRadio = FindViewById<TextView>(Resource.Id.tViewCurStation);
            #endregion

            #region MediaPlayer
            curRadio = Global.RadioKind.RockRadio;
            this.mPlayer = new MediaPlayer();
            this.mPlayer.Info += delegate { };
            this.mPlayer.Prepared += delegate
            {
                Global.SendOnLoadEnded();
                mPlayer.Start();
                Global.SendOnMediaStateChanged();
            };
            this.mPlayer.BufferingUpdate += delegate { };
            this.mPlayer.Completion += delegate { };
            this.mPlayer.Error += delegate { Global.SendOnError(Global.MsgCannotLoadStream); };
            this.mPlayer.SeekComplete += delegate { };
            #endregion

            Uri uri = new Uri("http://eradio.ua/now_play.php");
            HttpWebRequest req = new HttpWebRequest(uri);
            req.Referer = @"http://eradio.ua/rock/";
            req.Headers.Add("Accept-Encoding", "gzip, deflate");
            GZipStream gZipStream = new GZipStream(req.GetResponse().GetResponseStream(), CompressionMode.Decompress, false);
            StreamReader sRd = new StreamReader(gZipStream);
            this.Title = sRd.ReadToEnd();
        }

        void btnPlay_Click(object sender, EventArgs e)
        {
            if (this.mPlayer.IsPlaying) this.StopMedia();
            else this.PlayMedia();
        }

        private void PlayMedia()
        {
            Global.SendOnLoadStart();
            this.mPlayer.Reset();
            switch (this.curRadio)
            {
                case Global.RadioKind.UaRadio:
                    this.mPlayer.SetDataSource(@"http://live.eradio.ua/e-ua_hi");
                    break;
                case Global.RadioKind.RockRadio:
                    this.mPlayer.SetDataSource(@"http://live.eradio.ua/e-rock_hi");
                    break;
                case Global.RadioKind.HitRadio:
                    this.mPlayer.SetDataSource(@"http://live.eradio.ua/e-hit_hi");
                    break;
                case Global.RadioKind.DanceRadio:
                    this.mPlayer.SetDataSource(@"http://live.eradio.ua/e-dance_hi");
                    break;
            }
            mPlayer.PrepareAsync();
        }

        private void StopMedia()
        {
            mPlayer.Reset();
            Global.SendOnMediaStateChanged();
        }
    }
}

