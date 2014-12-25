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

namespace Eradio
{
    public static class Global
    {
        public static bool IsDestoyed = false;
        private static MediaProvider _mediaProviderObj;
        private static BackThread _backThreadObj;

        static Global()
        {
            _mediaProviderObj = new MediaProvider();
            _backThreadObj = new BackThread();            
        }

        #region Messages
        public static string MsgCannotLoadStream = "Не вдалося відтворити потік";
        public static string MsgTitle = "Є! Rock Радіо";
        public static string MsgLoading = "Завантаження...\n";
        public static string MediaStreamPath = @"http://live.eradio.ua/e-rock_hi";
        #endregion

        #region Voids
        public static void SendOnError(string message)
        {
            SendOnLoadEnded();
            if (OnError != null) OnError(null, message);            
        }
        public static void SendOnLoadStart()
        {
            if (OnLoadStart != null) OnLoadStart(null, null);
        }
        public static void SendOnLoadEnded()
        {
            if (OnLoadEnd != null) OnLoadEnd(null, null);
        }
        public static void SendOnMediaStateChanged()
        {
            if (OnMediaStateChanged != null) OnMediaStateChanged(null, null);
        }
        public static void SendOnPlaying()
        {
            if (OnPlaying != null) OnPlaying(null, null);
        }

        public static void SendOnTopTenChanged(TopTen arg)
        {
            if (OnTopTenChanged != null) OnTopTenChanged(null, arg);
        }
        public static void SendOnNowPlayChanged(NowPlay arg)
        {
            if (OnNowPlayChanged != null) OnNowPlayChanged(null, arg);
        }
        public static void SendOnHistoryPlayChanged(HistoryPlay arg)
        {
            if (OnHistoryPlayChanged != null) OnHistoryPlayChanged(null, arg);
        }

        public static void ClearEvents()
        {
            OnError = null;
            OnLoadStart = null;
            OnLoadEnd = null;
            OnMediaStateChanged = null;
            OnPlaying = null;
            OnHistoryPlayChanged = null;
            OnTopTenChanged = null;
        }

        public static void RefreshData()
        {
            _backThreadObj.RefreshData();
        }

        public static void StartPlay()
        {
            _mediaProviderObj.PlayMedia();
            _backThreadObj.StartThread();
        }
        public static void StopPlay()
        {
            _mediaProviderObj.StopMedia();
            _backThreadObj.StopThread();
        }
        public static bool IsPlay()
        {
            return _mediaProviderObj.IsPlaying();
        }
        #endregion

        #region Event
        public static event MessageEventHandler OnError;
        public static event EventHandler OnLoadStart;
        public static event EventHandler OnLoadEnd;
        public static event EventHandler OnMediaStateChanged;
        public static event EventHandler OnPlaying;        
        public static event NowPlayEventHandler OnNowPlayChanged;        
        public static event HistoryPlayHandler OnHistoryPlayChanged;        
        public static event TopTenEventHandler OnTopTenChanged;

        public delegate void NowPlayEventHandler(object sender, NowPlay arg);
        public delegate void HistoryPlayHandler(object sendeg, HistoryPlay arg);
        public delegate void TopTenEventHandler(object sender, TopTen arg);
        public delegate void MessageEventHandler(object sender, string arg);
        #endregion
    }
}