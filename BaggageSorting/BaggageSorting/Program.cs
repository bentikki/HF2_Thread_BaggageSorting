using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfTerminals = 4;
            string[] Destinations = { "USA", "Russia", "Spain", "Portugal", "France", "Germany" };

            //New FlightPlan takes a path to FlghtPlan.txt, destinations and number of terminals.
            FlightPlan plan = new FlightPlan(@"..\..\FlightPlan\FlightPlan.txt", Destinations, numberOfTerminals);

            //SortingSystem takes the number of checkins, and a FlightPlan object.
            SortingSystem sortingSystem = new SortingSystem(5, plan);

        }


    }
}
