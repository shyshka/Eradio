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
using Android.Media;

namespace Eradio
{
    public class MediaProvider
    {
        private MediaPlayer _mPlayerObj;

        public MediaPlayer MPlayerObj
        {
            get { return _mPlayerObj; }
            set { _mPlayerObj = value; }
        }

        public MediaProvider()
        {
            this._mPlayerObj = new MediaPlayer();
            this._mPlayerObj.Info += delegate { };
            this._mPlayerObj.Prepared += delegate
            {
                Global.SendOnLoadEnded();
                this._mPlayerObj.Start();
                Global.SendOnMediaStateChanged();
            };
            this._mPlayerObj.BufferingUpdate += delegate { Global.SendOnPlaying(); };
            this._mPlayerObj.Completion += delegate { Global.SendOnError(Global.MsgCannotLoadStream); };
            this._mPlayerObj.Error += delegate { Global.SendOnError(Global.MsgCannotLoadStream); };
            this._mPlayerObj.SeekComplete += delegate { };
        }
    }
}