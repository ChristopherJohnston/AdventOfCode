using System;
using System.IO;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines(@"input.txt");
            long earliest = long.Parse(input[0]);
            // string[] buses = "7,13,x,x,59,x,31,19".Split(',');
            string[] buses = input[1].Split(',');

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
