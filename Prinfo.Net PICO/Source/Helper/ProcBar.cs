using System;
using System.Collections.Generic;
using System.Text;

namespace com.monitoring.prinfo.pico
{
    /// <summary>
    /// prints a procentual bar to the console
    /// </summary>
    public class ProcBar
    {
        public int ProcentualValue { set; get; }
        private int _barLength = 25;
        public int BarLength 
        {
            set
            {
                _barLength = value;
            }
            get
            {
                return _barLength;
            }
        }

        public void Draw()
        {
            Console.CursorLeft = 0;
            
            int barValue = (int)((double)ProcentualValue / 100 * _barLength);

            Console.Write('[');

            for (int i = 1; i <= _barLength; i++)
            {
                if (i <= barValue)
                    Console.Write('=');
                else
                    Console.Write('.');
            }

            Console.Write(']');
            //clear prozvalue area
            Console.Write("     ");
            Console.CursorLeft -= 5;
            Console.Write(" " + ProcentualValue + "%");
        }


    }
}
