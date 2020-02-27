using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class Baggage
    {
        public int Id { get; private set; }
        public string Destination { get; private set; }
        public List<string> Log { get; private set; } = new List<string>();

        public Baggage(int id, string destination)
        {
            Id = id;
            Destination = destination;
        }

        public void AddToLog(string logMessage)
        {
            Log.Add($"[{Thread.CurrentThread.ManagedThreadId}]: {DateTime.Now}: {logMessage}" );
        }

    }
}
