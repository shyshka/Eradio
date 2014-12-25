﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Eradio
{
    public class BackThread
    {
        private const int interval = 10000;
        private Thread threadNowPlay;        

        private NowPlay _nowPlayObj;
        private NowPlay NowPlayObj
        {
            get { return _nowPlayObj; }
            set
            {
                if (value == null) return;
                if (_nowPlayObj != null)
                    if (_nowPlayObj.Equals(value)) return;
                _nowPlayObj = value;
                Global.SendOnNowPlayChanged(_nowPlayObj);
            }
        }

        private HistoryPlay _historyPlayObj;
        private HistoryPlay HistoryPlayObj
        {
            get { return _historyPlayObj; }
            set
            {
                if (value == null) return;
                if (_historyPlayObj != null)
                    if (_historyPlayObj.Equals(value)) return;
                _historyPlayObj = value;
                Global.SendOnHistoryPlayChanged(_historyPlayObj);
            }
        }

        private TopTen _topTenObj;
        private TopTen TopTenObj
        {
            get { return _topTenObj; }
            set
            {
                if (value == null) return;
                if (_topTenObj != null)
                    if (_topTenObj.Equals(value)) return;
                _topTenObj = value;
                Global.SendOnTopTenChanged(_topTenObj);                
            }
        }

        public BackThread()
        {
            this._nowPlayObj = new NowPlay();
            this._historyPlayObj = new HistoryPlay();
            this._topTenObj = new TopTen();           

            Global.OnNowPlayChanged += delegate
            {
                new Thread(new ThreadStart(delegate
                {
                    HistoryPlayObj = HistoryPlay.CreateNewObject();
                    TopTenObj = TopTen.CreateNewObject();
                })).Start();                
            };
        }

        public void RefreshData()
        {
            if (Global.IsDestoyed && !string.IsNullOrEmpty(_nowPlayObj.ARTIST_NAME))
            {
                Global.SendOnNowPlayChanged(_nowPlayObj);
                Global.SendOnHistoryPlayChanged(_historyPlayObj);
                Global.SendOnTopTenChanged(_topTenObj);
            }
        }

        public void StartThread()
        {
            StopThread();
            threadNowPlay = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    NowPlayObj = NowPlay.CreateNewObject();
                    Thread.Sleep(interval);
                }
            }));
            threadNowPlay.Start();           
        }

        public void StopThread()
        {
           if (threadNowPlay != null) threadNowPlay.Abort();
        }
    }
}
