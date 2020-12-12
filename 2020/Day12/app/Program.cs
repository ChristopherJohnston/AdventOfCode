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
            int currentDirection = 90;
            int east = 0;
            int north = 0;

            foreach (string instruction in ParseInput()) {
                Match match = Regex.Match(instruction, @"^([A-Z])([0-9]+)");
                string action = match.Groups[1].Value;
                int value = int.Parse(match.Groups[2].Value);

                switch (action) {
                    case "N":
                        north += value;
                        break;                        
                    case "S":
                        north -= value;
                        break;
                    case "E":
                        east += value;
                        break;
                    case "W":
                        east -= value;
                        break;
                    case "L":
                        currentDirection -= value;
                        if (currentDirection<0) {
                            currentDirection = 360 + currentDirection;
                        }
                        break;
                    case "R":
                        currentDirection += value;
                        if (currentDirection > 359) {
                            currentDirection = currentDirection - 360;
                        }
                        break;
                    case "F":
                        switch (currentDirection) {
                            case 90:
                                east += value;
                                break;
                            case 180:
                                north -= value;
                                break;
                            case 270:
                                east -= value;
                                break;
                            case 0:
                                north += value;
                                break;
                            default:
                                Console.WriteLine("Unknown direction {0}", currentDirection);
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown action {0}", action);
                        break;
                }
            }

            Console.WriteLine("East {0}, North {1} = {2}", east, north, Math.Abs(east)+Math.Abs(north));
         }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
