using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void WriteLineToConsole(string line, int startLocation, int endLocation, int numTrees)
        {
            for (int i=0; i<line.Length; i++) {
                if ((endLocation>startLocation && i >= startLocation && i <= endLocation) || (startLocation>endLocation && (i <=endLocation || i>=startLocation))) {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    
                }
                Console.Write(line[i]);
            }

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;            
            Console.WriteLine(" = {0}-{1}, {2}", startLocation, endLocation, numTrees);
        }

        static void Main(string[] args)
        {
            int currentPos = 0;
            int numTrees = 0;

            string[] map = ParseInput().ToArray();
            int lineWidth = map[0].Length;

            foreach (string line in map) {
                if (line[currentPos] == '#') {
                    numTrees++;
                }

                int newPos = currentPos + 3;
                if (newPos >= lineWidth) {
                    newPos = 0 + (newPos - lineWidth);
                }

                WriteLineToConsole(line, currentPos, newPos, numTrees);
                
                currentPos = newPos;
            }
        }

        static IEnumerable<string> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt");

            foreach (string line in lines) {
                yield return line;
            }
        }
    }
}
