using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Request
{
    public class NowPlay
    {
        public string ARTIST_NAME;
        public string TRACK_SONG;
        public string PICTURE;

        public static NowPlay CreateNewObject()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://eradio.ua/now_play.php");
                var data = Encoding.ASCII.GetBytes("src=http://eradio.ua/rock/");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamJson = response.GetResponseStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NowPlay));
                NowPlay nowPlayObj = (NowPlay)ser.ReadObject(streamJson);
                nowPlayObj.PICTURE = nowPlayObj.PICTURE.Trim('\r', '\n');
                return nowPlayObj;
            }
            catch { return null; }
        }

        public override bool Equals(object obj)
        {
            bool res = true;
            NowPlay objNowPlay = obj as NowPlay;
            if (objNowPlay.ARTIST_NAME != this.ARTIST_NAME) res = false;
            if (objNowPlay.TRACK_SONG != this.TRACK_SONG) res = false;
            if (objNowPlay.PICTURE != this.PICTURE) res = false;
            return res;
        }
    }
}
