using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class Baggage
    {
        public int Id { get; private set; }
        public string Destination { get; private set; }

        public Baggage(string destination)
        {
            Id = new Random().Next(0, 10000);
            Destination = destination;
        }


    }
}
