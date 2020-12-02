using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace app
{
    struct Password {
        public int min;
        public int max;
        public char letter;
        public string password;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var passwords = ParseInput();
            var validPasswords = 0;
            foreach (var password in passwords) {
                if (isValid(password)) {
                    validPasswords +=1;
                }
            }
            Console.WriteLine(validPasswords);
        }

        static List<Password> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt");
            var p = new List<Password>();
            Regex pattern = new Regex(@"([0-9]+)-([0-9]+) ([a-z]): ([0-9A-Za-z]+)");
            foreach (string line in lines) {
                var match = pattern.Match(line);

                p.Add(new Password() { 
                    min=int.Parse(match.Groups[1].Value),
                    max=int.Parse(match.Groups[2].Value),
                    letter=match.Groups[3].Value.ToCharArray()[0],
                    password=match.Groups[4].Value
                });
            }
            
            return p;
        }

        static bool isValid(Password password) {
            var letterCount = 0;
            foreach (char letter in password.password) {
                if (letter == password.letter) {
                    letterCount +=1;
                }

                if (letterCount > password.max) {
                    return false;
                }
            }

            if (letterCount < password.min) {
                return false;
            }
            return true;
        }
    }
}
