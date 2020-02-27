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
        public static void PrintMessage(string s)
        {
            //Console.WriteLine( $"Thread[{Thread.CurrentThread.ManagedThreadId}]: " + s);
        }

        public static void LogMessage(string s)
        {
            Console.WriteLine($"Thread[{Thread.CurrentThread.ManagedThreadId}]: " + s);
        }
    }
}
