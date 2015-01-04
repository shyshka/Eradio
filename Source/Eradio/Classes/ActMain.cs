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
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.ActMain);      
			this.AcceptEvents ();

			#region Visual Elements
			this.btnPlay = FindViewById<ImageButton>(Resource.Id.btnPlay);
			this.btnPlay.Click += delegate
			{
				if (Global.IsPlay()) Global.StopPlay();
				else Global.StartPlay();
			};
			this.btnPlay.SetImageResource(Global.IsPlay() ?
			                              Resource.Drawable.Stop :
			                              Resource.Drawable.Play);
			
			this.tViewArtist = FindViewById<TextView>(Resource.Id.tViewArtist);
			this.tViewTrack = FindViewById<TextView>(Resource.Id.tViewTrack);
			this.iViewTrack = FindViewById<ImageView>(Resource.Id.iViewArtist);
			
			this.lViewHistory = FindViewById<ListView>(Resource.Id.lViewHistoryPlay);
			lViewHistory.ItemClick+=(obj,arg)=>
			{
				Toast.MakeText(this,arg.Position.ToString(),ToastLength.Short).Show();
			};
			#endregion
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
                    Global.SendOnError("hello");
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
            //Global.ClearEvents();
            Global.IsDestoyed = true;
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
			Global.RefreshData ();
			base.OnRestoreInstanceState (savedInstanceState);
		}

		private void AcceptEvents()
		{
			#region OnError
			Global.OnError += (obj, arg) => RunOnUiThread(delegate
			                                              {
				Toast toast = Toast.MakeText(this, arg, ToastLength.Short);
				toast.SetGravity(GravityFlags.Bottom, 0, 0);                    
				LinearLayout toastContainer = (LinearLayout)toast.View;
				ImageView imageView = new ImageView(this);
				imageView.SetImageResource(Resource.Drawable.Alert);
				toastContainer.AddView(imageView, 0);                    
				toast.Show();
			});
			#endregion
			
			#region OnLoadStarted
			Global.OnLoadStart += delegate
			{
				RunOnUiThread(delegate
				              {
					prDlg = ProgressDialog.Show(this, Global.MsgTitle, Global.MsgLoading, true, false);
					prDlg.Show();
				});
			};
			#endregion
			
			#region OnLoadEnded
			Global.OnLoadEnd += (obj, arg) => RunOnUiThread(() =>
			                                                {
				if (prDlg != null) prDlg.Dismiss();
			});
			
			#endregion
			
			#region OnPlaying
			Global.OnPlaying += (obj, arg) => RunOnUiThread(() =>
			                                                {
				this.Title = string.Format("{0} {1}", Global.MsgTitle, Global.MsgLoading);
				
				//// These are the values that we want to pass to the next activity
				//Bundle valuesForActivity = new Bundle();
				
				
				//// Create the PendingIntent with the back stack             
				//// When the user clicks the notification, SecondActivity will start up.
				//Intent resultIntent = new Intent(this, typeof(ActMain));
				//resultIntent.PutExtras(valuesForActivity); // Pass some values to SecondActivity.
				
				//TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
				////stackBuilder.AddParentStack(ActMain);
				//stackBuilder.AddNextIntent(resultIntent);
				
				//PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
				
				//// Build the notification
				//NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
				//    .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
				//    .SetContentIntent(resultPendingIntent) // start up this activity when the user clicks the intent.
				//    .SetContentTitle("Є! Rock Радіо") // Set the title
				//    //.SetNumber(_count) // Display the count in the Content Info
				//    .SetSmallIcon(Resource.Drawable.Erock) // This is the icon to display
				//    .SetContentText("Ви слухаєте Є! Rock Радіо"); // the message to display.
				
				//// Finally publish the notification
				//NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
				//notificationManager.Notify(14588, builder.Build());
			});       
			#endregion
			
			#region OnMediaStateChanged
			Global.OnMediaStateChanged += delegate
			{
				this.btnPlay.SetImageResource(Global.IsPlay()? 
				                              Resource.Drawable.Stop : 
				                              Resource.Drawable.Play);
			};
			#endregion	

			
			#region Thread
			Global.OnNowPlayChanged += (obj, arg) =>
				this.RunOnUiThread(delegate {
					tViewArtist.Text = "ARTIST: " + arg.ARTIST_NAME;
					tViewTrack.Text = "TRACK: " + arg.TRACK_SONG;
					Android.Graphics.Bitmap logo = WebProvider.GetImageBitmapFromUrl(arg.PICTURE);
					if (logo != null) iViewTrack.SetImageBitmap(logo);
					else iViewTrack.SetImageResource(Resource.Drawable.Erock);
					Animation anim = AnimationUtils.LoadAnimation(this, Resource.Layout.AnimCombo);
					iViewTrack.StartAnimation(anim);
				});
			
			Global.OnHistoryPlayChanged += (obj, arg) =>
				this.RunOnUiThread(() => {
					lViewHistory.Adapter = new HistoryPlayAdapter(this, arg);
				});            
			#endregion
		}
    }
}

