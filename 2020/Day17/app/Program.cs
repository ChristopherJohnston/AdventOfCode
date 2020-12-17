using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.
            Otherwise, the cube becomes inactive.


            If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active.
            Otherwise, the cube remains inactive.

            .#. .#. .#.
            ..# ..# ..#
            ### ### ###

            .#. .#. .#.
            ..# ..# ..#
            ### ### ###

            .#. .#. .#.
            ..# ..# ..#
            ### ### ###
            */
            Dictionary<(long,long,long), char> initialState = new Dictionary<(long, long, long), char>();
            string[] lines = File.ReadAllLines(@"input.txt");
            for (int y=0; y<lines.Length; y++) {
                for (int x=0; x<lines[y].Length; x++) {
                    initialState[(x,y,0)] = lines[y][x];
                }
            }

            Part1(initialState);
        }

        static void Part1(Dictionary<(long,long,long), char> state) {
            int minX = 0;
            int maxX = 7;
            int minY = 0;
            int maxY = 7;
            int minZ = 0;
            int maxZ = 0;

            for (int i=0; i<6; i++) {
                Dictionary<(long x,long y,long z), char> newState = new Dictionary<(long, long, long), char>();

                // Go through the existing items
                for (int x=minX-1; x<=maxX+1; x++) {
                    for (int y=minY-1; y<=maxY+1; y++) {
                        for (int z=minZ-1; z<=maxZ+1; z++) {
                            long activeNeighbours = 0;

                            // check around current item
                            for (int x1=x-1; x1<=x+1; x1++) {
                                for (int y1=y-1; y1<=y+1; y1++) {
                                    for (int z1=z-1; z1<=z+1; z1++) {
                                        if ((x,y,z) == (x1,y1,z1)) {
                                            // don't check the current item
                                            continue;
                                        }

                                        if (state.ContainsKey((x1,y1,z1)) && state[(x1,y1,z1)] == '#')
                                            activeNeighbours++;
                                    }
                                }
                            }

                            if (state.ContainsKey((x,y,z)) && state[(x,y,z)] == '#') {
                                newState[(x,y,z)] = (activeNeighbours == 2 || activeNeighbours == 3) ? '#' : '.';
                            } else {
                                newState[(x,y,z)] = (activeNeighbours == 3) ? '#': '.';
                            }
                        }
                    }
                }

                minX--;
                maxX++;
                minY--;
                maxY++;
                minZ--;
                maxZ++;
                state = newState;        
            }

            for (int z2=minZ; z2<=maxZ; z2++) {
                Console.WriteLine("Z={0}", z2);
                for (int y2=minY; y2<=maxY; y2++) {
                    for (int x2=minX; x2<=maxX; x2++ ) {
                        Console.Write(state[(x2,y2,z2)]);
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine(state.Values.Count((c) => c == '#'));
        }
    }
}
