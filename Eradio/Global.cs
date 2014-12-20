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
        public static string MsgCannotLoadStream = "�� ������� ��������� ����";
        public static string MsgTitle = "�! ����";
        public static string MsgLoading = "������������\n";

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

        public static event EventHandler OnError;
        public static event EventHandler OnLoadStart;
        public static event EventHandler OnLoadEnd;
        public static event EventHandler OnMediaStateChanged;

        public class MessageEventArgs : EventArgs
        {
            public string Message;

            public MessageEventArgs(string message)
            {
                this.Message = message;
            }
        }

        public enum RadioKind
        {
            UaRadio,
            RockRadio,
            HitRadio,
            DanceRadio
        }

        public static string[] RadioList = new string[] { "�! UA Radio", "�! Rock Radio", "�! Hit Radio", "�! DanceRadio" };
    }
}