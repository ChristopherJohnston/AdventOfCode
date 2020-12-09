using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace app
{
    class Program
    {
        static bool IsValidNumber(long[] inputs) {
            // find any 2 numbers in the numbers that sum to the last item
            for (int j=0; j<24; j++) {
                for (int k=j+1; k<25; k++) {
                    if (inputs[j] + inputs[k] == inputs[25]) {
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
                long[] numbers = input.Skip(i).Take(26).ToArray();

                if (!IsValidNumber(numbers)) {
                    nonSummingValue = numbers.Last();
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
