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
            long[] prevMeasurements = new long[3];
            long[] currMeasurements = new long[3];
            int i=0;

            int numIncreases = 0;

            foreach (string d in ParseInput()) {
                long depth = long.Parse(d);

                if (i < 3) {
                    prevMeasurements[i] = depth;
                    i++;
                }
                else {
                    currMeasurements[0] = prevMeasurements[1];
                    currMeasurements[1] = prevMeasurements[2];
                    currMeasurements[2] = depth;

                    if (prevMeasurements.Sum() < currMeasurements.Sum()) {
                        numIncreases++;
                    }
                    
                    currMeasurements.CopyTo(prevMeasurements, 0);
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
