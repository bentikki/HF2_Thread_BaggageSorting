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
        public static string[] Destinations = { "USA", "Russia", "Spain", "Portugal", "France", "Germany" };
        public static SortingSystem sortingSystem = new SortingSystem();

        static void Main(string[] args)
        {
            CheckInDesk Desk1 = new CheckInDesk("Checkin desk #1");
            CheckInDesk Desk2 = new CheckInDesk("Checkin desk #2");
            CheckInDesk Desk3 = new CheckInDesk("Checkin desk #3");
            CheckInDesk Desk4 = new CheckInDesk("Checkin desk #4");

            for (int i = 0; i < Destinations.Length; i++)
            {
                new Terminal("Terminal #" + ( i + 1 ), Destinations[i]);
            }

            


        }


    }
}
