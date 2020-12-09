using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace app
{
    class Program
    {
        static bool IsValidNumber(int value, List<int> preamble) {
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
            List<int> preamble = new List<int>();
            int preambleLength = 25;
            int i = 0;

            foreach (string line in ParseInput()) {
                int value = int.Parse(line);                
            
                if (i > preambleLength) {
                    preamble.RemoveAt(0);
                    if (!IsValidNumber(value, preamble)) {
                        Console.WriteLine(value);
                        return;
                    }
                }
                
                preamble.Add(value);
                i++;
            }
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
