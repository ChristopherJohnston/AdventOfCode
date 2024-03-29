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
            Part1();
            Part2();
        }

        static void Part2() {
            string mask = string.Empty;
            Dictionary<long, long> memory = new Dictionary<long, long>();

            foreach (string command in ParseInput()) {
                if (command.Contains("mask")) {
                    mask = Regex.Match(command, @"^mask = (.*)$").Groups[1].Value;
                } else if (command.Contains("mem")) {
                    Match m = Regex.Match(command, @"^mem\[(\d+)\] = (\d+)$");
                    string currentKey = Convert.ToString(long.Parse(m.Groups[1].Value), 2);
                    List<string> keys = new List<string>() { String.Empty };

                    for (int i=0; i<mask.Length; i++) {
                        if (mask[i] == 'X') {
                            List<string> nb = new List<string>();
                            for (int j=0; j<keys.Count; j++) {
                                nb.Add(keys[j] + '1');
                                keys[j] += '0';
                            }
                            keys.AddRange(nb);
                        } else {
                            int offset = currentKey.Length-mask.Length;
                            for (int j=0; j<keys.Count; j++) {
                                keys[j] += (mask[i] == '0' && (i+offset >=0)) ? currentKey[i+offset] : mask[i];
                            }
                        }
                    }
                    
                    keys.ForEach((k) => memory[Convert.ToInt64(k, 2)] = long.Parse(m.Groups[2].Value));
                }
            }

            Console.WriteLine(memory.Sum((kv) => kv.Value)); // 4877695371685
        }

        static void Part1() {
            string mask = "";
            Dictionary<long, long> memory = new Dictionary<long, long>();

            foreach (string command in ParseInput()) {
                if (command.Contains("mask")) {
                    mask = Regex.Match(command, @"^mask = (.*)$").Groups[1].Value;
                } else if (command.Contains("mem")) {
                    Match m = Regex.Match(command, @"^mem\[(\d+)\] = (\d+)$");
                    string value = Convert.ToString(long.Parse(m.Groups[2].Value), 2);
                    string newValue = String.Empty;
                    for (int i=0; i<mask.Length; i++) {
                        if (mask[i] == 'X' && (i+value.Length-36 >=0)) {
                            newValue += value[i+value.Length-36];
                        } else if (mask[i] == 'X') {
                            newValue += '0';
                        }
                        else {
                            newValue += mask[i];
                        }
                    }
                    memory[long.Parse(m.Groups[1].Value)] = Convert.ToInt64(newValue, 2);
                }
            }

            Console.WriteLine(memory.Sum((kv) => kv.Value)); // 9296748256641
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
