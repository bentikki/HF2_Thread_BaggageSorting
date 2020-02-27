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

        public CheckInDesk(string name)
        {
            Name = name;
            new Thread(GenerateBaggage).Start();
        }

        public void LoadBaggage(Baggage bag)
        {
            for (int i = 0; i < Program.Destinations.Length; i++)
            {
                if(bag.Destination == Program.Destinations[i])
                {
                    Printer.PrintMessage("Added baggage: " + bag.Id + " to Destination " + bag.Destination);
                    bag.AddToLog("Added baggage: " + bag.Id + " to Destination " + bag.Destination);
                    Program.sortingSystem.AddBaggage(bag);
                }
            }
        }

        private void GenerateBaggage(object obj)
        {
            while (true)
            {
                string[] arr = Program.Destinations;

                string destination = arr[ new Random().Next(0, arr.Length) ];
                Baggage bag = new Baggage(new Random().Next(10000), destination);
                this.LoadBaggage(bag);
            }
        }



    }
}
