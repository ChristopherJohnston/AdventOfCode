using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = ParseInput().ToArray();

            // string[] input = new string[] {
            //     "L.LL.LL.LL",
            //     "LLLLLLL.LL",
            //     "L.L.L..L..",
            //     "LLLL.LL.LL",
            //     "L.LL.LL.LL",
            //     "L.LLLLL.LL",
            //     "..L.L.....",
            //     "LLLLLLLLLL",
            //     "L.LLLLLL.L",
            //     "L.LLLLL.LL"
            // };

            int numSeatsChanged = 1;
            int iterations = 0;

            while (numSeatsChanged > 0) {
                numSeatsChanged = 0;
                string[] newInput = new string[input.Length];
                input.CopyTo(newInput, 0);

                for (int r=0; r<input.Length; r++) {
                    int width = input[r].Length;

                    for (int c=0; c<width; c++) {
                        int occupied = 0;

                        if (input[r][c] == '.') {
                            continue;
                        }
                        
                        for (int i=r-1; i<r+2; i++) {
                            for (int j=c-1; j<c+2; j++) {
                                if (i>=0 && i<input.Length && j>=0 && j<width && input[i][j] == '#') {
                                    if (i==r && j==c) {
                                        continue;
                                    }
                                    occupied++;
                                }
                            }
                        }

                        if (input[r][c] == '#' && occupied >= 4) {
                            newInput[r] = newInput[r].Remove(c,1).Insert(c, "L");
                            numSeatsChanged++;
                        }
                        else if (occupied == 0 && input[r][c] == 'L') {
                            newInput[r] = newInput[r].Remove(c,1).Insert(c, "#");
                            numSeatsChanged++;
                        }
                    }
                }

                iterations++;
                input = new string[input.Length];
                newInput.CopyTo(input, 0);
            }

            Console.WriteLine(input.Sum((s) => s.Count(c => c=='#')));
        }

        static IEnumerable<string> ParseInput() {
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                yield return line;
            }
        }
    }
}
