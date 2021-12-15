using System;
using Common;
using System.Linq;
using System.Collections.Generic;
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
            
            List<string> input = FileUtils.ParseInput(file).ToList();

            string start = input.First();

            Dictionary<string, string> map = new Dictionary<string, string>();

            foreach (string rule in input.Skip(2)) {
                string[] r = rule.Split(" -> ");
                map[r[0]] = r[1];
            }

            Part1(map, start);
        }

        static void Part1(Dictionary<string, string> map, string template) {
            // We are looking at the first 10 steps
            for (int step=0; step<10; step++) {

                // Split the current template string into a character array
                char[] current = template.ToArray();

                // Iterate through the array backwards and insert any characters
                // which match in the map to the template string.
                // NB if we step forwards in the array then we can't
                // insert to the template correctly.
                for (int i=current.Length-2;i>=0; i--) {
                    string check = string.Concat(current[i], current[i+1]);
                    if (map.ContainsKey(check)) {
                        template = template.Insert(i+1, map[check]);
                    }
                }
            }

            // Group the characters in the template by frequency
            var counts = template.ToArray().GroupBy(b=>b).OrderBy(c=>c.Count()).Reverse();
            var mostCommon = counts.First().Count();
            var leastCommon = counts.Last().Count();
            
            // Part 1 Answer: Template Length=19457, Most Common - Least Common = 3284.
            Console.WriteLine($"Part 1 Answer: Template Length={template.Length}, Most Common - Least Common = {mostCommon - leastCommon}.");
        }
    }
}
