using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace app
{
    /*
    --- Day 13: Transparent Origami ---

    You reach another volcanically active part of the cave. It would be nice if you could do some kind of thermal imaging so you could tell ahead of time which caves are too hot to safely enter.

    Fortunately, the submarine seems to be equipped with a thermal camera! When you activate it, you are greeted with:

    Congratulations on your purchase! To activate this infrared thermal imaging
    camera system, please enter the code found on page 1 of the manual.

    Apparently, the Elves have never used this feature. To your surprise, you manage to find the manual; as you go to open it, page 1 falls out. It's a large sheet of transparent paper! The transparent paper is marked with random dots and includes instructions on how to fold it up (your puzzle input). For example:

    6,10
    0,14
    9,10
    0,3
    10,4
    4,11
    6,0
    6,12
    4,1
    0,13
    10,12
    3,4
    3,0
    8,4
    1,10
    2,14
    8,10
    9,0

    fold along y=7
    fold along x=5
    The first section is a list of dots on the transparent paper. 0,0 represents the top-left coordinate. The first value, x, increases to the right. The second value, y, increases downward. So, the coordinate 3,0 is to the right of 0,0, and the coordinate 0,7 is below 0,0. The coordinates in this example form the following pattern, where # is a dot on the paper and . is an empty, unmarked position:

    ...#..#..#.
    ....#......
    ...........
    #..........
    ...#....#.#
    ...........
    ...........
    ...........
    ...........
    ...........
    .#....#.##.
    ....#......
    ......#...#
    #..........
    #.#........
    Then, there is a list of fold instructions. Each instruction indicates a line on the transparent paper and wants you to fold the paper up (for horizontal y=... lines) or left (for vertical x=... lines). In this example, the first fold instruction is fold along y=7, which designates the line formed by all of the positions where y is 7 (marked here with -):

    ...#..#..#.
    ....#......
    ...........
    #..........
    ...#....#.#
    ...........
    ...........
    -----------
    ...........
    ...........
    .#....#.##.
    ....#......
    ......#...#
    #..........
    #.#........
    Because this is a horizontal line, fold the bottom half up. Some of the dots might end up overlapping after the fold is complete, but dots will never appear exactly on a fold line. The result of doing this fold looks like this:

    #.##..#..#.
    #...#......
    ......#...#
    #...#......
    .#.#..#.###
    ...........
    ...........
    Now, only 17 dots are visible.

    Notice, for example, the two dots in the bottom left corner before the transparent paper is folded; after the fold is complete, those dots appear in the top left corner (at 0,0 and 0,1). Because the paper is transparent, the dot just below them in the result (at 0,3) remains visible, as it can be seen through the transparent paper.

    Also notice that some dots can end up overlapping; in this case, the dots merge together and become a single dot.

    The second fold instruction is fold along x=5, which indicates this line:

    #.##.|#..#.
    #...#|.....
    .....|#...#
    #...#|.....
    .#.#.|#.###
    .....|.....
    .....|.....
    Because this is a vertical line, fold left:

    #####
    #...#
    #...#
    #...#
    #####
    .....
    .....
    The instructions made a square!

    The transparent paper is pretty big, so for now, focus on just completing the first fold. After the first fold in the example above, 17 dots are visible - dots that end up overlapping after the fold is completed count as a single dot.

    How many dots are visible after completing just the first fold instruction on your transparent paper?

    --- Part Two ---

    Finish folding the transparent paper according to the instructions. The manual says the code is always eight capital letters.

    What code do you use to activate the infrared thermal imaging camera system?
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

            List<(int x, int y)> map = new List<(int x, int y)>();
            List<(string axis, int position)> folds = new List<(string axis, int position)>();        

            foreach (string line in input) {
                if (line == "") {
                    continue;
                }
                else if (line.StartsWith("fold")) {
                    var fold = line.Replace("fold along ", "").Split('=');
                    folds.Add((fold[0], Int32.Parse(fold[1])));
                }
                else {
                    var coords = line.SplitToList<int>(',').ToArray();
                    map.Add((coords[0], coords[1]));
                }
            }

            Part1(new List<(int x, int y)>(map), folds);
            Part2(new List<(int x, int y)>(map), folds);
        }

        static void Part1(List<(int x, int y)> map, List<(string axis, int position)> folds) {
            // Take the first fold
            var fold = folds.First();
            var newMap = Fold(map, fold.axis, fold.position);

            // Part 1 Answer: 747
            Console.WriteLine($"Part 1 Answer: {newMap.Count()}");
        }

        static void Part2(List<(int x, int y)> map, List<(string axis, int position)> folds) {
            // Go through all the folds
            foreach ((string axis, int position) fold in folds) {
                map = Fold(map, fold.axis, fold.position);
            }

            // Find the upper and lower bounds of the map
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach ((int x, int y) coordinate in map) {
                minX = Math.Min(minX, coordinate.x);
                maxX = Math.Max(maxX, coordinate.x);
                minY = Math.Min(minY, coordinate.y);
                maxY = Math.Max(maxY, coordinate.y);
            }

            /*
            Part 2 Answer:
            .##..###..#..#.####.###...##..#..#.#..#
            #..#.#..#.#..#....#.#..#.#..#.#..#.#..#
            #..#.#..#.####...#..#..#.#....#..#.####
            ####.###..#..#..#...###..#....#..#.#..#
            #..#.#.#..#..#.#....#....#..#.#..#.#..#
            #..#.#..#.#..#.####.#.....##...##..#..#

            ARZHPCUH
            */
            Console.WriteLine("Part 2 Answer:");
            
            // Go through the map and print out the characters
            for (int y=minY; y<=maxY; y++) {
                for (int x=minX; x<=maxX; x++) {                
                    string val = (map.Contains((x,y))) ?  "#" : ".";
                    Console.Write(val);
                }

                Console.WriteLine("");
            }            
        }

        static List<(int x, int y)> Fold(List<(int x, int y)> map, string axis, int position) {
            Dictionary<(int x, int y), (int x, int y)> toFold = new Dictionary<(int x, int y), (int x, int y)>();

            // Go through each item and determine its new position if it is greater than the fold position
            foreach ((int x, int y) coordinate in map) {
                if ( (axis == "y" && coordinate.y > position) || (axis == "x" && coordinate.x > position) ) {
                    (int x, int y) newCoordinate = (axis == "y") ? (coordinate.x, position - (coordinate.y-position)) : (position - (coordinate.x-position), coordinate.y);
                    toFold[coordinate] = newCoordinate;
                }
            }

            // Go through each fold and re-position it on the map
            foreach (var f in toFold)
            {
                map.Remove(f.Key);

                if (!map.Contains(f.Value)) {
                    map.Add(f.Value);
                }
            }

            return map;
        }
    }
}
