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

namespace Eradio
{
    [Activity(Label = "Є! Rock Радіо", MainLauncher = true, Icon = "@drawable/icon")]
    public class ActMain : Activity
    {
        private ImageButton btnPlay;
        private Button btnSleep;
        private TextView tViewArtist;
        private TextView tViewTrack;
        private ImageView iViewTrack;
        private ListView lViewHistory;
        private ProgressDialog prDlg;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.ActMain);

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


                // These are the values that we want to pass to the next activity
                Bundle valuesForActivity = new Bundle();
                

                // Create the PendingIntent with the back stack             
                // When the user clicks the notification, SecondActivity will start up.
                Intent resultIntent = new Intent(this, typeof(ActMain));
                resultIntent.PutExtras(valuesForActivity); // Pass some values to SecondActivity.

                TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
                //stackBuilder.AddParentStack(ActMain);
                stackBuilder.AddNextIntent(resultIntent);

                PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

                // Build the notification
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                    .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
                    .SetContentIntent(resultPendingIntent) // start up this activity when the user clicks the intent.
                    .SetContentTitle("Є! Rock Радіо") // Set the title
                    //.SetNumber(_count) // Display the count in the Content Info
                    .SetSmallIcon(Resource.Drawable.Erock) // This is the icon to display
                    .SetContentText("Ви слухаєте Є! Rock Радіо"); // the message to display.

                // Finally publish the notification
                NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                notificationManager.Notify(14588, builder.Build());
            });       
            #endregion

            #region OnMediaStateChanged
            Global.OnMediaStateChanged += delegate
            {
                this.btnPlay.SetImageResource(Global.MediaProviderObj.MPlayerObj.IsPlaying ? Resource.Drawable.Stop : Resource.Drawable.Play);
            };
            #endregion

            #region Visual Elements
            this.btnPlay = FindViewById<ImageButton>(Resource.Id.btnPlay);
            this.btnPlay.Click += btnPlay_Click;
            this.tViewArtist = FindViewById<TextView>(Resource.Id.tViewArtist);
            this.tViewTrack = FindViewById<TextView>(Resource.Id.tViewTrack);
            this.iViewTrack = FindViewById<ImageView>(Resource.Id.iViewArtist);

            this.lViewHistory = FindViewById<ListView>(Resource.Id.lViewHistoryPlay);
            #endregion

            #region Thread
            BackThread thread = new BackThread();
            thread.NowPlayChanged += (obj, arg) =>
                this.RunOnUiThread(() =>
                {
                    tViewArtist.Text = "ARTIST: " + arg.ARTIST_NAME;
                    tViewTrack.Text = "TRACK: " + arg.TRACK_SONG;
                    iViewTrack.SetImageBitmap(WebProvider.GetImageBitmapFromUrl(arg.PICTURE));
                });

            thread.HistoryPlayChanged += (obj, arg) =>
                this.RunOnUiThread(() =>
                    {
                        lViewHistory.Adapter = new HistoryPlayAdapter(this, arg);
                    });
            thread.StartThread();
            #endregion
        }

        void btnPlay_Click(object sender, EventArgs e)
        {
            if (Global.MediaProviderObj.MPlayerObj.IsPlaying) this.StopMedia();
            else this.PlayMedia();
        }

        private void PlayMedia()
        {
            Global.SendOnLoadStart();
            Global.MediaProviderObj.MPlayerObj.Reset();
            Global.MediaProviderObj.MPlayerObj.SetDataSource(Global.MediaStreamPath);
            Global.MediaProviderObj.MPlayerObj.PrepareAsync();
        }

        private void StopMedia()
        {
            Global.MediaProviderObj.MPlayerObj.Reset();
            Global.SendOnMediaStateChanged();
        }
    }
}

