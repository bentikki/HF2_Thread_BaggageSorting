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
        private FlightPlan flightPlan;
        public static int InQueueCapacity { get; private set; } = 50;
        public static Queue<Baggage> baggageQueueIn = new Queue<Baggage>();
        public static List<Terminal> Terminals = new List<Terminal>();
        public static List<CheckInDesk> checkInDesks = new List<CheckInDesk>();

        public SortingSystem(int numberOfCheckins, FlightPlan flightPlan)
        {
            this.flightPlan = flightPlan;
            //Add Terminals
            for (int i = 0; i < this.flightPlan.Destinations.Length; i++)
            {
                AddTerminal("Terminal #" + (i + 1), this.flightPlan.Destinations[i], i + 1);
            }
            //Add CheckinDesks
            for (int i = 0; i < numberOfCheckins; i++)
            {
                AddCheckInDesk("Checkin desk #" + (i + 1), (i + 1));
            }

            StartSorting();
        }

        private void StartSorting()
        {
            for (int i = 0; i < checkInDesks.Count; i++)
            {
                checkInDesks[i].OpenDesk();
            }

            for (int i = 0; i < 3; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SortBaggage));
            }

            new Thread(CheckFlightplan).Start();
        }

        //Main Baggagesort function. Used to sort the Baggage Queue IN.
        private void SortBaggage(object obj)
        {
            while (true)
            {
                Baggage bag = null;
                lock (baggageQueueIn)
                {
                    if(baggageQueueIn.Count == 0)
                    {
                        string message = $".....Baggage Queue(IN) is empty, not enough Baggage coming through Checkin - Current size[{baggageQueueIn.Count}]";
                        Printer.PrintMessage(message);
                        Printer.LogMessage(message);
                        Monitor.Wait(baggageQueueIn);
                    }
                    else
                    {
                        bag = baggageQueueIn.Dequeue();
                        Printer.PrintMessage($".....Added baggage with ID:{bag.Id} Destination:{bag.Destination} to Baggage Queue(IN) -  Current size[{baggageQueueIn.Count}]");
                        bag.AddToLog($".....Added baggage with ID:{bag.Id} Destination:{bag.Destination} to Baggage Queue(IN) -  Current size[{baggageQueueIn.Count}]");
                        Monitor.PulseAll(baggageQueueIn);
                    }
                }
                if (bag != null)
                {
                    Terminal destinationTermintal = Terminals.Find(x => x.Destination == bag.Destination);
                    if (destinationTermintal != null)
                    {
                        lock (destinationTermintal.BaggageList)
                        {
                            destinationTermintal.BaggageList.Enqueue(bag);
                            Monitor.Pulse(destinationTermintal.BaggageList);
                        }
                    }
                    else
                    {
                        //No gate with destination is found.
                        Printer.LogMessage($"!!!!!!!!!!!Warning[ Baggage lost with ID[{bag.Id}] and Destination[{bag.Destination}] ]Warning!!!!!!!!!!!");
                    }
                }

                Thread.Sleep(StaticRandom.Rand(200, 1000));
            }

        }

        //Method to check flightplan
        private void CheckFlightplan()
        {
            while (flightPlan.Plan.Count > 0)
            {

                List<DateTime> removeKeys = new List<DateTime>();
                Dictionary<int, string> newTerminals = new Dictionary<int, string>();

                foreach (KeyValuePair<DateTime, object[]> entry in flightPlan.Plan)
                {
                    if (DateTime.Now > entry.Key)
                    {
                        int terminalID = Convert.ToInt16(entry.Value[0]);
                        string terminalDestination = (string)entry.Value[1];

                        newTerminals.Add(terminalID, terminalDestination);
                        removeKeys.Add(entry.Key);
                    }
                }
                foreach (DateTime key in removeKeys)
                {
                    flightPlan.Plan.Remove(key);
                }
                foreach (KeyValuePair<int, string> entry in newTerminals)
                {
                    ShutdownTerminal(entry.Key);
                    AddTerminal(entry.Key, entry.Value);
                }

                Thread.Sleep(1000);
                
            }
        }

        //Method to add Terminals
        private Terminal AddTerminal(string name, string destination, int id)
        {
            Terminal terminal = new Terminal(name, destination, id);
            lock (Terminals)
            {
                Terminals.Add(terminal);
            }
            terminal.OpenTerminal();
            string message = $"Terminal {terminal.Name} with destination {terminal.Destination} is added.";
            Printer.PrintMessage(message);
            Printer.LogMessage(message);
            return terminal;
        }
        private Terminal AddTerminal(int id, string destination)
        {
            return AddTerminal("Terminal #" + (id + 1), destination, id + 1);
        }

        //Method to add Check In Desks
        private void AddCheckInDesk(string name, int id)
        {
            CheckInDesk checkInDesk = new CheckInDesk(name, id);
            lock (checkInDesks)
            {
                checkInDesks.Add(checkInDesk);
            }
            string message = $"Checkin Desk {checkInDesk.Name} with ID[{checkInDesk.DeskID}] is added.";
            Printer.PrintMessage(message);
            Printer.LogMessage(message);
        }

        public void ShutdownCheckinDesk(int shutDownID)
        {
            CheckInDesk removeDesk = checkInDesks.Find(i => i.DeskID == shutDownID);
            if (removeDesk != null)
            {
                Printer.LogMessage($"!!!!!!!!!!!!Shutting down Checkin Desk with ID[{removeDesk.DeskID}]!!!!!!!!!!!!");
                lock (checkInDesks)
                {
                    checkInDesks.Remove(removeDesk);
                }
                removeDesk.Shutdown();
            }
            else
            {
                Printer.LogMessage($"!!!!!!!!!!!!Warning[ No Checkin Desk with ID[{shutDownID}] found. ]Warning!!!!!!!!!!!!");
            }

        }

        public void ShutdownTerminal(int shutDownID)
        {
            Terminal removeTerminal = Terminals.Find(i => i.TerminalID == shutDownID);
            if (removeTerminal != null)
            {
                Printer.LogMessage($"!!!!!!!!!!!!Shutting down Terminal with ID[{removeTerminal.TerminalID}] and destination {removeTerminal.Destination}!!!!!!!!!!!!");
                lock (Terminals)
                {
                    Terminals.Remove(removeTerminal);
                }
                removeTerminal.Shutdown();
            }
            else
            {
                Printer.LogMessage($"!!!!!!!!!!!!Warning[ No Terminal with ID[{shutDownID}] found. ]Warning!!!!!!!!!!!!");
            }

        }

    }
}
