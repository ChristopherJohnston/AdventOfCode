using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            var validPasswords_SledRental = 0;
            var validPasswords_TobogganCorporate = 0;

            foreach (var password in ParseInput()) {
                if (isValid_SledRental(password)) {
                    validPasswords_SledRental +=1;
                }

                if (isValid_TobogganCorporate(password)) {
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
                var match = pattern.Match(line);

                yield return new PasswordItem() { 
                    min=int.Parse(match.Groups[1].Value),
                    max=int.Parse(match.Groups[2].Value),
                    letter=match.Groups[3].Value.ToCharArray()[0],
                    password=match.Groups[4].Value
                };
            }
        }

        static bool isValid_SledRental(PasswordItem passwordItem) {
            var letterCount = 0;
            foreach (char letter in passwordItem.password) {
                if (letter == passwordItem.letter) {
                    letterCount +=1;
                }

                if (letterCount > passwordItem.max) {
                    return false;
                }
            }

            if (letterCount < passwordItem.min) {
                return false;
            }
            return true;
        }

        static bool isValid_TobogganCorporate(PasswordItem passwordItem) {
            var slot1ContainsLetter = (passwordItem.password[passwordItem.min-1] == passwordItem.letter);
            var slot2ContainsLetter = (passwordItem.password[passwordItem.max-1] == passwordItem.letter);
            return slot1ContainsLetter ^ slot2ContainsLetter;
        }
    }
}