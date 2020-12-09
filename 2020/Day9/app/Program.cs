using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace app
{
    class Program
    {
        static bool IsValidNumber(long value, List<long> preamble) {
            // find sum
            for (int j=0; j<24; j++) {
                for (int k=j+1; k<25; k++) {
                    if (preamble[j] + preamble[k] == value) {
                        return true;
                    }
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            List<long> preamble = new List<long>();
            int preambleLength = 25;
            int i = 0;
            long? nonSummingValue = null;

            long[] input = ParseInput().Select(s => long.Parse(s)).ToArray();

            foreach (long value in input) {            
                if (i > preambleLength) {
                    preamble.RemoveAt(0);
                    if (!IsValidNumber(value, preamble)) {
                        Console.WriteLine(value);
                        nonSummingValue = value;
                        break;
                    }
                }
                
                preamble.Add(value);
                i++;
            }

            if (!nonSummingValue.HasValue) {
                Console.WriteLine("No solution found for non-summing value.");
            }

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
