using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
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
        
        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
