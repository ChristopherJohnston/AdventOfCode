using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
                int x = 0;
                int y = 0;
                foreach (string direction in directions) {
                    var d = GetDelta(direction);
                    x += d.x;
                    y += d.y;                    
                }
                floor[(x,y)] = (floor.ContainsKey((x,y)) && floor[(x,y)]) ? false : true;
            }

            Console.WriteLine(floor.Count(kv => kv.Value));

            // Each day the tiles affected will increase by 1 in y (n/s) and 2 in x (e/w)
            long minX = 0;
            long maxX = 0;
            long minY = 0;
            long maxY = 0;

            // first determine current dimensions
            foreach (var key in floor.Keys) {
                minX = Math.Min(minX, key.x);
                minY = Math.Min(minY, key.y);
                maxX = Math.Max(maxX, key.x);
                maxY = Math.Max(maxY, key.y);
            }

            for (int day=0; day<100; day++) {
                Dictionary<(long x, long y), bool> newFloor = new Dictionary<(long x, long y), bool>();

                minX -= 2;
                minY -= 1;
                maxX += 2;
                maxY += 1;

                // go through each tile
                for (long x=minX; x<=maxX; x++) {
                    for (long y=minY; y<=maxY; y++) {
                        int blackTileCount = 0;

                        // look in the 6 directions and count black tiles
                        foreach (string direction in new[] { "nw", "ne", "e", "se", "sw", "w"}) {
                            var d = GetDelta(direction);

                            bool tileColour = false;
                            floor.TryGetValue((x + d.x, y + d.y), out tileColour);
                            if (tileColour)
                                blackTileCount++;
                        }

                        bool currentTile = false;
                        floor.TryGetValue((x, y), out currentTile);
                        if (currentTile)
                            newFloor[(x,y)] = (blackTileCount == 0 || blackTileCount > 2) ? false : true;
                        else                        
                            newFloor[(x,y)] = (blackTileCount == 2) ? true : false;
                    }
                }

                floor = newFloor;
                Console.WriteLine("Day {0}: {1}", day+1, floor.Count(kv => kv.Value));
            }

            Console.WriteLine(floor.Count(kv => kv.Value));
        }

        static (int x, int y) GetDelta(string direction) {
            // As the grid is hexaganol, single east and west movements move +2 in the direction:
            //
            // =============================
            // | X |   | X |   | X |   | X |
            // -----------------------------
            // |   | X |   | X |   | X |   |
            // -----------------------------
            // | X |   | NW|   | NE|   | X |
            // -----------------------------
            // |   | W |   | C |   | E |   |
            // -----------------------------
            // | X |   | SW|   | SE|   | X |
            // -----------------------------
            // |   | X |   | X |   | X |   |
            // -----------------------------
            // | X |   | X |   | X |   | X |
            // =============================
            //
            // This is a doubled coordinates system. See https://www.redblobgames.com/grids/hexagons/

            int dx = 0;
            int dy = 0;
            switch (direction) {
                case "nw":
                    dx = -1;
                    dy = 1;
                    break;
                case "ne":
                    dx = 1;
                    dy = 1;
                    break;
                case "e":
                    dx = 2;
                    break;
                case "se":
                    dx = 1;
                    dy = -1;
                    break;
                case "sw":
                    dx = -1;
                    dy = -1;
                    break;
                case "w":
                    dx = -2;
                    break;
            }
            return (dx,dy);
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
