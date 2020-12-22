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
            Array.Reverse(c);
            return new string(c);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<long, string> tops = new Dictionary<long, string>();
            Dictionary<long, string> bottoms = new Dictionary<long, string>();
            Dictionary<long, string> lefts = new Dictionary<long, string>();
            Dictionary<long, string> rights = new Dictionary<long, string>();
            Dictionary<long, string[]> tiles = new Dictionary<long, string[]>();

            foreach (string[] tile in ParseInput()) {
                long tileNumber = long.Parse(Regex.Match(tile[0], @"Tile ([0-9]+)").Groups[1].Value);
                tiles[tileNumber] = tile.Skip(1).ToArray();

                tops[tileNumber] = tile[1];
                bottoms[tileNumber] = tile[^1];

                string left = string.Empty;
                string right = string.Empty;

                for (int i=1; i<tile.Length; i++) {
                    string row = tile[i];
                    left += row[0];
                    right += row[^1];
                }

                lefts[tileNumber] = left;
                rights[tileNumber] = right;
            }

            Dictionary<long, Dictionary<string, long>> map = new Dictionary<long, Dictionary<string, long>>();

            var edges = new Dictionary<string, Dictionary<long, string>>();
            edges["Below"] = tops;
            edges["Above"] = bottoms;
            edges["Left"] = lefts;
            edges["Right"] = rights;

            // Each tile
            foreach (KeyValuePair<long, string[]> tile in tiles) {
                map[tile.Key] = new Dictionary<string, long>();

                // Look at each of its edges
                foreach (var kv in edges) {
                    var tileEdge = kv.Value[tile.Key];

                    // Compare with every other tile
                    foreach (KeyValuePair<long, string[]> other in tiles) {
                        if (other.Key == tile.Key) {
                            continue;
                        }

                        // Look at each other tile's edge
                        foreach (var kv2 in edges) {
                            var otherEdge = kv2.Value[other.Key];

                            if (tileEdge == otherEdge || tileEdge.Reversed() == otherEdge.Reversed() || tileEdge == otherEdge.Reversed()) {
                                map[tile.Key][kv.Key] = other.Key;
                            }
                        }
                    }
                }
            }

            var corners = map.Where(m=>m.Value.Count == 2).Select(kv => kv.Key).ToList();
            Console.WriteLine(corners.Aggregate((long)1, (a,v) => a*v));
        }

        static IEnumerable<string[]> ParseInput() {
            List<string> currentLine = new List<string>();
            foreach (string line in File.ReadAllLines(@"input.txt")) {
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
