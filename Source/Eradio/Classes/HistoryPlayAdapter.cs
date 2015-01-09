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
        public HistoryPlayCollection HistoryPlayObj;
        private Activity _activity;

        public HistoryPlayAdapter(Activity activity, HistoryPlayCollection historyPlay)
        {
            _activity = activity;
            HistoryPlayObj = historyPlay;
        }

        public override int Count
        {
            get { return HistoryPlayObj.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (convertView == null || !(convertView is LinearLayout))
				view = _activity.LayoutInflater.Inflate (Resource.Layout.HistoryItem, parent, false);

			var textArtist = view.FindViewById (Resource.Id.tViewArtist) as TextView;
			var textTrack = view.FindViewById (Resource.Id.tViewTrack) as TextView;
			var textTime = view.FindViewById (Resource.Id.tViewTime) as TextView;
			var imageArtist = view.FindViewById (Resource.Id.iViewArtist) as ImageView;
			            
			var item = HistoryPlayObj[position];
			textArtist.Text = item.ArtistName;
			textTrack.Text = item.TrackName;
			textTime.Text = item.TimeVal;
			if (item.ImageArtist != null)
				imageArtist.SetImageBitmap (item.ImageArtist);
			else
				imageArtist.SetImageResource (Resource.Drawable.Erock);

			var btnVk = view.FindViewById (Resource.Id.btnVk) as ImageButton;
			btnVk.Tag = position;
			btnVk.Click -= SetVk;
			btnVk.Click += this.SetVk;				
			return view;
		}

		private void SetVk(object obj,EventArgs arg)
		{
			var item = HistoryPlayObj[int.Parse((obj as ImageView).Tag.ToString())];
			string vkText = string.Format ("I liked song: \n{0}\n{1}", item.ArtistName, item.TrackName);
			Toast.MakeText (_activity, vkText, ToastLength.Short).Show ();

			string phpRequest = string.Format (
				                    "http://vkontakte.ru/share.php?" +
				                    "url=http://eradio.ua/rock/" +
				                    "&title=Є! Rock Radio" +
				                    "&description=Мені сподобалась пісня {0} - {1} на http://eradio.ua/rock/" +
				                    "&image={2}" +
				                    "&noparse=true", item.ArtistName, item.TrackName, item.ImagePath);

			Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(phpRequest));
			_activity.StartActivity(browserIntent);
		}

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}