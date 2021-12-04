using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace app
{
    /*
    --- Day 4: Giant Squid ---

    You're already almost 1.5km (almost a mile) below the surface of the ocean, already so deep that you can't see any sunlight. What you can see, however, is a giant squid that has attached itself to the outside of your submarine.

    Maybe it wants to play bingo?

    Bingo is played on a set of boards each consisting of a 5x5 grid of numbers. Numbers are chosen at random, and the chosen number is marked on all boards on which it appears. (Numbers may not appear on all boards.) If all numbers in any row or any column of a board are marked, that board wins. (Diagonals don't count.)

    The submarine has a bingo subsystem to help passengers (currently, you and the giant squid) pass the time. It automatically generates a random order in which to draw numbers and a random set of boards (your puzzle input). For example:

    7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

    22 13 17 11  0
    8  2 23  4 24
    21  9 14 16  7
    6 10  3 18  5
    1 12 20 15 19

    3 15  0  2 22
    9 18 13 17  5
    19  8  7 25 23
    20 11 10 24  4
    14 21 16 12  6

    14 21 17 24  4
    10 16 15  9 19
    18  8 23 26 20
    22 11 13  6  5
    2  0 12  3  7
    After the first five numbers are drawn (7, 4, 9, 5, and 11), there are no winners, but the boards are marked as follows (shown here adjacent to each other to save space):

    22 13 17 11  0         3 15  0  2 22        14 21 17 24  4
    8  2 23  4 24         9 18 13 17  5        10 16 15  9 19
    21  9 14 16  7        19  8  7 25 23        18  8 23 26 20
    6 10  3 18  5        20 11 10 24  4        22 11 13  6  5
    1 12 20 15 19        14 21 16 12  6         2  0 12  3  7
    After the next six numbers are drawn (17, 23, 2, 0, 14, and 21), there are still no winners:

    22 13 17 11  0         3 15  0  2 22        14 21 17 24  4
    8  2 23  4 24         9 18 13 17  5        10 16 15  9 19
    21  9 14 16  7        19  8  7 25 23        18  8 23 26 20
    6 10  3 18  5        20 11 10 24  4        22 11 13  6  5
    1 12 20 15 19        14 21 16 12  6         2  0 12  3  7
    Finally, 24 is drawn:

    22 13 17 11  0         3 15  0  2 22        14 21 17 24  4
    8  2 23  4 24         9 18 13 17  5        10 16 15  9 19
    21  9 14 16  7        19  8  7 25 23        18  8 23 26 20
    6 10  3 18  5        20 11 10 24  4        22 11 13  6  5
    1 12 20 15 19        14 21 16 12  6         2  0 12  3  7
    At this point, the third board wins because it has at least one complete row or column of marked numbers (in this case, the entire top row is marked: 14 21 17 24 4).

    The score of the winning board can now be calculated. Start by finding the sum of all unmarked numbers on that board; in this case, the sum is 188. Then, multiply that sum by the number that was just called when the board won, 24, to get the final score, 188 * 24 = 4512.

    To guarantee victory against the giant squid, figure out which board will win first. What will your final score be if you choose that board?


    --- Part Two ---

    On the other hand, it might be wise to try a different strategy: let the giant squid win.

    You aren't sure how many bingo boards a giant squid could play at once, so rather than waste time counting its arms, the safe thing to do is to figure out which board will win last and choose that one. That way, no matter which boards it picks, it will win for sure.

    In the above example, the second board is the last to win, which happens after 13 is eventually called and its middle column is completely marked. If you were to keep playing until this point, the second board would have a sum of unmarked numbers equal to 148 for a final score of 148 * 13 = 1924.

    Figure out which board will win last. Once it wins, what would its final score be?
    */
    class Program
    {
        static string file = @"input.txt";
        
        static void Main(string[] args)
        {
            if (args.Length > 0) {
                file = args[0];
            }
            Console.WriteLine("**** Part 1 ****");
            Part1();
            Console.WriteLine("**** Part 2 ****");
            Part2();
        }

        static void Part1() {
            List<string> input = FileUtils.ParseInput(file).ToList();
            
            var numbers = input[0].SplitToList<int>();
            List<int[][]> cards = GetCards(input.Skip(2).ToList());            
            List<bool[][]> matches = GetEmptyMatches(cards);

            foreach (int number in numbers) {
                matches = CheckNumbers(cards, matches, number);

                List<int> winners = CheckForWinners(cards, matches);

                foreach (int winningCardIndex in winners) {    
                    int score = GetCardScore(cards[winningCardIndex], matches[winningCardIndex]);
                    // Winner! Card 7 scores 330. Answer: 8580
                    Console.WriteLine($"Winner! Card {winningCardIndex} scores {score}. Answer: {score*number}");
                    return;
                }
            }
        }

        static void Part2() {
            List<string> input = FileUtils.ParseInput(file).ToList();
            
            var numbers = input[0].SplitToList<int>();
            List<int[][]> cards = GetCards(input.Skip(2).ToList());            
            List<bool[][]> matches = GetEmptyMatches(cards);

            foreach (int number in numbers) {
                matches = CheckNumbers(cards, matches, number);
                List<int> winners = CheckForWinners(cards, matches);

                foreach (int winningCardIndex in winners) {
                    if (cards.Count == 1) {
                        int cardScore = GetCardScore(cards[winningCardIndex], matches[winningCardIndex]);
                        // Loser! Final card scores 228 with number 42. Answer: 9576
                        Console.WriteLine($"Loser! Final card scores {cardScore} with number {number}. Answer: {cardScore * number}");
                        return;
                    }

                    cards.RemoveAt(winningCardIndex);
                    matches.RemoveAt(winningCardIndex);
                }
            }
        }

        static int GetCardScore(int[][] card, bool[][] matches) {
            int score = 0;
            for (int r=0; r<card.Length; r++) {
                for (int c=0; c<card[r].Length; c++) {
                    if (!matches[r][c]) {
                        score += card[r][c];
                    }
                }
            }
            return score;
        }

        static List<int> CheckForWinners(List<int[][]> cards, List<bool[][]> matches) {
            List<int> winners = new List<int>();

            for (int i=0; i<matches.Count;i++) {
                bool[][] card = matches[i];
                // Check rows
                for (int r=0; r<card.Length; r++) {
                    if (card[r].All(x => x==true)) {
                        winners.Add(i);
                    }
                }

                // Check cols                                        
                for (int c=0; c<card[0].Length; c++) {
                    List<bool> col = new List<bool>();

                    for (int r=0; r<card.Length; r++) {
                        col.Add(card[r][c]);
                    }
                    if (col.All(x => x==true)) {
                        winners.Add(i);
                    }
                }
            }

            // Sort and reverse indices so they are removed from the list correctly
            winners.Sort();
            winners.Reverse();
            return winners;
        }

        static List<bool[][]> CheckNumbers(List<int[][]> cards, List<bool[][]> matches, int number) {
            for (int i=0; i<cards.Count; i++) {
                for (int r=0;r<cards[i].Length; r++) {
                    for (int c=0; c<cards[i][r].Length; c++) {
                        if (cards[i][r][c] == number) {
                            matches[i][r][c] = true;
                        }
                    }
                }
            }

            return matches;
        }

        static List<bool[][]> GetEmptyMatches(List<int[][]> cards) {
            List<bool[][]> matches = new List<bool[][]>();

            foreach (int[][] card in cards) {
                bool[][] cardMatches = new bool[card.Length][];
                for (int r=0;r<cardMatches.Length;r++) {
                    cardMatches[r] = new bool[card[r].Length];
                }
                matches.Add(cardMatches);
            }

            return matches;
        }
        static List<int[][]> GetCards(List<string> input) {
            List<int[][]> cards = new List<int[][]>();
            int start = 0;
            // Go through the input for cards
            while (start < input.Count) {
                int[][] card = new int[5][];
                var cardInput = input.Skip(start).Take(5).ToList();
                // Create a card
                for (int r=0;r<5;r++) {
                    card[r] = new int[5];
                    MatchCollection matches = Regex.Matches(cardInput[r], @"\d+");
                    for (int c=0;c<matches.Count;c++) {
                        card[r][c] = Int32.Parse(matches[c].Value);
                    }
                }
                cards.Add(card);
                start += 6;                
            }

            return cards;
        }
    }
}
 