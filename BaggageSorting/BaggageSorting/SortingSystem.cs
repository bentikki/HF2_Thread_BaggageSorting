using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class SortingSystem
    {
        static object _lock = new object();
        public Queue<Baggage> baggageQueueIn = new Queue<Baggage>();
        public List<Terminal> Terminals = new List<Terminal>();

        public void AddBaggage(object obj)
        {
            if (obj is Baggage bag)
            {
                lock (_lock)
                {
                    baggageQueueIn.Enqueue(bag);
                    Printer.PrintMessage($".....Added baggage with ID:{bag.Id} Destination:{bag.Destination} to Baggage Queue(IN) -  Current size[{baggageQueueIn.Count}]");
                    bag.AddToLog($".....Added baggage with ID:{bag.Id} Destination:{bag.Destination} to Baggage Queue(IN) -  Current size[{baggageQueueIn.Count}]");
                }

                Terminal destinationTermintal = Terminals.Find(x => x.Destination == bag.Destination);
                if(destinationTermintal != null)
                {
                    lock (destinationTermintal.BaggageList)
                    {
                        destinationTermintal.AddBaggageToTerminal(bag);
                        Monitor.Pulse(destinationTermintal.BaggageList);
                    }
                }
                else
                {
                    //No gate with destination is found.
                }
                
            }
        }

        public void AddTerminal(Terminal terminal)
        {
            lock (Terminals)
            {
                Terminals.Add(terminal);
                Printer.PrintMessage($"Terminal #{terminal.Name} with destination {terminal.Destination} is added.");
            }
        }

    }
}
