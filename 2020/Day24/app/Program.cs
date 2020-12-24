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
            Dictionary<(long x, long y), bool> floor = new Dictionary<(long x, long y), bool>();

            // Determine each tile's moves
            List<List<string>> tileMoves = new List<List<string>>();
            foreach (string line in ParseInput()) {
                List<string> directions = new List<string>();
                string currentDirection = string.Empty;
                foreach (char c in line) {
                    switch (c) {
                        case 'n':
                        case 's':
                            if (currentDirection == string.Empty) {
                                currentDirection += c;
                            }
                            else {
                                directions.Add(currentDirection);
                                currentDirection = c.ToString();
                            }
                            break;
                        case 'e':
                        case 'w':
                            if (currentDirection == "n" || currentDirection == "s" || currentDirection == string.Empty) {
                                    currentDirection += c;
                            }
                            else {
                                directions.Add(currentDirection);
                                currentDirection = c.ToString();
                            }
                            break;                
                    }
                }
                directions.Add(currentDirection);
                
                // Iterate through the tile's movements and determine the x,y location.
                // as it's hexaganol, single east and west movements move +2 in the direction:
                //
                // =============================
                // |   |   |   |   |   |   |   |
                // -----------------------------
                // |   |   |   |   |   |   |   |
                // -----------------------------
                // |   |   | NW|   | NE|   |   |
                // -----------------------------
                // |   | W |   | C |   | E |   |
                // -----------------------------
                // |   |   | SW|   | SE|   |   |
                // -----------------------------
                // |   |   |   |   |   |   |   |
                // -----------------------------
                // |   |   |   |   |   |   |   |
                // =============================
                //

                int x = 0;
                int y = 0;
                foreach (string direction in directions) {
                    switch (direction) {
                        case "nw":
                            x--;
                            y++;
                            break;
                        case "ne":
                            x++;
                            y++;
                            break;
                        case "e":
                            x+=2;
                            break;
                        case "se":
                            x++;
                            y--;
                            break;
                        case "sw":
                            x--;
                            y--;
                            break;
                        case "w":
                            x-=2;
                            break;
                    }                    
                }
                floor[(x,y)] = (floor.ContainsKey((x,y)) && floor[(x,y)]) ? false : true;
            }

            Console.WriteLine(floor.Count(kv => kv.Value));
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
