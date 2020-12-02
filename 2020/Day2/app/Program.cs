using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace app
{
    struct PasswordItem {
        public int min;
        public int max;
        public char letter;
        public string password;
    }

    class Program
    {
        static void Main(string[] args)
        {
            int validPasswords_SledRental = 0;
            int validPasswords_TobogganCorporate = 0;

            foreach (PasswordItem passwordItem in ParseInput()) {
                if (isValid_SledRental(passwordItem)) {
                    validPasswords_SledRental +=1;
                }

                if (isValid_TobogganCorporate(passwordItem)) {
                    validPasswords_TobogganCorporate +=1;
                }
            }

            Console.WriteLine("Valid Password for SledRental (Part 1): {0}", validPasswords_SledRental);
            Console.WriteLine("Valid Password for TobogganCorporate (Part 2): {0}", validPasswords_TobogganCorporate);
        }

        static IEnumerable<PasswordItem> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt");
            Regex pattern = new Regex(@"([0-9]+)-([0-9]+) ([a-z]): ([0-9A-Za-z]+)");

            foreach (string line in lines) {
                Match match = pattern.Match(line);

                yield return new PasswordItem() { 
                    min=int.Parse(match.Groups[1].Value),
                    max=int.Parse(match.Groups[2].Value),
                    letter=match.Groups[3].Value.ToCharArray()[0],
                    password=match.Groups[4].Value
                };
            }
        }

        static bool isValid_SledRental(PasswordItem passwordItem) {
            int letterCount = passwordItem.password.Count((letter) => letter == passwordItem.letter);
            return (letterCount >= passwordItem.min) && (letterCount <= passwordItem.max);
        }

        static bool isValid_TobogganCorporate(PasswordItem passwordItem) {
            // N.B. The rule was redefined to mean: min = first location of letter, max = second locaiton of letter
            // Indexing for the password starts at 1
            bool slot1ContainsLetter = (passwordItem.password[passwordItem.min-1] == passwordItem.letter);
            bool slot2ContainsLetter = (passwordItem.password[passwordItem.max-1] == passwordItem.letter);
            return slot1ContainsLetter ^ slot2ContainsLetter;
        }
    }
}