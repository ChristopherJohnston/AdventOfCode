using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace app
{
    /*
    --- Day 14: Extended Polymerization ---

    The incredible pressures at this depth are starting to put a strain on your submarine. The submarine has polymerization equipment that would produce suitable materials to reinforce the submarine, and the nearby volcanically-active caves should even have the necessary input elements in sufficient quantities.

    The submarine manual contains instructions for finding the optimal polymer formula; specifically, it offers a polymer template and a list of pair insertion rules (your puzzle input). You just need to work out what polymer would result after repeating the pair insertion process a few times.

    For example:

    NNCB

    CH -> B
    HH -> N
    CB -> H
    NH -> C
    HB -> C
    HC -> B
    HN -> C
    NN -> C
    BH -> H
    NC -> B
    NB -> B
    BN -> B
    BB -> N
    BC -> B
    CC -> N
    CN -> C
    The first line is the polymer template - this is the starting point of the process.

    The following section defines the pair insertion rules. A rule like AB -> C means that when elements A and B are immediately adjacent, element C should be inserted between them. These insertions all happen simultaneously.

    So, starting with the polymer template NNCB, the first step simultaneously considers all three pairs:

    The first pair (NN) matches the rule NN -> C, so element C is inserted between the first N and the second N.
    The second pair (NC) matches the rule NC -> B, so element B is inserted between the N and the C.
    The third pair (CB) matches the rule CB -> H, so element H is inserted between the C and the B.
    Note that these pairs overlap: the second element of one pair is the first element of the next pair. Also, because all pairs are considered simultaneously, inserted elements are not considered to be part of a pair until the next step.

    After the first step of this process, the polymer becomes NCNBCHB.

    Here are the results of a few steps using the above rules:

    Template:     NNCB
    After step 1: NCNBCHB
    After step 2: NBCCNBBBCBHCB
    After step 3: NBBBCNCCNBBNBNBBCHBHHBCHB
    After step 4: NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB
    This polymer grows quickly. After step 5, it has length 97; After step 10, it has length 3073. After step 10, B occurs 1749 times, C occurs 298 times, H occurs 161 times, and N occurs 865 times; taking the quantity of the most common element (B, 1749) and subtracting the quantity of the least common element (H, 161) produces 1749 - 161 = 1588.

    Apply 10 steps of pair insertion to the polymer template and find the most and least common elements in the result. What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?

    --- Part Two ---

    The resulting polymer isn't nearly strong enough to reinforce the submarine. You'll need to run more steps of the pair insertion process; a total of 40 steps should do it.

    In the above example, the most common element is B (occurring 2192039569602 times) and the least common element is H (occurring 3849876073 times); subtracting these produces 2188189693529.

    Apply 40 steps of pair insertion to the polymer template and find the most and least common elements in the result. What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?
    */    
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

            // Part 1 Answer: Template Length=19457, Most Common - Least Common = 3284.
            Part1(map, start, 10);
            // Part 2 Answer after 10 steps: 3284
            Part2(map, start, 10);

            // Part 2 Answer after 40 steps: 4302675529689
            Part2(map, start, 40);
        }

        static void Part1(Dictionary<string, string> map, string template, int steps) {
            // We are looking at the first 10 steps
            for (int step=0; step<steps; step++) {
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
            
            Console.WriteLine($"Part 1 Answer: Template Length={template.Length}, Most Common - Least Common = {mostCommon - leastCommon}.");
        }

        /*
        Thank you, /r/adventofcode!
        */
        static void Part2(Dictionary<string, string> map, string template, int steps) {
            // Create a dictionary of pairs and their count
            Dictionary<string, long> pairs = new Dictionary<string, long>();

            for (int i=template.Length-2;i>=0; i--) {
                string pair = string.Concat(template[i], template[i+1]);
                if (pairs.ContainsKey(pair)) {
                    pairs[pair]++;
                }
                else {
                    pairs[pair] = 1;
                }                
            }

            // Go through the required number of steps
            for (int step=0; step<steps; step++) {
                Dictionary<string, long> newPairs = new Dictionary<string, long>();

                // go through all of the pairs and insert from the map
                foreach ((string pair, long count) in pairs) {
                    if (map.ContainsKey(pair)) {
                        // each pair in the list of pairs is split and the counts of their
                        // resultant pair added.
                        // e.g. for HC-> B....add to count for HB nd BC
                        string p1 = string.Concat(pair[0], map[pair]);
                        newPairs[p1] = (newPairs.ContainsKey(p1)) ? newPairs[p1] + count : count;

                        string p2 = string.Concat(map[pair], pair[1]);
                        newPairs[p2] = (newPairs.ContainsKey(p2)) ? newPairs[p2] + count : count;
                    }
                }

                pairs = newPairs;
            }

            // Split the pairs into individual characters and their counts
            var individualChars = pairs.SelectMany(x=>{
                return new (char element, long count)[] {
                    (x.Key[0], x.Value),
                    (x.Key[1], x.Value)
                };
            })
            .ToList()
            .GroupBy(b=>b.element);

            // Order the characters by their count
            var counts = individualChars.OrderBy(g=> g.Sum(e=>e.count)).Reverse();

            // Get the most common and least common - divide by 2 because by splitting the
            // pairs into individual characters we've doubled up their counts.
            double mostCommon = counts.First().Sum(k=>k.count) / 2.0;
            double leastCommon = counts.Last().Sum(k=>k.count) / 2.0;

            Console.WriteLine($"Part 2 Answer after {steps} steps: {(long)Math.Floor(mostCommon-leastCommon)}");
        }
    }
}