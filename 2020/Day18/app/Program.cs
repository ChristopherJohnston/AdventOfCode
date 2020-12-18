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
            // Part 1
            Console.WriteLine(ParseInput().Select((e) => Evaluate(e)).Sum());
            
            // Part 2
            List<string> expressions = new List<string>();
            foreach (string expression in ParseInput()) {
                string e = expression.Replace(" ", "");
                int i = 0;

                while (i<e.Length) {
                    if (e[i] == '+') {
                        if (e[i-1] == ')') {
                            // if ) seek the ( and add another (
                            int n = 1;
                            int pDepth = 0;
                            while (i-n > 0) {
                                if (e[i-n] == ')') {
                                    pDepth++;
                                } else if (e[i-n] == '(') {
                                    pDepth--;
                                }

                                if (pDepth==0) {
                                    break;
                                };
                                n++;
                            }
                            e = e.Insert(i-n+1, "(");
                            i++;
                        } else {
                            // otherwise put ( before previous number
                            e = e.Insert(i-1, "(");
                            i++;
                        }

                        if (e[i+1] == '(') {
                            // if ( seek the ) and add another )
                            int n = 1;
                            int pDepth = 0;
                            while (i+n < e.Length) {
                                if (e[i+n] == '(') {
                                    pDepth++;
                                } else if (e[i+n] == ')') {
                                    pDepth--;
                                }

                                if (pDepth == 0) {                                    
                                    break;
                                }
                                n++;
                            }

                            e = e.Insert(i+n, ")");
                        } else {
                            // otherwise put ) after next number
                            e = e.Insert(i+2, ")");
                        }
                    }

                    i++;
                }

                expressions.Add(e);
            }

            Console.WriteLine(expressions.Select((e) => Evaluate(e)).Sum());
        }

        static long Evaluate(string expression) {
            Stack<long> operands = new Stack<long>();
            Stack<char> operators = new Stack<char>();
            Func<char, long, long, long> applyOp = (op, n1, n2) => (op == '*') ? n1*n2 : n1+n2;
            expression = expression.Replace(" ", "");

            for (int i=0; i<expression.Length; i++) {
                if (new char[] { '+', '*', '('}.Contains(expression[i])) {
                    operators.Push(expression[i]);
                } else if (expression[i] == ')')  {
                    // We're at the end of a set of parentheses so apply all operators before the opening parenthesis
                    // and push to the operands stack. e.g. (4 * 5) => 20
                    if (operators.Peek() == '(') operators.Pop();

                    while (operators.Count > 0 && operators.Peek() != '(') {
                        operands.Push(applyOp(operators.Pop(), operands.Pop(), operands.Pop()));
                    }
                } else {
                    // We have a number, apply the previous operator to the previous number if it's not in parentheses
                    // otherwise push to the operands stack. e.g. 2 * 3 => 6
                    long val = long.Parse(expression[i].ToString());
                    operands.Push((operators.Count > 0 && operators.Peek() != '(') ? applyOp(operators.Pop(), operands.Pop(), val) : val);
                }
            }

            // Apply remaining operators to remaining operands which are outside of parentheses. e.g. 6 + 20 => 26
            while (operators.Count > 0) {
                operands.Push(applyOp(operators.Pop(), operands.Pop(), operands.Pop()));
            }

            // The result is the last remaining operator
            long res = operands.Pop();
            Console.WriteLine("{0} -> {1}", expression, res);
            return res;
        }

        static IEnumerable<string> ParseInput() {        
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
