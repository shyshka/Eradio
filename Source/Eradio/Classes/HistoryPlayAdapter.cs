using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace Eradio
{
    public class HistoryPlayAdapter:BaseAdapter
    {
        public HistoryPlay HistoryPlayObj;
        private Activity _activity;

        public HistoryPlayAdapter(Activity activity, HistoryPlay historyPlay)
        {
            _activity = activity;
            HistoryPlayObj = historyPlay;
        }

        public override int Count
        {
            get { return HistoryPlayObj.LstHistory.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (convertView == null || !(convertView is LinearLayout))
                view = _activity.LayoutInflater.Inflate(Resource.Layout.HistoryItem, parent, false);

            var textArtist = view.FindViewById(Resource.Id.tViewArtist) as TextView;
            var textTrack = view.FindViewById(Resource.Id.tViewTrack) as TextView;
            var textTime = view.FindViewById(Resource.Id.tViewTime) as TextView;
            var imageArtist = view.FindViewById(Resource.Id.iViewArtist) as ImageView;
			            
            var item = HistoryPlayObj.LstHistory[position];
            textArtist.Text = item.ArtistName;
            textTrack.Text = item.TrackName;
            textTime.Text = item.TimeVal;
            imageArtist.SetImageBitmap(item.ImageArtist);            

			var btnVk = view.FindViewById (Resource.Id.btnVk) as ImageButton;
			btnVk.Click += delegate {
				string vkText = string.Format ("I liked song: \n{0}\n{1}", item.ArtistName, item.TrackName);
				Log.Debug("App","Clicked item vk "+position.ToString()+ vkText.Trim('\n'));
				Toast.MakeText (_activity, vkText, ToastLength.Short).Show ();
			};

            return view;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}