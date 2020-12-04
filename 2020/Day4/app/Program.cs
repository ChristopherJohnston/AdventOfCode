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

            Func<string,int,int, bool> IsValidYear = (value, min, max) => {
                int year;
                return (int.TryParse(value, out year) && year>=min && year<=max);
            };

            int nValid = ParseInput().Select( (passport) => {
                var dict = passport.Split(' ').Select(p=>p.Split(':')).ToDictionary(sp => sp[0], sp=>sp[1]);
                
                // Required keys and their validtors
                return new (string key, Func<string, bool> validator)[] {
                    ("byr", (s)=> IsValidYear(s, 1920, 2002)),
                    ("iyr", (s) => IsValidYear(s, 2010, 2020)),
                    ("eyr", (s) => IsValidYear(s, 2020, 2030)),
                    ("hgt", (s) => {
                        int value;
                        int.TryParse(s.Substring(0, s.Length-2), out value);
                        string unit = s.Substring(s.Length-2, 2);
                        return ((unit == "cm" && value >= 150 && value <= 193) || (unit == "in" && value >=59 && value<=76));
                    }),
                    ("hcl", (s) => Regex.IsMatch(s, @"^\#[0-9a-f]{6}$")),
                    ("ecl", (s) => new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(s)),
                    ("pid", (s) => Regex.IsMatch(s, @"^[0-9]{9}$"))
                }.Select((kv) => (dict.ContainsKey(kv.key) && (skipValidation || kv.validator(dict[kv.key])))).All((i)=>i);
            }).Count((b) => b);

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
            
            // There may not be a trailing line at the end of the file
            yield return currentPassport.TrimEnd();
        }
    }
}
