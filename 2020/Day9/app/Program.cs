using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace app
{
    class Program
    {
        static bool IsValidNumber(long[] input, int start) {
            // Find any 2 numbers in the  25 indices of the input array from the start index
            // that sum to the 26th number.
            long target = input[start+25];
            for (int i=start; i<start+24; i++) {
                for (int j=i+1; j<i+25; j++) {
                    if (input[i] + input[j] == target) {
                        return true;
                    }
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            // Part 1 - Find a number that is not the sum of any 2 the previous 25
            long? nonSummingValue = null;

            long[] input = ParseInput().Select(s => long.Parse(s)).ToArray();

            for (int i=0; i<input.Length-26; i++) {
                if (!IsValidNumber(input, i)) {
                    nonSummingValue = input[i+25];
                    Console.WriteLine(nonSummingValue);
                    break;
                }
            }

            if (!nonSummingValue.HasValue) {
                Console.WriteLine("No solution found for non-summing value.");
            }

            // Part 2 - Find any length of contiguous numbers that sum to the non-summing value
            for (int l=4; l<input.Length; l++) {
                for (int m=2; m<input.Length-5-m; m++) {
                    long[] contiguousNumbers = input.Skip(l-4).Take(m).ToArray();
                    long x = contiguousNumbers.Sum();
                
                    if (x == nonSummingValue) {
                        long result = contiguousNumbers.Min() + contiguousNumbers.Max();
                        Console.WriteLine(result);
                        return;
                    }
                }
            }
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
