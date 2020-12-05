using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static int Search(string ticket, char lower) {
            int min = 0;
            int max = (int)Math.Pow((double)2, (double)ticket.Length) - 1;

            foreach (char letter in ticket) {
                int middle = (max-min)/2 + 1;

                if (letter == lower) {
                    max -= middle;
                }
                else
                {
                    min += middle;
                }
                // Console.WriteLine("{0}: {1}-{2}", letter, min, max);
            }
            return min;
        }

        static void Main(string[] args)
        {
            int maxId = 0;
            foreach (string ticket in ParseInput()) {
                // string ticket = "FBFBBFFRLR";
                
                int row = Search(ticket.Substring(0,7), 'F');
                int seat = Search(ticket.Substring(7,3), 'L');
                
                // Console.WriteLine("{0}: Row {1}, Seat {2}", ticket, row, seat);
                int seatId = row * 8 + seat;
                maxId = Math.Max(maxId, seatId);
            }

            Console.WriteLine(maxId);
        }
        static IEnumerable<string> ParseInput() {
            string[] lines = File.ReadAllLines(@"input.txt");

            foreach (string line in lines) {
                yield return line;
            }
        }
    }
}
