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

            Part1(map);
        }

        static void Part1(int[][] map) {
            List<int> riskLevels = new List<int>();

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
                        riskLevels.Add(current+1);
                    }
                }
            }

            Console.WriteLine($"Part1: {riskLevels.Sum()}");
        }
    }
}
