using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace app
{
    /*
    --- Day 9: Smoke Basin ---

    These caves seem to be lava tubes. Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

    If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

    Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

    2199943210
    3987894921
    9856789892
    8767896789
    9899965678
    Each number corresponds to the height of a particular location, where 9 is the highest and 0 is the lowest a location can be.

    Your first goal is to find the low points - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

    In the above example, there are four low points, all highlighted: two are in the first row (a 1 and a 0), one is in the third row (a 5), and one is in the bottom row (also a 5). All other locations on the heightmap have some lower adjacent location, and so are not low points.

    The risk level of a low point is 1 plus its height. In the above example, the risk levels of the low points are 2, 1, 6, and 6. The sum of the risk levels of all low points in the heightmap is therefore 15.

    Find all of the low points on your heightmap. What is the sum of the risk levels of all low points on your heightmap?

    --- Part Two ---

    Next, you need to find the largest basins so you know what areas are most important to avoid.

    A basin is all locations that eventually flow downward to a single low point. Therefore, every low point has a basin, although some basins are very small. Locations of height 9 do not count as being in any basin, and all other locations will always be part of exactly one basin.

    The size of a basin is the number of locations within the basin, including the low point. The example above has four basins.

    The top-left basin, size 3:

    2199943210
    3987894921
    9856789892
    8767896789
    9899965678
    The top-right basin, size 9:

    2199943210
    3987894921
    9856789892
    8767896789
    9899965678
    The middle basin, size 14:

    2199943210
    3987894921
    9856789892
    8767896789
    9899965678
    The bottom-right basin, size 9:

    2199943210
    3987894921
    9856789892
    8767896789
    9899965678
    Find the three largest basins and multiply their sizes together. In the above example, this is 9 * 14 * 9 = 1134.

    What do you get if you multiply together the sizes of the three largest basins?
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

            int[][] map = new int[input.Count][];

            for(int i=0; i<input.Count; i++) {
                map[i] = input[i].Select(n => Int32.Parse(n.ToString())).ToArray();
            }

            Dictionary<(int r, int c), int> lowPoints = Part1(map);

            Part2(map, lowPoints);
        }

        static Dictionary<(int r, int c), int> Part1(int[][] map) {
            Dictionary<(int r, int c), int> lowPoints = new Dictionary<(int r, int c), int>();

            for (int r=0; r<map.Length; r++) {                
                for (int c=0; c<map[r].Length; c++) {
                    int current = map[r][c];
                    List<int> others = new List<int>();

                    if (r>0) {
                        others.Add(map[r-1][c]);
                    }

                    if (r<map.Length-1) {
                        others.Add(map[r+1][c]);
                    }

                    if (c>0) {
                        others.Add(map[r][c-1]);
                    }

                    if (c<map[r].Length-1) {
                        others.Add(map[r][c+1]);
                    }

                    if (others.All(o => o > current)) {
                        lowPoints[(r,c)] = current;
                    }
                }
            }

            // Part1: 603
            Console.WriteLine($"Part1: {lowPoints.Values.Sum() + lowPoints.Count}");
            return lowPoints;
        }

        static void Part2(int[][] map, Dictionary<(int r, int c), int> lowPoints) {
            List<long> basinSizes = new List<long>();
            foreach ((int r, int c) lowPoint in lowPoints.Keys) {
                basinSizes.Add(CountLocations(map, new List<(int r, int c)>(), lowPoint, lowPoint));
            }

            // The answer is the product of the three largest basins
            long answer = basinSizes.OrderByDescending(n=>n).Take(3).Aggregate((long)1, (acc, val) => acc * val);

            // Part 2: 786780                   
            Console.WriteLine($"Part 2: {answer}");
        }

        static long CountLocations(int[][] map, List<(int r, int c)> seen, (int r, int c) previous, (int r, int c) current) {
            int currentValue = map[current.r][current.c];

            // If we've seen the location, it's height is lower than the previous height or its height is 9 then stop the recursion.
            if (currentValue == 9 || currentValue < map[previous.r][previous.c] || seen.Contains(current)) {
                return 0;
            }

            long count = 1;
            seen.Add(current);
            
            // Look left from the current location as long as we're not on the far left
            if (current.c > 0) {
                count += CountLocations(map, seen, current, (current.r, current.c-1));
            }

            // Look right from the current location as long as we're not on the far right
            if (current.c < map[current.r].Length-1) {
                count += CountLocations(map, seen, current, (current.r, current.c+1));
            }

            // Look up from the current location as long as we're not at the top
            if (current.r > 0) {
                count += CountLocations(map, seen, current, (current.r-1, current.c));
            }

            // Look down from the current location as long as we're not at the bottom
            if (current.r < map.Length-1) {
                count += CountLocations(map, seen, current, (current.r+1, current.c));
            }

            return count;
        }
    }
}
