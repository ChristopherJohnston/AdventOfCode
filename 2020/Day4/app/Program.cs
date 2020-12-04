using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            int nValid = 0;

            foreach (string passport in ParseInput()) {
                int nKeys = 0;

                // Parse the key-value pairs into a dictionary. pairs separated by spaces, key-value separated by colons
                Dictionary<string, string> passportDict = new Dictionary<string, string>();

                foreach (string kv in passport.Split(' ')) {
                    string[] pair = kv.Split(':');
                    passportDict.Add(pair[0], pair[1]);
                }

                // Check for too few or too many fields
                if (passportDict.Keys.Count < 7 || passportDict.Keys.Count > 8) {
                    continue;
                }
                
                // Required keys and their validtors
                (string, Func<string, bool>)[] requiredKeys = new (string, Func<string, bool>)[] {
                    ("byr", (s)=> IsValidYear(s, 1920, 2002)),
                    ("iyr", (s) => IsValidYear(s, 2010, 2020)),
                    ("eyr", (s) => IsValidYear(s, 2020, 2030)),
                    ("hgt", IsValidHeight),
                    ("hcl", (s) => Regex.IsMatch(s, @"^\#[0-9a-f]{6}$")),
                    ("ecl", (s) => new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(s)),
                    ("pid", (s) => Regex.IsMatch(s, @"^[0-9]{9}$"))
                };

                foreach(var kv in requiredKeys) {
                    // all required keys must be present and valid
                    if (passportDict.ContainsKey(kv.Item1) && kv.Item2(passportDict[kv.Item1])) {
                        nKeys++;
                    } else {
                        break;
                    }
                }

                if (nKeys == requiredKeys.Length) {
                    nValid++;
                }
            }

            Console.WriteLine(nValid);
        }

        static bool IsValidYear(string value, int min, int max) {
            int year;
            return (int.TryParse(value, out year) && year>=min && year<=max);
        }

        static bool IsValidHeight(string height) {
            int value;
            if (!int.TryParse(height.Substring(0, height.Length-2), out value)) {
                return false;
            }

            string unit = height.Substring(height.Length-2, 2);
            return ((unit == "cm" && value >= 150 && value <= 193) || (unit == "in" && value >=59 && value<=76));
        }

        static IEnumerable<string> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt");
            string currentPassport = String.Empty;
            for (int i=0; i<lines.Length; i++) {
                if (lines[i] == string.Empty) {
                    yield return currentPassport.TrimEnd();
                    currentPassport = String.Empty;
                } else {
                    currentPassport += lines[i] + " ";
                }                
            }
            
            // There may not be a trailing line at the end of the file
            yield return currentPassport.TrimEnd();
        }
    }
}
