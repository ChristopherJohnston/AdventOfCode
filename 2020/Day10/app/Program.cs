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
            List<int> input = ParseInput().Select(int.Parse).ToList();
            input.Sort();
            
            int OneJolt = 0;
            int ThreeJolt = 1; // built-in adapter is always 3-jolt differnce
            int previousJoltage = 0;
            
            foreach (int joltage in input) {
                int difference = joltage - previousJoltage;
                if (difference == 3) {
                    ThreeJolt++;
                }
                else {
                    OneJolt++;
                }

                previousJoltage = joltage;
            }

            Console.WriteLine(OneJolt * ThreeJolt);
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
