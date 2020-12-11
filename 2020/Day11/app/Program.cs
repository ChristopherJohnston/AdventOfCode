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

            Func<string, bool> IsOccupied = (string s) => {
                foreach (char c in s) {
                    if (c == 'L') {
                        return false;
                    }
                    if (c == '#') {
                        return true;
                    }
                }
                return false;
            };

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

                        string left = input[r].Substring(0, c);
                        
                        string right = input[r].Substring(c+1);

                        if (IsOccupied(new string(left.ToCharArray().Reverse().ToArray())))
                            occupied++;
                        
                        if (IsOccupied(right))
                            occupied++;

                        string up = string.Empty;
                        string down = string.Empty;
                        for (int i=0; i<input.Length; i++) {
                            if (i<r)
                                up+=input[i][c];
                            else if (i>r)
                                down+=input[i][c];
                        }

                        if (IsOccupied(new string(up.ToCharArray().Reverse().ToArray())))
                            occupied++;
                        
                        if (IsOccupied(down))
                            occupied++;                    
                        
                        string diagonalUpLeft = string.Empty;
                        string diagonalUpRight = string.Empty;
                        int j=c-1;
                        int j2=c+1;
                        for (int i=r-1; i>=0; i--) {
                            if (j>=0) {
                                diagonalUpLeft += input[i][j];
                                j--;
                            }

                            if (j2 < width) {
                                diagonalUpRight += input[i][j2];
                                j2++;
                            }                    
                        }                        

                        if (IsOccupied(diagonalUpLeft))
                            occupied++;
                        
                        if (IsOccupied(diagonalUpRight))
                            occupied++;
                        
                        string diagonalDownLeft = string.Empty;
                        string diagonalDownRight = string.Empty;
                        j = c-1;
                        j2 = c+1;
                        for (int i=r+1; i<input.Length; i++) {                        
                            if (j>=0) {
                                diagonalDownLeft += input[i][j];
                                j--;
                            }

                            if (j2 < width) {
                                diagonalDownRight += input[i][j2];
                                j2++;
                            }  
                        }

                        if (IsOccupied(diagonalDownLeft))
                            occupied++;
                        
                        if (IsOccupied(diagonalDownRight))
                            occupied++;

                        if (input[r][c] == '#' && occupied >= 5) {
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
