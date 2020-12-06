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
            Console.WriteLine(ParseInput().Select((p) => {
                string[] answers = p.Split(' ');
                Dictionary<char, int> m = new Dictionary<char, int>();
                foreach (string answer in answers) {
                    foreach (char a in answer.ToCharArray())
                    {
                        if (m.ContainsKey(a)) {
                            m[a] += 1;
                        }
                        else {
                            m[a] = 1;
                        }
                    }
                }
                return m.Count( (p) => p.Value == answers.Length );
            }).Sum());
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
        }
    }
}
