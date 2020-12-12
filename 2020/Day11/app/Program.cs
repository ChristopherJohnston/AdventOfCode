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
                        if (input[r][c] == '.') {
                            continue;
                        }
                        
                        string[] seats = new string[8];
                        for (int i=0; i<8;i++) {
                            seats[i] = string.Empty;
                        }

                        // Left and Right
                        seats[0] = new string(input[r].Substring(0, c).ToCharArray().Reverse().ToArray());
                        seats[1] = input[r].Substring(c+1);

                        // Up and down
                        for (int i=0; i<input.Length; i++) {
                            if (i<r)
                                seats[2]+=input[i][c];
                            else if (i>r)
                                seats[3]+=input[i][c];
                        }

                        seats[2] = new string(seats[2].ToCharArray().Reverse().ToArray());
                        
                        // Diagonals up
                        int left=c-1;
                        int right=c+1;
                        for (int i=r-1; i>=0; i--) {
                            if (left>=0) {
                                seats[4] += input[i][left];
                                left--;
                            }

                            if (right < width) {
                                seats[5] += input[i][right];
                                right++;
                            }                    
                        }

                        // Diagonals down
                        left = c-1;
                        right = c+1;
                        for (int i=r+1; i<input.Length; i++) {                        
                            if (left>=0) {
                                seats[6] += input[i][left];
                                left--;
                            }

                            if (right < width) {
                                seats[7] += input[i][right];
                                right++;
                            }  
                        }

                        int occupied = seats.Count((s) => IsOccupied(s));

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
