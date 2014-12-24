using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Request
{
    public class BackThread
    {
        public  bool IsCanceled = false;
        private const int interval = 10000;

        public delegate void NowPlayEventHandler(object sender, NowPlay arg);
        public  event NowPlayEventHandler NowPlayChanged;

        private  NowPlay _nowPlay_obj;
        private  NowPlay NowPlay_obj
        {
            get { return _nowPlay_obj; }
            set
            {
                if (value == null) return;
                if (_nowPlay_obj != null)
                    if (_nowPlay_obj.Equals(value)) return;
                _nowPlay_obj = value;
                if (NowPlayChanged != null) NowPlayChanged(null, _nowPlay_obj);
            }
        }

        public  void StartThread()
        {
            Thread thread = new Thread(new ThreadStart(delegate
                {
                    while (!IsCanceled)
                    {
                        NowPlay_obj = NowPlay.CreateNewObject();
                        Thread.Sleep(interval);
                    }
                }));
            thread.Start();
        }
    }
}
