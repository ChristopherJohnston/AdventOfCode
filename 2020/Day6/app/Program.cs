using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ParseInput().Select((p) => p.ToCharArray().Distinct().Count()).Sum());
        }

        static IEnumerable<string> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt");
            string currentPassport = String.Empty;
            for (int i=0; i<lines.Length; i++) {
                if (lines[i] == string.Empty) {
                    yield return currentPassport.TrimEnd();
                    currentPassport = String.Empty;
                } else {
                    currentPassport += lines[i];
                }
            }            
        }
    }
}
