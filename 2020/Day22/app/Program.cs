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
            Console.WriteLine("Total Games: {0}", gameCount);
        }

        static int Part2(List<Queue<int>> players) {
            int game = ++gameCount;
            Console.WriteLine("=== Game {0} ===", game);
            Console.WriteLine();

            List<(List<int>, List<int>)> previousRounds = new List<(List<int>, List<int>)>();

            int round = 1;
            // Play Game
            while (players.All(p => p.Count > 0)) {
                Console.WriteLine("-- Round {0} (Game {1}) --", round, game);
                for (int i=0; i<players.Count; i++) {
                    Console.WriteLine("Player {0}s deck: {1}", i+1, string.Join(',',players[i].ToArray()));
                }                

                var decks = (players[0].ToList(), players[1].ToList());
                bool defaultWin = previousRounds.Select((pr) => (pr.Item1.SequenceEqual(decks.Item1) && pr.Item2.SequenceEqual(decks.Item2))).Any(r=>r);
                previousRounds.Add(decks);

                var currentRoundCards = players.Select((p) => p.Dequeue()).ToList();
                int winningPlayer;

                if (defaultWin) {
                    Console.WriteLine("Game {0} won by player 1 by default", game);
                    winningPlayer = 0;
                    players[1].Clear();
                } else if (players.Select((p, pidx) => p.Count >= currentRoundCards[pidx]).All(p=>p)) {
                    // start sub game if all players have at least the same number of cards in their deck as the current card value
                    
                    Console.WriteLine("Playing a sub-game to determine the winner...");
                    Console.WriteLine();

                    List<Queue<int>> newPlayers = new List<Queue<int>>();
                    for (int i=0; i<players.Count; i++) {
                        newPlayers.Add(new Queue<int>(players[i].ToArray().Take(currentRoundCards[i])));
                    }
                    winningPlayer = Part2(newPlayers);

                    Console.WriteLine("...anyway, back to game {0}.", game);
                } else {
                    winningPlayer = currentRoundCards.IndexOf(currentRoundCards.Max());
                }

                players[winningPlayer].Enqueue(currentRoundCards[winningPlayer]);
                currentRoundCards.RemoveAt(winningPlayer);
                currentRoundCards.ForEach(c=> players[winningPlayer].Enqueue(c));

                Console.WriteLine("Player {0} wins round {1} of game {2}", winningPlayer+1, round, game);
                Console.WriteLine();
                round++;
            }

            // Count scores
            for (int i=0; i<players.Count; i++) {
                Queue<int> currentPlayer = players[i];
                if (currentPlayer.Count != 0) {
                    string winnerDeck = string.Join(',',currentPlayer.ToArray());
                    long score = 0;
                    for (int multiplier=currentPlayer.Count; multiplier>0; multiplier--) {
                        score += currentPlayer.Dequeue() * multiplier;
                    }
                    if (game == 1) {
                        Console.WriteLine("The winner of game {0} is player {1} with score {2}. Deck: {3}", game, i+1, score, winnerDeck);
                    }
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
