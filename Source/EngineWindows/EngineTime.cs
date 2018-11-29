using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindows
{
    public class EngineTime
    {
        #region Attributes
            private Stopwatch _stopwatch;
            private double _lastUpdate;
        #endregion
        #region Properties
            public double ElapseTime
            {
                get
                {
                    return _stopwatch.ElapsedMilliseconds * 0.001;
                }
            }
        #endregion
        #region Constructors
            public EngineTime()
            {
                _stopwatch = new Stopwatch();
            }
        #endregion

        #region Timer
            public void Start()
            {
                _stopwatch.Start();
                _lastUpdate = 0;
            }

            public void Stop()
            {
                _stopwatch.Stop();
            }

            public double Update()
            {
                double now = ElapseTime;
                double updateTime = now - _lastUpdate;
                _lastUpdate = now;
                return updateTime;
            }
        #endregion
    }
}
