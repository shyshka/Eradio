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
using Android.Support.V4.App;
using Android.Views.Animations;

namespace Eradio
{
    [Activity(Label = "Є! Rock Радіо", MainLauncher = true, Icon = "@drawable/Erock")]
    public class ActMain : Activity
    {
        private ImageButton btnPlay;
        private TextView tViewArtist;
        private TextView tViewTrack;
        private ImageView iViewTrack;
        private ListView lViewHistory;
        private ProgressDialog prDlg;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			this.SetContentView (Resource.Layout.ActMain);      

			#region Visual Elements
			this.btnPlay = FindViewById<ImageButton> (Resource.Id.btnPlay);
			this.btnPlay.Click += this.btnPlay_OnClick;

			this.tViewArtist = FindViewById<TextView> (Resource.Id.tViewArtist);
			this.tViewTrack = FindViewById<TextView> (Resource.Id.tViewTrack);
			this.iViewTrack = FindViewById<ImageView> (Resource.Id.iViewArtist);			
			this.lViewHistory = FindViewById<ListView> (Resource.Id.lViewHistoryPlay);		
			#endregion

			TabHost tabHost = FindViewById<TabHost> (Resource.Id.tabHost1);
			tabHost.Setup ();
			TabHost.TabSpec tabSpec;

			tabSpec = tabHost.NewTabSpec("tag1");
			View v1 = this.LayoutInflater.Inflate (Resource.Layout.TabHeader, null);
			(v1.FindViewById(Resource.Id.tViewHeader) as TextView).Text = "Останні 10";
			tabSpec.SetIndicator (v1);
			tabSpec.SetContent (Resource.Id.linearLayout6);
			tabHost.AddTab (tabSpec);

			tabSpec = tabHost.NewTabSpec ("tag2");
			View v2 = this.LayoutInflater.Inflate (Resource.Layout.TabHeader, null);
			(v2.FindViewById(Resource.Id.tViewHeader) as TextView).Text = "Топ 10";
			tabSpec.SetIndicator (v2);
			tabSpec.SetContent (Resource.Id.linearLayout7);
			tabHost.AddTab (tabSpec);

			tabSpec = tabHost.NewTabSpec ("tag3");
			View v3 = this.LayoutInflater.Inflate (Resource.Layout.TabHeader, null);
			(v3.FindViewById(Resource.Id.tViewHeader) as TextView).Text = "Нові пісні";
			tabSpec.SetIndicator (v3);
			tabSpec.SetContent (Resource.Id.linearLayout8);
			tabHost.AddTab (tabSpec);

			this.AcceptEvents ();
			Global.RefreshData ();
		}

		public void btnPlay_OnClick(object obj, EventArgs arg)
		{
			if (Global.IsPlay ())
				Global.StopPlay ();
			else
				Global.StartPlay ();
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 0, 0, "Налаштування");            
            menu.Add(0, 1, 1, "Закрити");

            menu.GetItem(0).SetIcon(Resource.Drawable.Settings);
            menu.GetItem(1).SetIcon(Resource.Drawable.Exit);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0:
                    Global.SendOnError("Налаштування");
                    break;
                case 1:                    
                    Global.StopPlay();
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    this.Finish();
                    break;                
            }  

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            this.ClearEvents();
            Global.IsDestoyed = true;
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {

			base.OnRestoreInstanceState (savedInstanceState);
		}

		private void OnError(object obj,string arg)
		{
			RunOnUiThread (() => Global.ShowToast (this, arg));
		}

		private void OnLoadStart(object obj,EventArgs arg)
		{
			RunOnUiThread (() => {
				prDlg = ProgressDialog.Show (this, Global.MsgTitle, Global.MsgLoading, true, false);
				prDlg.Show ();
			});
		}

		private void OnLoadEnded(object obj,EventArgs arg)
		{
			RunOnUiThread (() => {
				try {
					prDlg.Dismiss ();
				} catch {
				}
				;
			});
		}

		private void OnPlaying(object obj, EventArgs arg)
		{
			RunOnUiThread (() => {

			});      
		}

		private void OnMediaStateChanged(object obj, EventArgs arg)
		{
			RunOnUiThread (() => {
				this.btnPlay.SetImageResource (Global.IsPlay () ? 
			                              Resource.Drawable.Stop : 
			                              Resource.Drawable.Play);
			});
		}

		private void OnNowPlayChanged(object obj, NowPlay arg)
		{
			RunOnUiThread (() => {
				tViewArtist.Text = arg.ARTIST_NAME;
				tViewTrack.Text = arg.TRACK_SONG;
				Android.Graphics.Bitmap logo = WebProvider.GetImageBitmapFromUrl (arg.PICTURE);
				if (logo != null)
					iViewTrack.SetImageBitmap (logo);
				else
					iViewTrack.SetImageResource (Resource.Drawable.Erock);
				Animation anim = AnimationUtils.LoadAnimation (this, Resource.Layout.AnimCombo);
				iViewTrack.StartAnimation (anim);
			});
		}

		private void OnHistoryPlayChanged(object obj, HistoryPlayCollection arg)
		{
			RunOnUiThread (() => {
				lViewHistory.Adapter = new HistoryPlayAdapter (this, arg);
			});            
		}

		private void ClearEvents()
		{
			Global.OnError -= this.OnError;
			Global.OnLoadStart -= this.OnLoadStart;
			Global.OnLoadEnd -= this.OnLoadEnded;
			Global.OnPlaying -= this.OnPlaying;
			Global.OnMediaStateChanged -= this.OnMediaStateChanged;
			Global.OnNowPlayChanged -= this.OnNowPlayChanged;
			Global.OnHistoryPlayChanged -= this.OnHistoryPlayChanged;	
		}

		private void AcceptEvents()
		{
			Global.OnError += this.OnError;
			Global.OnLoadStart += this.OnLoadStart;
			Global.OnLoadEnd += this.OnLoadEnded;
			Global.OnPlaying += this.OnPlaying;
			Global.OnMediaStateChanged += this.OnMediaStateChanged;
			Global.OnNowPlayChanged += this.OnNowPlayChanged;
			Global.OnHistoryPlayChanged += this.OnHistoryPlayChanged;	
		}
    }
}

