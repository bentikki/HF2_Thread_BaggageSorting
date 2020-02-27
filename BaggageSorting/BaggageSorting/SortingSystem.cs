using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class SortingSystem
    {
        private Queue<Baggage> baggageQueue { get; set; }

        public void AddBaggage(Baggage bag)
        {
            lock (baggageQueue)
            {
                this.baggageQueue.Enqueue(bag);
            }
        }



    }
}
