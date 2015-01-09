using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Eradio
{
    public class HistoryPlayCollection
    {
		public HistoryPlayCollection()
        {
			this._lstHistory = new List<HistoryPlayItem> ();
        }

		public int Count {
			get {
				return this._lstHistory.Count;
			}
		}

		public HistoryPlayItem this[int i]
		{
			get{ return this._lstHistory [i]; }
		}

        private List<HistoryPlayItem> _lstHistory;        

		/// <summary>
		/// Метод ініціалізації обьекту типу HistoryPlayCollection
		/// </summary>
		/// <returns>The new object of HistoryPlayCollection</returns>
        public static HistoryPlayCollection CreateNewObject()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://eradio.ua/play_history.php");
                var data = Encoding.ASCII.GetBytes("src=http://eradio.ua/rock/");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.ContentLength = data.Length;
                var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                GZipStream streamDecompress = new GZipStream(streamResponse, CompressionMode.Decompress);
                StreamReader sr = new StreamReader(streamDecompress);
                string s = sr.ReadToEnd();
                sr.Close();

                string sPicture = "<img src=\"[^\"]*\"";
                string sArtist = "track\"[^-]*";
                string sTrack = "- <span>[^<]*";
                string sTime = "time\">[^<]*";

                MatchCollection pictures = Regex.Matches(s, sPicture);
                MatchCollection artists = Regex.Matches(s, sArtist);
                MatchCollection tracks = Regex.Matches(s, sTrack);
                MatchCollection times = Regex.Matches(s, sTime);

                HistoryPlayCollection historyPlayObj = new HistoryPlayCollection();                
                for (int i = 0; i < artists.Count; i++)
                {
                    historyPlayObj._lstHistory.Add(new HistoryPlayItem
                    {
                        ImagePath = pictures[i].Value.Remove(0, 10).Trim('"'),
                        ArtistName = artists[i].Value.Remove(0, 9).Trim(' '),
                        TrackName = tracks[i].Value.Remove(0, 8),
                        TimeVal = times[i].Value.Remove(0, 7).Trim(' '),
                        ImageArtist = WebProvider.GetImageBitmapFromUrl(pictures[i].Value.Remove(0, 10).Trim('"'))
                    });                    
                }
                return historyPlayObj;
            }
            catch { return null; }
        }

        public override bool Equals(object obj)
        {
           /* bool res = true;
            H/istoryPlay objNowPlay = obj as HistoryPlay;
            if (objNowPlay.ARTIST_NAME != this.ARTIST_NAME) res = false;
            if (objNowPlay.TRACK_SONG != this.TRACK_SONG) res = false;
            if (objNowPlay.PICTURE != this.PICTURE) res = false;*/
            return false;
        }  

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
    }
}
