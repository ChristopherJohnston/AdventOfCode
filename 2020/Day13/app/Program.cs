using System;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines(@"input.txt");
            long earliest = long.Parse(input[0]);
            string[] buses = input[1].Split(',');

            // long earliest = 939;
            // string[] buses = "7,13,x,x,59,x,31,19".Split(',');
            // string[] buses = "67,7,59,61".Split(',');
            // Part1(earliest, buses);
            Part2(buses);
        }

        public static void Part2(string[] buses)
        {
            long timestamp = 1;
            long multiplier = 1;

            for (int offset=0; offset<buses.Length; offset++)
            {
                if (buses[offset] == "x")
                    continue;
                    
                long id = long.Parse(buses[offset]);
                bool found = false;
                while (!found) {
                    timestamp += multiplier;
                    found = (timestamp + offset) % id == 0;
                }
                multiplier *= id;
                Console.WriteLine("Iteration {0}: Timestamp={1}, Multiplier={2}", offset, timestamp, multiplier);
            }

            Console.WriteLine(timestamp);
        }

        static void Part1(long earliest, string[] buses) {
            long waitTime = 939;
            long earliestBus = 0;

            foreach (string bus in buses) {
                if (bus == "x")
                    continue;
                long busId = long.Parse(bus);
                long timeToBus = earliest % busId;
                long newWaitTime = busId - timeToBus;
                if ((timeToBus > (busId/2.0)) && newWaitTime < waitTime) {
                    earliestBus = busId;
                    waitTime = newWaitTime;
                }
            }

            Console.WriteLine("Bus Id: {0}, Wait Time {1}, Result: {2}", earliestBus, waitTime, waitTime * earliestBus);
        }
    }
}
