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
            Part1();
            Part2();
        }

        static void Part2() {
            string mask = "";
            Dictionary<long, long> memory = new Dictionary<long, long>();

            foreach (string command in ParseInput()) {
                if (command.Contains("mask")) {
                    mask = Regex.Match(command, @"^mask = (.*)$").Groups[1].Value;
                } else if (command.Contains("mem")) {
                    Match m = Regex.Match(command, @"^mem\[(\d+)\] = (\d+)$");
                    long key = long.Parse(m.Groups[1].Value);
                    long value = long.Parse(m.Groups[2].Value);

                    string bits = Convert.ToString(key, 2);
                    List<string> newBits = new List<string>() { String.Empty };

                    for (int i=0; i<mask.Length; i++) {
                        switch (mask[i]) {
                            case '1':
                                for (int j=0; j<newBits.Count; j++) {
                                    newBits[j] += '1';
                                }                                
                                break;
                            case '0':
                                for (int j=0; j<newBits.Count; j++) {
                                    newBits[j] += (i+bits.Length-36 >=0) ? bits[i+bits.Length-36] : '0';
                                }
                                break;
                            case 'X':
                            default:
                                List<string> nb = new List<string>();
                                for (int j=0; j<newBits.Count; j++) {
                                    nb.Add(newBits[j] + "1");
                                    newBits[j] += '0';
                                }
                                newBits.AddRange(nb);
                                break;
                        }
                    }
                    
                    foreach (string n in newBits) {
                        memory[Convert.ToInt64(n, 2)] = value;
                    }
                }
            }

            Console.WriteLine(memory.Sum((kv) => kv.Value));
        }

        static void Part1() {
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
                        if (mask[i] == 'X' && (i+bits.Length-36 >=0)) {
                            newBits += bits[i+bits.Length-36];
                        } else if (mask[i] == 'X') {
                            newBits += '0';
                        }
                        else {
                            newBits += mask[i];
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
