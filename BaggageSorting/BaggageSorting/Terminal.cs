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
        public Queue<Baggage> BaggageList { get; private set; } = new Queue<Baggage>();


        public Terminal(string name, string destination)
        {
            Name = name;
            Destination = destination;
        }

        public void AddBaggageToTerminal(Baggage bag)
        {
            Program.sortingSystem.baggageQueueIn.Dequeue();
            BaggageList.Enqueue(bag);
            Printer.PrintMessage($"..........Added baggage with ID:{bag.Id} Destination:{bag.Destination} to {this.Name}:{this.Destination} Baggage Queue(OUT) -  Current size[{this.BaggageList.Count}]");
            bag.AddToLog($"..........Added baggage with ID:{bag.Id} Destination:{bag.Destination} to {this.Name}:{this.Destination} Baggage Queue(OUT) -  Current size[{this.BaggageList.Count}]");

            foreach (string message in bag.Log)
            {
                Printer.LogMessage(message);
            }
            Thread.Sleep(500);
        }

    }
}
