using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class Terminal
    {
        public string Name { get; private set; }
        public string Destination { get; private set; }
        public int TerminalID { get; private set; }
        public Queue<Baggage> BaggageList { get; private set; } = new Queue<Baggage>();
        private Thread loaderThread;


        public Terminal(string name, string destination, int id)
        {
            Name = name;
            Destination = destination;
            TerminalID = id;
        }

        public void OpenTerminal()
        {
            loaderThread = new Thread(LoadBaggage);
            loaderThread.Start();
        }

        public void Shutdown()
        {
            loaderThread.Abort();
        }

        public void LoadBaggage()
        {
            Printer.LogMessage($"{Name} LoadBaggage Thread Started.");
            while (true)
            {
                Baggage bag;
                lock (BaggageList)
                {
                    Monitor.Wait(BaggageList);
                    bag = BaggageList.Dequeue();
                    Monitor.Pulse(BaggageList);
                }
                Printer.PrintMessage($"..........Terminal[{this.TerminalID}]:Added baggage with ID:{bag.Id} Destination:{bag.Destination} to {this.Name}:{this.Destination} Baggage Queue(OUT) -  Current size[{this.BaggageList.Count}]");
                bag.AddToLog($"..........Terminal[{this.TerminalID}]:Added baggage with ID:{bag.Id} Destination:{bag.Destination} to {this.Name}:{this.Destination} Baggage Queue(OUT) -  Current size[{this.BaggageList.Count}]");

                foreach (string message in bag.Log)
                {
                    Printer.LogMessage(message);
                }
                Thread.Sleep(StaticRandom.Rand(100, 500));
            }


        }

        public void AddBaggageToTerminal(object obj)
        {
            Baggage bag = (Baggage)obj;
            if (bag != null)
            {
                Monitor.Enter(SortingSystem.baggageQueueIn);
                SortingSystem.baggageQueueIn.Dequeue();

                BaggageList.Enqueue(bag);
                Printer.PrintMessage($"..........Added baggage with ID:{bag.Id} Destination:{bag.Destination} to {this.Name}:{this.Destination} Baggage Queue(OUT) -  Current size[{this.BaggageList.Count}]");
                bag.AddToLog($"..........Added baggage with ID:{bag.Id} Destination:{bag.Destination} to {this.Name}:{this.Destination} Baggage Queue(OUT) -  Current size[{this.BaggageList.Count}]");

                foreach (string message in bag.Log)
                {
                    Printer.LogMessage(message);
                }
                Thread.Sleep(StaticRandom.Rand(100, 500));
            }
        }

    }
}
