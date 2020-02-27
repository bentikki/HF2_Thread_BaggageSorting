using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class Terminal
    {
        public string Name { get; private set; }
        public string Destination { get; private set; }
        private Baggage[] baggageList = new Baggage[30];

        public Terminal(string name, string destination)
        {
            Name = name;
            Destination = destination;
        }

    }
}
