using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace app
{
    public static class StringExtensions {
        public static string Reversed(this String str) {
            char[] c = str.ToCharArray();
            c.Reverse();
            return new string(c);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, long> tops = new Dictionary<string, long>();
            Dictionary<string, long> bottoms = new Dictionary<string, long>();
            Dictionary<string, long> lefts = new Dictionary<string, long>();
            Dictionary<string, long> rights = new Dictionary<string, long>();

            foreach (string[] tile in ParseInput()) {
                long tileNumber = long.Parse(Regex.Match(tile[0], @"Tile ([0-9]+)").Groups[1].Value);
                tops[tile[1]] = tileNumber;
                bottoms[tile[^1]] = tileNumber;

                string left = string.Empty;
                string right = string.Empty;

                for (int i=1; i<tile.Length; i++) {
                    string row = tile[i];
                    left += row[0];
                    right += row[^1];
                }

                lefts[left] = tileNumber;
                rights[right] = tileNumber;
            }

            // Find Top-Bottom joins
            // Dictionary<long, long> topBottoms = new Dictionary<long, long>();
            // foreach (KeyValuePair<string, long> kv in tops) {
            //     if (bottoms.ContainsKey(kv.Key)) {
            //         topBottoms[kv.Value] = bottoms[kv.Key];
            //     } else if (bottoms.ContainsKey(kv.Key.Reversed())) {
            //         topBottoms[kv.Value] = bottoms[kv.Key.Reversed()];
            //     } else if (lefts.ContainsKey(kv.Key)) {
            //         topBottoms[kv.Value] = lefts[kv.Key];
            //     } else if (lefts.ContainsKey(kv.Key.Reversed())) {
            //         topBottoms[kv.Value] = lefts[kv.Key.Reversed()];
            //     } else if (rights.ContainsKey(kv.Key)) {
            //         topBottoms[kv.Value] = rights[kv.Key];
            //     } else if (rights.ContainsKey(kv.Key.Reversed())) {
            //         topBottoms[kv.Value] = rights[kv.Key.Reversed()];
            //     }
            // }

            Dictionary<long, long> bottomTops = new Dictionary<long, long>();
            foreach (KeyValuePair<string, long> kv in bottoms) {
                if (tops.ContainsKey(kv.Key)) {
                    bottomTops[kv.Value] = tops[kv.Key];
                } else if (tops.ContainsKey(kv.Key.Reversed())) {
                    bottomTops[kv.Value] = tops[kv.Key.Reversed()];
                } else if (lefts.ContainsKey(kv.Key)) {
                    bottomTops[kv.Value] = lefts[kv.Key];
                } else if (lefts.ContainsKey(kv.Key.Reversed())) {
                    bottomTops[kv.Value] = lefts[kv.Key.Reversed()];
                } else if (rights.ContainsKey(kv.Key)) {
                    bottomTops[kv.Value] = rights[kv.Key];
                } else if (rights.ContainsKey(kv.Key.Reversed())) {
                    bottomTops[kv.Value] = rights[kv.Key.Reversed()];
                }
            }

            Dictionary<long, long> leftRights = new Dictionary<long, long>();
            foreach (KeyValuePair<string, long> kv in lefts) {
                if (rights.ContainsKey(kv.Key)) {
                    leftRights[kv.Value] = rights[kv.Key];
                } else if (rights.ContainsKey(kv.Key.Reversed())) {
                    leftRights[kv.Value] = rights[kv.Key.Reversed()];
                } else if (tops.ContainsKey(kv.Key)) {
                    leftRights[kv.Value] = tops[kv.Key];
                } else if (tops.ContainsKey(kv.Key.Reversed())) {
                    leftRights[kv.Value] = tops[kv.Key.Reversed()];
                } else if (bottoms.ContainsKey(kv.Key)) {
                    leftRights[kv.Value] = bottoms[kv.Key];
                } else if (bottoms.ContainsKey(kv.Key.Reversed())) {
                    leftRights[kv.Value] = bottoms[kv.Key.Reversed()];
                }
            }

            // Dictionary<long, long> rightLefts = new Dictionary<long, long>();
            // foreach (KeyValuePair<string, long> kv in rights) {
            //     if (lefts.ContainsKey(kv.Key)) {
            //         rightLefts[kv.Value] = lefts[kv.Key];
            //     } else if (lefts.ContainsKey(kv.Key.Reversed())) {
            //         rightLefts[kv.Value] = lefts[kv.Key.Reversed()];
            //     } else if (tops.ContainsKey(kv.Key)) {
            //         rightLefts[kv.Value] = tops[kv.Key];
            //     } else if (tops.ContainsKey(kv.Key.Reversed())) {
            //         rightLefts[kv.Value] = tops[kv.Key.Reversed()];
            //     } else if (bottoms.ContainsKey(kv.Key)) {
            //         rightLefts[kv.Value] = bottoms[kv.Key];
            //     } else if (bottoms.ContainsKey(kv.Key.Reversed())) {
            //         rightLefts[kv.Value] = bottoms[kv.Key.Reversed()];
            //     }
            // }
        }

        static IEnumerable<string[]> ParseInput() {
            List<string> currentLine = new List<string>();
            foreach (string line in File.ReadAllLines(@"example.txt")) {
                if (line == string.Empty) {
                    yield return currentLine.ToArray();
                    currentLine = new List<string>();
                }
                else {
                    currentLine.Add(line);
                }
            }
        }
    }
}
