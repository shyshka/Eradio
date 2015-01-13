using Android.Graphics;
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
    public class TopTenCollection
    {    	
        private List<TopTenItem> _lstTopTen;        

		public TopTenCollection()
		{
			this._lstTopTen = new List<TopTenItem> ();
		}

		public int Count {
			get{ return this._lstTopTen.Count; }
		}

		public TopTenItem this [int i] {
			get { return this._lstTopTen [i]; }
		}

        public static TopTenCollection CreateNewObject()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://eradio.ua/top_ten.php");
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
                string sArtist = "</audio>[^-]*";
                string sTrack = "- <span>[^<]*";
                string sSrcFile = "\" src=\"..[^\"]*";

                MatchCollection pictures = Regex.Matches(s, sPicture);
                MatchCollection artists = Regex.Matches(s, sArtist);
                MatchCollection tracks = Regex.Matches(s, sTrack);
                MatchCollection srcFiles = Regex.Matches(s, sSrcFile);

                TopTenCollection historyPlayObj = new TopTenCollection();                
                for (int i = 0; i < artists.Count; i++)
                {
					historyPlayObj._lstTopTen.Add(new TopTenItem
                    {
                        ImagePath = pictures[i].Value.Remove(0, 10).Trim('"'),
                        ArtistName = artists[i].Value.Remove(0, 9).Trim(' '),
                        TrackName = tracks[i].Value.Remove(0, 8),
                        SrcFilePath = "http://eradio.ua" +srcFiles[i].Value.Remove(0, 9),
                        ImageBitmap = WebProvider.GetImageBitmapFromUrl(pictures[i].Value.Remove(0, 10).Trim('"'))
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
