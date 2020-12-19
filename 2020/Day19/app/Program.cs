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

            string ruleZero = String.Format("^{0}$", Solve(rules, 0, int.MaxValue));
            Console.WriteLine(ruleZero.Length);
            Console.WriteLine(messages.Count((m) => Regex.IsMatch(m, ruleZero)));

            rules[8] = "42 | 42 8";
            rules[8] = Solve(rules, 8, 10000);
            rules[11] = "42 31 | 42 11 31";
            rules[11] = Solve(rules, 11, 10000);

            ruleZero = String.Format("^{0}$",Solve(rules, 0, int.MaxValue));
            Console.WriteLine(ruleZero.Length);
            Console.WriteLine(messages.Count((m) => Regex.IsMatch(m, ruleZero)));
        }
        static string Solve(Dictionary<int, string> rules, int number, int maxIterations) {
            string ruleToSolve = rules[number];
            Regex regex = new Regex(@"[0-9]+");
            Match m = regex.Match(ruleToSolve);
            int iterations=0;
            while (m.Length > 0) {
                int n = int.Parse(m.Value);
                string r = rules[int.Parse(m.Value)];
                r = (r.Length == 1) ? r : string.Format("({0})", r);
                ruleToSolve = regex.Replace(ruleToSolve, r, 1);
                m = regex.Match(ruleToSolve);
                iterations++;
                if (n == number && iterations > maxIterations){
                    ruleToSolve = ruleToSolve.Replace("("+rules[number]+")", "");
                    break;
                }
            }

            ruleToSolve = ruleToSolve.Replace(" ", "");
            return ruleToSolve;
        }
    }
}
