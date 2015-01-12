namespace Eradio
{
	public class HistoryPlayItem
	{
		public string ImagePath;
		public string ArtistName;
		public string TrackName;
		public string TimeVal;
		public Android.Graphics.Bitmap ImageBitmap;

		public override bool Equals (object obj)
		{
			HistoryPlayItem comObj = obj as HistoryPlayItem;
			bool res = true;
			if (this.ImagePath != comObj.ImagePath)
				res = false;
			if (this.ArtistName != comObj.ArtistName)
				res = false;
			if (this.TrackName != comObj.TrackName)
				res = false;
			if (this.TimeVal != comObj.TimeVal)
				res = false;
			return res;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}