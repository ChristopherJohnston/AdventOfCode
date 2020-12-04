using System;
using System.Collections.Generic;
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
                Dictionary<string, string> passportDict = new Dictionary<string, string>();

                foreach (string kv in passport.Split(' ')) {
                    string[] pair = kv.Split(':');
                    passportDict.Add(pair[0], pair[1]);
                }

                if (passportDict.Keys.Count < 7 || passportDict.Keys.Count > 8) {
                    continue;
                }
                
                int nKeys = 0;
                string[] requiredKeys = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

                foreach(string key in requiredKeys) {
                    if (!passportDict.ContainsKey(key)) {
                        break;
                    }
                    nKeys++;
                }

                if (nKeys == requiredKeys.Length) {
                    nValid++;
                }                
            }
            Console.WriteLine(nValid);
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
            yield return currentPassport.TrimEnd();
        }
    }
}
