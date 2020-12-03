using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void WriteLineToConsole(string line, int startLocation, int nAcross, int numTrees)
        {
            int endLocation = (startLocation+nAcross+line.Length) % line.Length;

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
            var result = new[] { (1,1), (3,1), (5,1), (7,1), (1,2)}
                .Select((n) => RunScenario(n.Item1, n.Item2))
                .Aggregate((long)1, (p, n) => p*n);

            Console.WriteLine("Result: {0}", result);
        }

        static int RunScenario(int nAcross, int nDown) {
            int currentPos = 0;
            int numTrees = 0;

            foreach (string line in ParseInput(nDown)) {
                if (line[currentPos] == '#') {
                    numTrees++;
                }            

                // WriteLineToConsole(line, currentPos, nAcross, numTrees);
                
                currentPos = (currentPos+nAcross+line.Length) % line.Length;
            }

            Console.WriteLine("({0},{1}) => {2}", nAcross, nDown, numTrees);
            
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
