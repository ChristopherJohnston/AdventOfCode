using System;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            // int[] start = new int[] { 0, 3, 6 };
            int[] start = new int[] { 1, 20,11,6,12,0 };
            Dictionary<int, List<int>> turns = new Dictionary<int, List<int>>();
            int lastSpoken = 0;

            for (int turn=0; turn<2020; turn++) {
                if (turn < start.Length) {
                    int n = start[turn];
                    turns[n] = new List<int> { turn };
                    lastSpoken = n;
                    continue;
                }

                lastSpoken = (turns.ContainsKey(lastSpoken) && turns[lastSpoken].Count > 1)
                                    ? turns[lastSpoken][^1] - turns[lastSpoken][^2]
                                    : 0;

                if (turns.ContainsKey(lastSpoken)) {
                    turns[lastSpoken].Add(turn);
                } else {
                    turns[lastSpoken] = new List<int> { turn };
                }
            }

            Console.WriteLine(lastSpoken);
        }
    }
}
