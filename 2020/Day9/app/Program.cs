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

            string[] input = ParseInput().ToArray();

            foreach (string line in input) {
                long value = long.Parse(line);                
            
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
                    long[] lastFive = input.Skip(l-4).Take(m).Select((s) => long.Parse(s)).ToArray();
                    long x = lastFive.Sum();
                
                    if (x == nonSummingValue) {
                        long result = lastFive.Min() + lastFive.Max();
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
