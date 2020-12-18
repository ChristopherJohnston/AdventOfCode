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
            Part1();
        }        

        static void Part1() {
            long sumExpressions = 0;

            foreach (string expression in ParseInput()) {
                Stack<long> operands = new Stack<long>();
                Stack<char> operators = new Stack<char>();
                Func<char, long, long, long> applyOp = (op, n1, n2) => (op == '*') ? n1*n2 : n1+n2;

                for (int i=0; i<expression.Length; i++) {
                    if (expression[i] == ' ')
                        continue;

                    if (new char[] { '+', '*', '('}.Contains(expression[i])) {
                        operators.Push(expression[i]);
                    } else if (expression[i] == ')')  {
                        if (operators.Peek() == '(') operators.Pop();

                        while (operators.Count > 0 && operators.Peek() != '(') {
                            operands.Push(applyOp(operators.Pop(), operands.Pop(), operands.Pop()));
                        }
                    } else {
                        long val = long.Parse(expression[i].ToString());
                        operands.Push((operators.Count > 0 && operators.Peek() != '(') ? applyOp(operators.Pop(), operands.Pop(), val) : val);
                    }
                }

                while (operators.Count > 0) {
                    operands.Push(applyOp(operators.Pop(), operands.Pop(), operands.Pop()));
                }

                sumExpressions += operands.Pop();
            }

            Console.WriteLine(sumExpressions);
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
