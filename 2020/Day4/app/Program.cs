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
            bool skipValidation = false; // true for part 1, false for part2

            Console.WriteLine(ParseInput().Select( (passport) => {
                var dict = passport.Split(' ').Select(p=>p.Split(':')).ToDictionary(sp => sp[0], sp=>sp[1]);
                
                // Required keys and their validtors
                return new (string key, string re)[] {
                    ("byr", @"^(19[2-8][0-9]|199[0-9]|200[0-2])$"),
                    ("iyr", @"^(201[0-9]|2020)$"),
                    ("eyr", @"^(202[0-9]|2030)$"),
                    ("hgt", @"^(1[5-8][0-9]|19[0-3])cm$|^(59|6[0-9]|7[0-6])in$"),                    
                    ("hcl", @"^\#[0-9a-f]{6}$"),
                    ("ecl", @"^(amb|blu|brn|gry|grn|hzl|oth)$"),
                    ("pid", @"^[0-9]{9}$")
                }.Select((p) => (dict.ContainsKey(p.key) && (skipValidation || Regex.IsMatch(dict[p.key], p.re)))).All((i)=>i);
            }).Count((b) => b));
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
