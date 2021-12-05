using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace app
{
    /*
    --- Day 5: Hydrothermal Venture ---

    You come across a field of hydrothermal vents on the ocean floor! These vents constantly produce large, opaque clouds, so it would be best to avoid them if possible.

    They tend to form in lines; the submarine helpfully produces a list of nearby lines of vents (your puzzle input) for you to review. For example:

    0,9 -> 5,9
    8,0 -> 0,8
    9,4 -> 3,4
    2,2 -> 2,1
    7,0 -> 7,4
    6,4 -> 2,0
    0,9 -> 2,9
    3,4 -> 1,4
    0,0 -> 8,8
    5,5 -> 8,2
    Each line of vents is given as a line segment in the format x1,y1 -> x2,y2 where x1,y1 are the coordinates of one end the line segment and x2,y2 are the coordinates of the other end. These line segments include the points at both ends. In other words:

    An entry like 1,1 -> 1,3 covers points 1,1, 1,2, and 1,3.
    An entry like 9,7 -> 7,7 covers points 9,7, 8,7, and 7,7.
    For now, only consider horizontal and vertical lines: lines where either x1 = x2 or y1 = y2.

    So, the horizontal and vertical lines from the above list would produce the following diagram:

    .......1..
    ..1....1..
    ..1....1..
    .......1..
    .112111211
    ..........
    ..........
    ..........
    ..........
    222111....
    In this diagram, the top left corner is 0,0 and the bottom right corner is 9,9. Each position is shown as the number of lines which cover that point or . if no line covers that point. The top-left pair of 1s, for example, comes from 2,2 -> 2,1; the very bottom row is formed by the overlapping lines 0,9 -> 5,9 and 0,9 -> 2,9.

    To avoid the most dangerous areas, you need to determine the number of points where at least two lines overlap. In the above example, this is anywhere in the diagram with a 2 or larger - a total of 5 points.

    Consider only horizontal and vertical lines. At how many points do at least two lines overlap?

    --- Part Two ---

    Unfortunately, considering only horizontal and vertical lines doesn't give you the full picture; you need to also consider diagonal lines.

    Because of the limits of the hydrothermal vent mapping system, the lines in your list will only ever be horizontal, vertical, or a diagonal line at exactly 45 degrees. In other words:

    An entry like 1,1 -> 3,3 covers points 1,1, 2,2, and 3,3.
    An entry like 9,7 -> 7,9 covers points 9,7, 8,8, and 7,9.
    Considering all lines from the above example would now produce the following diagram:

    1.1....11.
    .111...2..
    ..2.1.111.
    ...1.2.2..
    .112313211
    ...1.2....
    ..1...1...
    .1.....1..
    1.......1.
    222111....
    You still need to determine the number of points where at least two lines overlap. In the above example, this is still anywhere in the diagram with a 2 or larger - now a total of 12 points.

    Consider all of the lines. At how many points do at least two lines overlap?
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

            int part1Answer = GetOverlaps(input, false);
            // Part1 Answer: 7318
            Console.WriteLine($"Part1 Answer: {part1Answer}");

            int part2Answer = GetOverlaps(input, true);
            // Part2 Answer: 7318
            Console.WriteLine($"Part2 Answer: {part2Answer}");
        }


        static int GetOverlaps(List<string> input, bool includeDiagonals) {
            Dictionary<(int x, int y), int> d = new Dictionary<(int x, int y), int>();

            foreach (string segment in input) {
                Match match = Regex.Match(segment, @"(\d+),(\d+) -> (\d+),(\d+)");

                int x1 = Int32.Parse(match.Groups[1].Value);
                int y1 = Int32.Parse(match.Groups[2].Value);
                int x2 = Int32.Parse(match.Groups[3].Value);
                int y2 = Int32.Parse(match.Groups[4].Value);

                if (x1==x2) {
                    // horizontal
                    int minY = Math.Min(y1, y2);
                    int maxY = Math.Max(y1, y2);

                    for (int y=minY; y<=maxY; y++) {
                        d[(x1, y)] = (d.ContainsKey((x1, y))) ? d[(x1, y)] + 1 : 1;
                    }
                } else if (y1==y2) {
                    //vertical
                    int minX = Math.Min(x1,x2);
                    int maxX = Math.Max(x1, x2);

                    for (int x=minX; x<=maxX; x++) {
                        d[(x, y1)] = (d.ContainsKey((x, y1))) ? d[(x, y1)] + 1 : 1;
                    }
                } else if (includeDiagonals) {
                    // diagonal
                    if (x2 > x1 && y1 > y2) {
                        // x up, y down
                        int y = y1;
                        for (int x=x1; x<=x2; x++) {
                            d[(x, y)] = (d.ContainsKey((x, y))) ? d[(x, y)] + 1 : 1;
                            y--;
                        }                
                    } else if (x1 > x2 && y2 > y1) {
                        // x down, y up
                        int y = y1;
                        for (int x=x1; x>=x2; x--) {
                            d[(x, y)] = (d.ContainsKey((x, y))) ? d[(x, y)] + 1 : 1;
                            y++;
                        }
                    } else if (x2 > x1 && y2 > y1) {
                        // x up, y up
                        int y = y1;
                        for (int x=x1; x<=x2; x++) {
                            d[(x, y)] = (d.ContainsKey((x, y))) ? d[(x, y)] + 1 : 1;
                            y++;
                        }
                    } else if (x1 > x2 && y1 > y2) {
                        // x down, y down
                        int y = y1;
                        for (int x=x1; x>=x2; x--) {
                            d[(x, y)] = (d.ContainsKey((x, y))) ? d[(x, y)] + 1 : 1;
                            y--;
                        }
                    }
                } else {
                    Console.WriteLine("Line has not been recognised");
                }
            }

            Render(d);

            int multipleOverlaps = 0;
            foreach (var kv in d) {
                if (kv.Value >= 2) {
                    multipleOverlaps++;
                }
            }

            return multipleOverlaps;
        }

        public static void Render(Dictionary<(int x, int y), int> d) {
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;

            foreach (var k in d.Keys) {
                minX = Math.Min(minX, k.x);
                maxX = Math.Max(maxX, k.x);
                minY = Math.Min(minY, k.y);
                maxY = Math.Max(maxY, k.y);
            }

            for (int c=minY; c<=maxY; c++) {
                string row = string.Empty;
                for (int r=minX; r<=maxX; r++){
                    row += (d.ContainsKey((r,c))) ? d[(r,c)].ToString() : ".";
                }
                Console.WriteLine(row);
            }
        }
    }
}
