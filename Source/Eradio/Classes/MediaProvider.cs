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

        public bool IsPlaying()
        {
            return this._mPlayerObj.IsPlaying;            
        }

        public void PlayMedia()
        {
            Global.SendOnLoadStart();
            this._mPlayerObj.Reset();
            this._mPlayerObj.SetDataSource(Global.MediaStreamPath);
            this._mPlayerObj.PrepareAsync();
        }

        public void StopMedia()
        {
            this._mPlayerObj.Reset();
            Global.SendOnMediaStateChanged();
        }
    }
}