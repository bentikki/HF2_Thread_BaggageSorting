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

            FlightPlan plan = new FlightPlan(@"..\..\FlightPlan\FlightPlan.txt", Destinations, numberOfTerminals);
            SortingSystem sortingSystem = new SortingSystem(5, plan);

            //while (true)
            //{
            //    ConsoleKeyInfo UserInput = Console.ReadKey();

            //    if (char.IsDigit(UserInput.KeyChar))
            //    {
            //        sortingSystem.ShutdownTerminal(int.Parse(UserInput.KeyChar.ToString()));
            //    }
            //}
        }


    }
}
