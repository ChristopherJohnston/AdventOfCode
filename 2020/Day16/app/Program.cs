using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

struct Notes {
    public Dictionary<string, string> Rules;
    public int[] MyTicket;

    public List<int[]> NearbyTickets;
}

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Notes n = ParseInput();
            List<int[]> validTickets = Part1(n);
            Part2(n, validTickets);
        }

        static void Part2(Notes n, List<int[]> validTickets) {
            validTickets.Add(n.MyTicket);
            Dictionary<string, List<int>> fieldCandidates = new Dictionary<string, List<int>>();

            foreach (KeyValuePair<string, string> kv in n.Rules) {
                Match m = Regex.Match(kv.Value, @"(\d+)-(\d+) or (\d+)-(\d+)");
                int[] rule = m.Groups.Values.Skip(1).Select((g) => int.Parse(g.Value)).ToArray();

                for (int i=0; i<n.Rules.Count; i++) {
                    if (validTickets.All((t) => ((t[i] >= rule[0] && t[i] <= rule[1]) || (t[i] >= rule[2] && t[i] <= rule[3])))) {
                        if (fieldCandidates.ContainsKey(kv.Key)) {
                            fieldCandidates[kv.Key].Add(i);
                        } else {
                            fieldCandidates[kv.Key] = new List<int> { i };
                        }
                    }
                }
            }

            string[] fields = new string[n.Rules.Count];

            // Solve the indices by finding the fields that only have one index and removing that index
            // from the rest of the fields until there are none left to find.
            while (fields.Any((v) => v == null)) {
                foreach (KeyValuePair<string, List<int>> fc in fieldCandidates) {
                    if (fc.Value.Count == 1) {
                        fields[fc.Value[0]] = fc.Key;

                        // Remove the index from the rest of the field candidates
                        foreach (KeyValuePair<string, List<int>> fc2 in fieldCandidates) {
                            if (fc2.Key != fc.Key)
                                fc2.Value.Remove(fc.Value[0]);         
                        }
                        // Remove the candidate
                        fieldCandidates.Remove(fc.Key);
                    }
                }
            }

            List<long> departureFields = new List<long>();
            for (int i=0; i<fields.Length; i++) {
                if (fields[i].Contains("departure")) {
                    departureFields.Add((long)n.MyTicket[i]);
                }
            }
            Console.WriteLine(departureFields.Aggregate((long)1, (acc, val) => acc * val));
        }

        static List<int[]> Part1(Notes n) {
            HashSet<int> allValidNumbers = new HashSet<int>();
            foreach (string v in n.Rules.Values) {
                Match m = Regex.Match(v, @"(\d+)-(\d+) or (\d+)-(\d+)");
                int[] rule = m.Groups.Values.Skip(1).Select((g) => int.Parse(g.Value)).ToArray();


                for (int i=rule[0]; i<=rule[1]; i++) {
                    allValidNumbers.Add(i);
                }
                for (int i=rule[2]; i<=rule[3]; i++) {
                    allValidNumbers.Add(i);
                }
            }

            int errorRate = 0;
            List<int[]> validTickets = new List<int[]>();

            foreach (int[] ticket in n.NearbyTickets) {
                int errors = ticket.Where((num) => !allValidNumbers.Contains(num)).Sum();
                if (errors == 0)
                    validTickets.Add(ticket);

                errorRate += errors;
            }

            Console.WriteLine(errorRate);
            return validTickets;
        }

        static Notes ParseInput() {
            Notes n = new Notes();

            string[] input = File.ReadAllLines(@"input.txt").ToArray();

            n.Rules = new Dictionary<string, string>();

            int i=0;
            for (i=0; i<input.Length; i++) {
                if (input[i] == string.Empty)
                    break;

                string[] kv= input[i].Split(": ");
                n.Rules[kv[0]] = kv[1];
            }

            i+=2;
            n.MyTicket = input[i].Split(',').Select(int.Parse).ToArray();

            n.NearbyTickets = new List<int[]>();
            for (i=i+3; i<input.Length; i++) {
                n.NearbyTickets.Add(input[i].Split(',').Select(int.Parse).ToArray());
            }
            
            return n;
        }
    }
}
