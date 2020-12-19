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
            string[] input = File.ReadAllLines(@"input.txt");
            int i=0;

            Dictionary<int, string> rules = new Dictionary<int, string>();
            while (input[i] != string.Empty) {
                string[] kv = input[i].Split(": ");
                rules[int.Parse(kv[0])] = kv[1].Replace("\"", "");
                i++;
            }

            string[] messages = input.Skip(i).ToArray();

            string ruleZero = rules[0];
            Regex regex = new Regex(@"[0-9]+");
            Match m = regex.Match(ruleZero);
            while (m.Length > 0) {
                string r = rules[int.Parse(m.Value)];
                r = (r.Length == 1) ? r : string.Format("({0})", r);
                ruleZero = regex.Replace(ruleZero, r, 1);
                m = regex.Match(ruleZero);
            }

            ruleZero = String.Format("^{0}$",ruleZero.Replace(" ", ""));
            Console.WriteLine(ruleZero.Length);

            Console.WriteLine(messages.Count((m) => Regex.IsMatch(m, ruleZero)));

        }
    }
}
