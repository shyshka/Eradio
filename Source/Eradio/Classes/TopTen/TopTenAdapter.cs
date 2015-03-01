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
    public class TopTenAdapter:BaseAdapter
    {
        private TopTenCollection _topTenObj;
        private Activity _activity;

		public TopTenAdapter(Activity activity, TopTenCollection topTen)
        {
            _activity = activity;
			_topTenObj = topTen;
        }

        public override int Count
        {
			get { return _topTenObj.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

		public override long GetItemId(int position)
		{
			return position;
		}

        public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (convertView == null || !(convertView is LinearLayout))
				view = _activity.LayoutInflater.Inflate (Resource.Layout.TopTenItem, parent, false);

			var textArtist = view.FindViewById (Resource.Id.tViewArtist) as TextView;
			var textTrack = view.FindViewById (Resource.Id.tViewTrack) as TextView;
			var imageArtist = view.FindViewById (Resource.Id.iViewArtist) as ImageView;
			            
			var item = _topTenObj[position];
			textArtist.Text = item.ArtistName;
			textTrack.Text = item.TrackName;
			if (item.ImageBitmap != null)
				imageArtist.SetImageBitmap (item.ImageBitmap);
			else
				imageArtist.SetImageResource (Resource.Drawable.Erock);

			var btnVk = view.FindViewById (Resource.Id.btnVk) as ImageView;
			btnVk.Tag = position;
			btnVk.Click -= SetVk;
			btnVk.Click += this.SetVk;				
			return view;
		}

		public TopTenItem this [int pos] {
			get {
				return this._topTenObj [pos];
			}
		}

		private void SetVk(object obj,EventArgs arg)
		{
			var item = _topTenObj[int.Parse((obj as ImageView).Tag.ToString())];
			string vkText = string.Format ("I liked song: \n{0}\n{1}", item.ArtistName, item.TrackName);
			string phpRequest = string.Format (
				                    "http://vkontakte.ru/share.php?" +
				                    "url=http://eradio.ua/rock/" +
				                    "&title=Є! Rock Radio" +
				                    "&description=Мені сподобалась пісня {0} - {1} на http://eradio.ua/rock/" +
				                    "&image={2}" +
				                    "&noparse=true", item.ArtistName, item.TrackName, item.ImagePath);

			Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(phpRequest));
			browserIntent.SetFlags (ActivityFlags.NewTask);
			_activity.StartActivity(browserIntent);
		}        
    }
}