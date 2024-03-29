﻿using System;
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
            int east = 0;
            int north = 0;
            int waypointEast = 10;
            int waypointNorth = 1;

            foreach (string instruction in ParseInput()) {
                Match match = Regex.Match(instruction, @"^([A-Z])([0-9]+)");
                string action = match.Groups[1].Value;
                int value = int.Parse(match.Groups[2].Value);
                int e = waypointEast;

                switch (action) {
                    case "N":
                        waypointNorth += value;
                        break;                        
                    case "S":
                        waypointNorth -= value;
                        break;
                    case "E":
                        waypointEast += value;
                        break;
                    case "W":
                        waypointEast -= value;
                        break;
                    case "L":
                        switch (value) {
                            case 90:
                                waypointEast = -waypointNorth;
                                waypointNorth = e;
                                break;
                            case 180:
                                waypointEast *= -1;
                                waypointNorth *= -1;                                
                                break;
                            case 270:
                                waypointEast = waypointNorth;
                                waypointNorth = -e;                            
                                break;
                            case 360:
                                break;
                            default:
                                Console.WriteLine("Unknown direction {0}", value);
                                break;
                        }
                        break;
                    case "R":
                        switch (value) {
                            case 90:
                                waypointEast = waypointNorth;
                                waypointNorth = -e;
                                break;
                            case 180:
                                waypointEast *= -1;
                                waypointNorth *= -1;                                
                                break;
                            case 270:
                                waypointEast = -waypointNorth;
                                waypointNorth = e;
                                break;
                            case 360:
                                break;
                            default:
                                Console.WriteLine("Unknown direction {0}", value);
                                break;
                        }
                        break;
                    case "F":
                        east += (waypointEast * value);
                        north += (waypointNorth * value);
                        break;
                    default:
                        Console.WriteLine("Unknown action {0}", action);
                        break;
                }
            }

            Console.WriteLine("East {0}, North {1} = {2}", east, north, Math.Abs(east)+Math.Abs(north));
         }

        static IEnumerable<string> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt"); // = Part1: 858, Part2: 39140
            // string[] lines = new string[] { "F10", "N3", "F7", "R90", "F11" }; // = Part1: 25, Part2: 286
            foreach (string line in lines) {
                yield return line;
            }            
        }
    }
}
