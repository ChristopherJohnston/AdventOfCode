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
            Dictionary<string, Dictionary<string, int>> rules = new Dictionary<string, Dictionary<string, int>>();

            foreach (string rule in ParseInput()) {
                string[] r = rule.Split(" bags contain ");
                Match c = Regex.Match(r[1], @"(?:([0-9]+ [a-zA-Z ]*) bag(s)?[,.][ ]?)*");
                Dictionary<string, int> contents = new Dictionary<string, int>();
                foreach (Capture capture in c.Groups[1].Captures)
                {
                    if (capture.Value == string.Empty) {
                        // "Contains no bags"
                        break;
                    }
                    string[] keyValue = capture.Value.Split(' ', 2);
                    contents[keyValue[1]] = int.Parse(keyValue[0]);
                }
                rules[r[0]] = contents;
            }

            string bagColour = "shiny gold";
            Console.WriteLine(rules.Keys.Count((colour) => CanContainBagColour(rules, colour, bagColour)));

            Console.WriteLine(CountBagsForBagColour(rules, bagColour));
        }

        static bool CanContainBagColour(Dictionary<string, Dictionary<string, int>> rules, string colour, string bagColour) {
            if (rules[colour].ContainsKey(bagColour)) {
                return true;
            }
            
            foreach (KeyValuePair<string, int> p in rules[colour]) {
                if (CanContainBagColour(rules, p.Key, bagColour)) {
                    return true;
                }
            }

            return false;
        }

        static int CountBagsForBagColour(Dictionary<string, Dictionary<string, int>> rules, string bagColour) {
            int count = 0;
            foreach (KeyValuePair<string, int> p in rules[bagColour]) {
                count += p.Value * (1+CountBagsForBagColour(rules, p.Key));
            }
            return count;
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
