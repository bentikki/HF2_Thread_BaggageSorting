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
            for (int i = 0; i < Destinations.Length; i++)
            {
                sortingSystem.AddTerminal(
                        new Terminal("Terminal #" + (i + 1), Destinations[i])
                    );
            }

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(new CheckInDesk("Checkin desk #" + (i + 1)).OpenDesk));
                //new CheckInDesk("Checkin desk #" + (i+1));
            }
            Console.ReadLine();
        }


    }
}
