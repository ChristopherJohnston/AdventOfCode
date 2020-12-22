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
            List<Queue<int>> players = new List<Queue<int>>();

            // Populate Players
            foreach (string[] player in ParseInput()) {
                Queue<int> q = new Queue<int>(player.Skip(1).Select(int.Parse));
                players.Add(q);
            }

            // Play Game
            while (players.All(p => p.Count > 0)) {
                var currentRoundCards = players.Select((p) => p.Dequeue()).ToList();
                int winningPlayer = currentRoundCards.IndexOf(currentRoundCards.Max());
                currentRoundCards.OrderBy((v) => v).Reverse().ToList().ForEach(c=> players[winningPlayer].Enqueue(c));
            }

            // Count scores
            for (int i=0; i<players.Count; i++) {
                Queue<int> currentPlayer = players[i];
                long score = 0;
                for (int multiplier=currentPlayer.Count; multiplier>0; multiplier--) {
                    score += currentPlayer.Dequeue() * multiplier;
                }
                Console.WriteLine("Player {0} score: {1}", i+1, score);
            }
        }

        static IEnumerable<string[]> ParseInput() {
            List<string> currentLine = new List<string>();
            foreach (string line in File.ReadAllLines(@"input.txt")) {
                if (line == string.Empty) {
                    yield return currentLine.ToArray();
                    currentLine = new List<string>();
                }
                else {
                    currentLine.Add(line);
                }
            }
        }
    }
}
