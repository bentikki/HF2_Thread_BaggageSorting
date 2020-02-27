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

            GenerateBaggage();
        }

        public void LoadBaggage(Baggage bag)
        {
            for (int i = 0; i < Program.Destinations.Length; i++)
            {
                if(bag.Destination == Program.Destinations[i])
                {
                    Console.WriteLine("Added baggage: " + bag.Id + " to Destination " + bag.Destination);
                }
            }
        }

        private void GenerateBaggage()
        {
            while (true)
            {
                string[] arr = Program.Destinations;

                string destination = arr[ new Random().Next(0, arr.Length) ];
                Baggage bag = new Baggage(destination);
                this.LoadBaggage(bag);
            }
        }



    }
}
