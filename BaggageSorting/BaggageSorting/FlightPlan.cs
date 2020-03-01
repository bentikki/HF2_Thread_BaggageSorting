using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorting
{
    class FlightPlan
    {
        private string filePathToPlan;
        private readonly string formatDescription = "Format: [Arrival Time], [Terminal ID], [Destination]";
        public Dictionary<DateTime, object[]> Plan { get; private set; } = new Dictionary<DateTime, object[]>();
        public string[] Destinations { get; private set; }
        public int NumberOfTerminals { get; private set; }

        public FlightPlan(string filePathToPlan, string[] destinations, int numberOfTerminals)
        {
            this.filePathToPlan = filePathToPlan;
            this.Destinations = destinations;
            this.NumberOfTerminals = numberOfTerminals;

            //Generates a new flight plan for testing.
            GenerateFlightPlan();

            //Reads from the flight plan.
            ReadFromPlanFile();
        }

        //Reads from flightplan file.
        private void ReadFromPlanFile()
        {
            string fileName = filePathToPlan;

            try
            {
                // Open the stream and read it back.    
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        if(s != formatDescription)
                        {
                            string[] entry = s.Split(',');
                            Plan.Add(
                                DateTime.Parse(entry[0]),
                                    new object[2]
                                    {
                                    entry[1],
                                    entry[2]
                                    }
                                );
                        } 
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

        }



        //Methods to generate and create Flightplan File.
        private void GenerateFlightPlan()
        {
            Dictionary<DateTime, string> generatePlan = new Dictionary<DateTime, string>();

            int startTime = 5;
            int endTime = 15;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j <= NumberOfTerminals; j++)
                {
                    generatePlan.Add(
                        DateTime.Now.AddSeconds(StaticRandom.Rand(startTime, endTime)).AddMilliseconds(StaticRandom.Rand(1, 999)),
                        $"{ j },{Destinations[StaticRandom.Rand(0, (Destinations.Length))]}"
                    );
                }
                startTime += 10;
                endTime += 10;
            }
            

            //Dictionary<DateTime, string> generatePlan =
            //    new Dictionary<DateTime, string>
            //    {
            //        {
            //            DateTime.Now.AddSeconds(StaticRandom.Rand(5,20)).AddMilliseconds(StaticRandom.Rand(1,999)),
            //            "1, UK"
            //        },
            //        {
            //            DateTime.Now.AddSeconds(StaticRandom.Rand(5,20)).AddMilliseconds(StaticRandom.Rand(1,999)),
            //            "2, Australia"
            //        },
            //        {
            //            DateTime.Now.AddSeconds(StaticRandom.Rand(5,20)).AddMilliseconds(StaticRandom.Rand(1,999)),
            //            "3, Brazil"
            //        },
            //        {
            //            DateTime.Now.AddSeconds(StaticRandom.Rand(5,20)).AddMilliseconds(StaticRandom.Rand(1,999)),
            //            "4, Portugal"
            //        },
            //    };

            CreateFile(generatePlan);
        }
        private void CreateFile(Dictionary<DateTime, string> generatePlan)
        {
            string fileName = filePathToPlan;
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(formatDescription);
                    foreach (KeyValuePair<DateTime, string> entry in generatePlan)
                    {
                        sw.WriteLine($"{entry.Key.ToString("HH:mm:ss.fff")}, {entry.Value}");

                    }
                }

            }
            catch (Exception Ex)
            {
                Printer.LogMessage(Ex.ToString());
            }
        }
    }
}
