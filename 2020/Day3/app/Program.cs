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
            long s1 = RunScenario(1,1);
            long s2 = RunScenario(3,1);
            long s3 = RunScenario(5,1);
            long s4 = RunScenario(7,1);
            long s5 = RunScenario(1,2);
            Console.WriteLine("Result: {0} * {1} * {2} * {3} * {4} = {5}", s1,s2,s3,s4,s5,s1*s2*s3*s4*s5);
        }

        static int RunScenario(int nAcross, int nDown) {
            int currentPos = 0;
            int numTrees = 0;

            foreach (string line in ParseInput(nDown)) {
                if (line[currentPos] == '#') {
                    numTrees++;
                }

                int newPos = currentPos + nAcross;
                if (newPos >= line.Length) {
                    newPos = 0 + (newPos - line.Length);
                }

                // WriteLineToConsole(line, currentPos, newPos, numTrees);
                
                currentPos = newPos;
            }
            return numTrees;
        }

        static IEnumerable<string> ParseInput(int nDown) {
            string[] lines = File.ReadAllLines(@"input.txt");

            for (int i=0; i<lines.Length; i+=nDown) {
                yield return lines[i];
            }
        }
    }
}
