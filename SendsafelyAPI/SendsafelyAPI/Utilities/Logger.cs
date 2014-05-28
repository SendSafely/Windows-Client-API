using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SendSafely.Utilities
{
    class Logger
    {
        public static void Log(String msg)
        {
            bool debug = false;

            if (debug)
            {
                Console.WriteLine(msg);
            }
        }
    }
}
