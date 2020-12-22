using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace app
{
    class Program
    {
        static int gameCount = 0;

        static void Main(string[] args)
        {
            List<Queue<int>> players = new List<Queue<int>>();

            // Populate Players
            foreach (string[] player in ParseInput()) {
                Queue<int> q = new Queue<int>(player.Skip(1).Select(int.Parse));
                players.Add(q);
            }

            // Part1(players);
            Part2(players);

            // Count scores
            for (int n=0; n<players.Count; n++) {
                Console.WriteLine("Player {0} score: {1}", n+1, players[n].Select((c, i) => (players[n].Count-i) * c).Sum());
            }
            Console.WriteLine("Total Games: {0}", gameCount);
        }

        static int Part2(List<Queue<int>> players) {
            int game = ++gameCount;

            List<(List<int>, List<int>)> previousRounds = new List<(List<int>, List<int>)>();

            // Play Game
            while (players.All(p => p.Count > 0)) {            
                var decks = (players[0].ToList(), players[1].ToList());
                bool defaultWin = previousRounds.Select((pr) => (pr.Item1.SequenceEqual(decks.Item1) && pr.Item2.SequenceEqual(decks.Item2))).Any(r=>r);
                previousRounds.Add(decks);

                var currentRoundCards = players.Select((p) => p.Dequeue()).ToList();
                int winningPlayer;

                if (defaultWin) {
                    winningPlayer = 0;
                    players[1].Clear();
                } else if (players.Select((p, pidx) => p.Count >= currentRoundCards[pidx]).All(p=>p)) {
                    // start sub game if all players have at least the same number of cards in their deck as the current card value
                    List<Queue<int>> newPlayers = new List<Queue<int>>();
                    for (int i=0; i<players.Count; i++) {
                        newPlayers.Add(new Queue<int>(players[i].Take(currentRoundCards[i])));
                    }
                    winningPlayer = Part2(newPlayers);
                } else {
                    winningPlayer = currentRoundCards.IndexOf(currentRoundCards.Max());
                }

                players[winningPlayer].Enqueue(currentRoundCards[winningPlayer]);
                currentRoundCards.RemoveAt(winningPlayer);
                currentRoundCards.ForEach(c=> players[winningPlayer].Enqueue(c));
            }

            // Count scores
            for (int i=0; i<players.Count; i++) {
                if (players[i].Count != 0) {
                    return i;
                }
            }

            // Should never get here
            return -1;
        }

        static void Part1(List<Queue<int>> players)
        {
            // Play Game
            while (players.All(p => p.Count > 0)) {
                var currentRoundCards = players.Select((p) => p.Dequeue()).ToList();
                int winningPlayer = currentRoundCards.IndexOf(currentRoundCards.Max());
                currentRoundCards.OrderBy((v) => v).Reverse().ToList().ForEach(c=> players[winningPlayer].Enqueue(c));
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
