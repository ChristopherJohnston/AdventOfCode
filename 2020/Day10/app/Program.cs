﻿using System;
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

            CountDifferences(input);
            CountArrangements(input);
        }

        static void CountArrangements(List<int> input){
            Dictionary<int, long> arrangements = new Dictionary<int, long>();
            arrangements[0] = 1;

            foreach (int currentJoltage in input) {
                arrangements[currentJoltage] = 0;
                for (int i=1; i<4; i++) {
                    if (arrangements.ContainsKey(currentJoltage-i))
                        arrangements[currentJoltage] += arrangements[currentJoltage-i];
                }
            }
            Console.WriteLine(arrangements[input.Last()]);
        }

        static void CountDifferences(List<int> input) {            
            int OneJolt = 0;
            int ThreeJolt = 1; // built-in adapter is always 3-jolt difference
            int previousJoltage = 0;

            foreach (int joltage in input) {
                if (joltage - previousJoltage == 3)
                    ThreeJolt++;
                else
                    OneJolt++;

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
