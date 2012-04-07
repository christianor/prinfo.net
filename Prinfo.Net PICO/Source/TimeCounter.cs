using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cancom.prinfo.pico
{
    class TimeCounter
    {
        private DateTime startTime;
        private DateTime endTime;

        public void StartTimeMessure()
        {
            startTime = DateTime.Now;
        }
        public void EndTimeMessure()
        {
            endTime = DateTime.Now;
        }
        public TimeSpan? GetTimeDifference()
        {
            if (startTime == null || endTime == null)
                return null;

            return endTime - startTime;
        }

    }
}
