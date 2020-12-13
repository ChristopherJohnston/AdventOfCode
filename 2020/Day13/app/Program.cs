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
            Part1(earliest, buses);
            Part2(buses);
        }

        static void Part2(string[] buses) {
            long i = 100000000000000;
            bool found = false;
            while (!found) {
                i++;

                found = buses.Select((busId, idx) => {
                    if (busId == "x")
                        return true;

                    long bId = long.Parse(busId);
                    return (i % bId) == (bId - idx);
                }).All((b) => b);
            }
            Console.WriteLine(i);
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
