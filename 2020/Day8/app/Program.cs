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

            // Part 1
            GameConsole console = new GameConsole(program);
            console.Boot();            
            Console.WriteLine(console.acc);
            
            // Part 2
            int i = 0;
            string flippedInstruction = string.Empty;

            while (!console.programFinished) {
                // Revert any previously flipped instruction
                if (flippedInstruction != string.Empty) {
                    program[i] = flippedInstruction;
                    i++;
                }

                // Find the next instruction to flip
                bool altered = false;                
                while (!altered) {
                    flippedInstruction = program[i];
                    string instruction = flippedInstruction.Split(' ')[0];
                    if (instruction == "nop") {
                        program[i] = flippedInstruction.Replace("nop", "jmp");
                        altered = true;
                    }
                    else if (instruction == "jmp") {
                        program[i] = flippedInstruction.Replace("jmp", "nop");
                        altered = true;
                    }
                    else {
                        i++;
                    }
                }
                
                // Run the altered program
                console.Reset();
                console.program = program;
                console.Boot();
            }

            Console.WriteLine(console.acc);
        }
        
        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }    
    class GameConsole {
        public bool loopDetected { get; private set; } = false;
        public bool programFinished { get; private set; } = false;
        public int acc { get; private set; } = 0;

        public string[] program { get; set; }

        public int lineNumber { get; private set; } = 0;

        public List<int> linesExecuted { get; private set; }

        public GameConsole() {
            this.Reset();
        }

        public GameConsole(string[] program) : this() {
            this.program = program;
        }

        public void Reset() {
            this.loopDetected = false;
            this.programFinished = false;
            this.acc = 0;
            this.lineNumber = 0;
            this.program = null;
            this.linesExecuted = new List<int>();
        }

        public void Boot() {
            if (this.program == null) {
                throw new Exception("No program is loaded");
            }

            while (!this.programFinished) {
                string[] currentLine = this.program[this.lineNumber].Split(' ');
                string instruction = currentLine[0];
                int argument = int.Parse(currentLine[1]);

                if (this.linesExecuted.Contains(this.lineNumber)) {
                    this.loopDetected = true;
                    break;                       
                }

                this.linesExecuted.Add(this.lineNumber);
                
                switch (instruction) {
                    case "acc":
                        acc += argument;
                        this.lineNumber++;
                        break;
                    case "jmp":
                        this.lineNumber += argument;
                        break;
                    case "nop":
                        this.lineNumber++;
                        break;
                    default:
                        Console.WriteLine("Error! {0} is not a valid instruction", instruction);
                        break;
                }

                if (this.lineNumber == this.program.Length) {
                    this.programFinished = true;
                }
            }
        }
    }
}
