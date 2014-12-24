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
        public static MediaProvider MediaProviderObj;

        static Global()
        {
            MediaProviderObj = new MediaProvider();
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
            if (OnError != null) OnError(null, new MessageEventArgs(message));
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

        #endregion

        #region Event
        public static event EventHandler OnError;
        public static event EventHandler OnLoadStart;
        public static event EventHandler OnLoadEnd;
        public static event EventHandler OnMediaStateChanged;
        public static event EventHandler OnPlaying;

        public class MessageEventArgs : EventArgs
        {
            public string Message;

            public MessageEventArgs(string message)
            {
                this.Message = message;
            }
        }
        #endregion
    }
}