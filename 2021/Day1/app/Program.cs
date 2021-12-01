using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace app
{
    class Program
    {
        static string file = @"input.txt";
        static void Main(string[] args)
        {
            if (args.Length > 0) {
                file = args[0];
            }

            Part1();
            Part2();
        }

        static void Part1() {
            long prevDepth = long.MinValue;
            int numIncreases = 0;
            foreach (string d in ParseInput()) {
                long currentDepth = long.Parse(d);
                if (prevDepth > long.MinValue && currentDepth > prevDepth) {
                    numIncreases++;
                }
                prevDepth = currentDepth;
            }
            Console.WriteLine(numIncreases);
        }

        static void Part2() {
            Queue<long> measurements = new Queue<long>();
            int numIncreases = 0;

            foreach (string d in ParseInput().Take(3)) {
                measurements.Enqueue(long.Parse(d));
            }

            foreach (string d in ParseInput().Skip(3)) {
                long prevSum = measurements.Sum();
                
                measurements.Dequeue();
                measurements.Enqueue(long.Parse(d));

                if (prevSum < measurements.Sum()) {
                    numIncreases++;
                }
            }

            Console.WriteLine(numIncreases);
        }
        
        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(file)) {
                yield return line;
            }
        }
    }
}
