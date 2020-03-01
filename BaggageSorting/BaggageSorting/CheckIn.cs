using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class CheckInDesk
    {
        public string Name { get; private set; }
        public int DeskID { get; private set; }
        private Queue<Baggage> checkInQueue = new Queue<Baggage>();
        private Thread generaterThread;
        private Thread loaderThread;

        public CheckInDesk(string name, int id)
        {
            Name = name;
            DeskID = id;
        }

        public void OpenDesk()
        {
            generaterThread = new Thread(GenerateBaggage);
            generaterThread.Start();

            loaderThread = new Thread(LoadBaggage);
            loaderThread.Start();
        }

        public void Shutdown()
        {
            generaterThread.Abort();
            loaderThread.Abort();
        }

        private void LoadBaggage()
        {
            Printer.LogMessage($"{Name} LoadBaggage Thread Started.");
            while (true)
            {
                lock (checkInQueue)
                {
                    while (checkInQueue.Count == 0)
                    {
                        string message = $"!!!!!!!!!!!!!There is no baggage at {Name}. Not enough customers!!!!!!!!!!!!!";
                        Printer.PrintMessage(message);
                        Printer.LogMessage(message);
                        Monitor.Wait(checkInQueue);
                    }

                    Baggage bag = checkInQueue.Dequeue();
                    Printer.PrintMessage($"Checkin[{DeskID}]: Added baggage: " + bag.Id + " to Destination " + bag.Destination);
                    bag.AddToLog($"Checkin[{DeskID}]: Added baggage: " + bag.Id + " to Destination " + bag.Destination);

                    lock (SortingSystem.baggageQueueIn)
                    {
                        while (SortingSystem.baggageQueueIn.Count >= SortingSystem.InQueueCapacity)
                        {
                            Printer.PrintMessage($"!!!!!!!!!!!!!Baggage Queue(IN) is at Capacity[{SortingSystem.InQueueCapacity}] baggageQueueIn[{SortingSystem.baggageQueueIn.Count}]!!!!!!!!!!!!!");
                            Monitor.Wait(SortingSystem.baggageQueueIn);
                        }
                        SortingSystem.baggageQueueIn.Enqueue(bag);
                        Monitor.Pulse(SortingSystem.baggageQueueIn);
                    }

                    Monitor.Pulse(checkInQueue);
                }
                Thread.Sleep(StaticRandom.Rand(200, 1500));
            }

        }

        private void GenerateBaggage()
        {
            Printer.LogMessage($"{Name} GenerateBaggage Thread Started.");
            while (true)
            {
                lock (checkInQueue)
                {
                    while (checkInQueue.Count >= 500)
                    {
                        string message = $"!!!!!!!!!!!!!There is a queue at {Name}. It is at Capacity checkInQueue[{checkInQueue.Count}]!!!!!!!!!!!!!";
                        Printer.PrintMessage(message);
                        Printer.LogMessage(message);
                        Monitor.Wait(checkInQueue);
                    }
                    lock (SortingSystem.Terminals)
                    {
                        if (SortingSystem.Terminals.Count > 0)
                        {
                            int index = StaticRandom.Rand(SortingSystem.Terminals.Count - 1);
                            string destination = SortingSystem.Terminals[index].Destination;
                            Baggage bag = new Baggage(StaticRandom.Rand(1000), destination);
                            checkInQueue.Enqueue(bag);
                            Monitor.Pulse(checkInQueue);
                        }
                    }
                }
                Thread.Sleep(StaticRandom.Rand(200,1000));
            }

        }

    }
}
