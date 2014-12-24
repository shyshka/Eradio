using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Request
{
    public class BackThread
    {
        private const int interval = 10000;
        private Thread threadNowPlay;
        private Thread threadHistoryPlay;

        public delegate void NowPlayEventHandler(object sender, NowPlay arg);
        public event NowPlayEventHandler NowPlayChanged;

        public delegate void HistoryPlayHandler(object sendeg, HistoryPlay arg);
        public event HistoryPlayHandler HistoryPlayChanged;

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
                if (NowPlayChanged != null) NowPlayChanged(null, _nowPlayObj);
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
                if (HistoryPlayChanged != null) HistoryPlayChanged(null, _historyPlayObj);
            }
        }

        public BackThread()
        {
            this.NowPlayChanged += delegate
            {
                threadHistoryPlay = new Thread(new ThreadStart(delegate
                {
                    HistoryPlayObj = HistoryPlay.CreateNewObject();
                }));
                threadHistoryPlay.Start();
            };
        }

        public void StartThread()
        {
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
           if (threadHistoryPlay != null) threadHistoryPlay.Abort();
        }
    }
}
