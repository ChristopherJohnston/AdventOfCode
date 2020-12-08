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
            string[] program = ParseInput().ToArray();

            int i = 0;
            bool programFinished = false;
            int acc = 0;
            List<int> linesExecuted = new List<int>();

            while (!programFinished) {
                string[] currentLine = program[i].Split(' ');
                string instruction = currentLine[0];
                int argument = int.Parse(currentLine[1]);

                if (linesExecuted.Contains(i)) {
                    programFinished = true;
                    break;                       
                }

                linesExecuted.Add(i);
                
                switch (instruction) {
                    case "acc":
                        acc += argument;
                        i++;
                        break;
                    case "jmp":
                        i += argument;
                        break;
                    case "nop":
                        i++;
                        break;
                    default:
                        Console.WriteLine("Error! {0} is not a valid instruction", instruction);
                        break;
                }            
            }

            Console.WriteLine(acc);
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
