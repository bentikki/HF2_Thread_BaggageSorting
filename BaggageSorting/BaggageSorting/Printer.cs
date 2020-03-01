using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    static class Printer
    {
        static public object _logLock = new object();

        public static void PrintMessage(string s)
        {
            string output = Thread.CurrentThread.ManagedThreadId.ToString("#00");
            //Console.WriteLine( $"Thread[{output}]: " + s);
        }

        public static void LogMessage(string s)
        {
            string output = Thread.CurrentThread.ManagedThreadId.ToString("#00");
            Console.WriteLine($"Thread[{output}]: " + s);
        }
    }
}
