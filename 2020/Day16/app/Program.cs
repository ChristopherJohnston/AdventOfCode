using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

struct Notes {
    public Dictionary<string, string> Rules;
    public IEnumerable<int> MyTicket;

    public List<int[]> NearbyTickets;
}

namespace app
{
    class Program
    {

        static void Main(string[] args)
        {
            Notes n = ParseInput();
            Part1(n);
        }

        static void Part1(Notes n) {
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

            foreach (IEnumerable<int> ticket in n.NearbyTickets) {
                errorRate += ticket.Where((num) => !allValidNumbers.Contains(num)).Sum();
            }

            Console.WriteLine(errorRate);
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
            n.MyTicket = input[i].Split(',').Select(int.Parse);

            n.NearbyTickets = new List<int[]>();
            for (i=i+3; i<input.Length; i++) {
                n.NearbyTickets.Add(input[i].Split(',').Select(int.Parse).ToArray());
            }
            
            return n;
        }
    }
}
