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
            string mask = "";
            Dictionary<long, long> memory = new Dictionary<long, long>();

            foreach (string command in ParseInput()) {
                if (command.Contains("mask")) {
                    mask = Regex.Match(command, @"^mask = (.*)$").Groups[1].Value;
                } else if (command.Contains("mem")) {
                    Match m = Regex.Match(command, @"^mem\[(\d+)\] = (\d+)$");
                    long key = long.Parse(m.Groups[1].Value);
                    long value = long.Parse(m.Groups[2].Value);
                    string bits = Convert.ToString(value, 2);
                    string newBits = String.Empty;
                    for (int i=0; i<mask.Length; i++) {
                        switch (mask[i]) {
                            case '1':
                                newBits += '1';
                                break;
                            case '0':
                                newBits += '0';
                                break;
                            case 'x':
                            default:
                                if (i+bits.Length-36 >=0) {
                                    newBits += bits[i+bits.Length-36];
                                }
                                else {
                                    newBits += '0';
                                }
                                
                                break;
                        }
                    }
                    memory[key] = Convert.ToInt64(newBits, 2);
                }
            }

            Console.WriteLine(memory.Sum((kv) => kv.Value));
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
