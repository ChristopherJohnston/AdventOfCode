using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace app
{
    /*
    --- Day 8: Seven Segment Search ---

    You barely reach the safety of the cave when the whale smashes into the cave mouth, collapsing it. Sensors indicate another exit to this cave at a much greater depth, so you have no choice but to press on.

    As your submarine slowly makes its way through the cave system, you notice that the four-digit seven-segment displays in your submarine are malfunctioning; they must have been damaged during the escape. You'll be in a lot of trouble without them, so you'd better figure out what's wrong.

    Each digit of a seven-segment display is rendered by turning on or off any of seven segments named a through g:

    0:      1:      2:      3:      4:
     aaaa    ....    aaaa    aaaa    ....
    b    c  .    c  .    c  .    c  b    c
    b    c  .    c  .    c  .    c  b    c
     ....    ....    dddd    dddd    dddd
    e    f  .    f  e    .  .    f  .    f
    e    f  .    f  e    .  .    f  .    f
     gggg    ....    gggg    gggg    ....

    5:      6:      7:      8:      9:
     aaaa    aaaa    aaaa    aaaa    aaaa
    b    .  b    .  .    c  b    c  b    c
    b    .  b    .  .    c  b    c  b    c
     dddd    dddd    ....    dddd    dddd
    .    f  e    f  .    f  e    f  .    f
    .    f  e    f  .    f  e    f  .    f
     gggg    gggg    ....    gggg    gggg

    So, to render a 1, only segments c and f would be turned on; the rest would be off. To render a 7, only segments a, c, and f would be turned on.

    The problem is that the signals which control the segments have been mixed up on each display. The submarine is still trying to display numbers by producing output on signal wires a through g, but those wires are connected to segments randomly. Worse, the wire/segment connections are mixed up separately for each four-digit display! (All of the digits within a display use the same connections, though.)

    So, you might know that only signal wires b and g are turned on, but that doesn't mean segments b and g are turned on: the only digit that uses two segments is 1, so it must mean segments c and f are meant to be on. With just that information, you still can't tell which wire (b/g) goes to which segment (c/f). For that, you'll need to collect more information.

    For each display, you watch the changing signals for a while, make a note of all ten unique signal patterns you see, and then write down a single four digit output value (your puzzle input). Using the signal patterns, you should be able to work out which pattern corresponds to which digit.

    For example, here is what you might see in a single entry in your notes:

    acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
    cdfeb fcadb cdfeb cdbaf
    (The entry is wrapped here to two lines so it fits; in your notes, it will all be on a single line.)

    Each entry consists of ten unique signal patterns, a | delimiter, and finally the four digit output value. Within an entry, the same wire/segment connections are used (but you don't know what the connections actually are). The unique signal patterns correspond to the ten different ways the submarine tries to render a digit using the current wire/segment connections. Because 7 is the only digit that uses three segments, dab in the above example means that to render a 7, signal lines d, a, and b are on. Because 4 is the only digit that uses four segments, eafb means that to render a 4, signal lines e, a, f, and b are on.

    Using this information, you should be able to work out which combination of signal wires corresponds to each of the ten digits. Then, you can decode the four digit output value. Unfortunately, in the above example, all of the digits in the output value (cdfeb fcadb cdfeb cdbaf) use five segments and are more difficult to deduce.

    For now, focus on the easy digits. Consider this larger example:

    be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
    edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
    fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
    fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
    aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
    fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
    dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
    bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
    egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
    gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce

    Because the digits 1, 4, 7, and 8 each use a unique number of segments, you should be able to tell which combinations of signals correspond to those digits.
    Counting only digits in the output values (the part after | on each line), in the above example,
    there are 26 instances of digits that use a unique number of segments (highlighted above).

    In the output values, how many times do digits 1, 4, 7, or 8 appear?

    --- Part Two ---

    Through a little deduction, you should now be able to determine the remaining digits. Consider again the first example above:

    acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf

    After some careful analysis, the mapping between signal wires and segments only make sense in the following configuration:

     dddd
    e    a
    e    a
     ffff
    g    b
    g    b
     cccc

    So, the unique signal patterns would correspond to the following digits:

    acedgfb: 8
    cdfbe: 5
    gcdfa: 2
    fbcad: 3
    dab: 7
    cefabd: 9
    cdfgeb: 6
    eafb: 4
    cagedb: 0
    ab: 1

    Then, the four digits of the output value can be decoded:

    cdfeb: 5
    fcadb: 3
    cdfeb: 5
    cdbaf: 3

    Therefore, the output value for this entry is 5353.

    Following this same process for each entry in the second, larger example above, the output value of each entry can be determined:

    fdgacbe cefdb cefbgd gcbe: 8394
    fcgedb cgb dgebacf gc: 9781
    cg cg fdcagb cbg: 1197
    efabcd cedba gadfec cb: 9361
    gecf egdcabf bgf bfgea: 4873
    gebdcfa ecba ca fadegcb: 8418
    cefg dcbef fcge gbcadfe: 4548
    ed bcgafe cdgba cbgef: 1625
    gbdfcae bgc cg cgb: 8717
    fgae cfgab fg bagce: 4315

    Adding all of the output values in this larger example produces 61229.

    For each entry, determine all of the wire/segment connections and decode the four-digit output values. What do you get if you add up all of the output values?
    */
    class Program
    {
        static string file = @"input.txt";

        static void Main(string[] args)
        {
            if (args.Length > 0) {
                file = args[0];
            }
            
            List<string> input = FileUtils.ParseInput(file).ToList();
            Part1(input);

            // Single-line input from example
            // input = new List<string>();
            // input.Add("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf");

            Part2(input);
        }

        static void Part1(List<string> input) {
            // 0 - 6 segments, abcefg
            // 1 - 2 segments, cf **
            // 2 - 5 segments, acdeg
            // 3 - 5 segments, acdfg
            // 4 - 4 segments, bcdf **
            // 5 - 5 segments, abdfg
            // 6 - 6 segments, abdefg
            // 7 - 3 segments, acf **
            // 8 - 7 segments, abcdefg **
            // 9 - 6 segments, abcdfg

            // Get number of times 1, 4,7, 8 appear

            long uniqueCount = 0;

            foreach (string line in input) {
                string[] l = line.Split(" | ");
                string[] signals = l[0].Split(' ');
                string[] outputs = l[1].Split(' ');

                foreach (string output in outputs) {
                    if (output.Length == 2 || output.Length == 3 || output.Length == 4 || output.Length == 7) {
                        uniqueCount++;
                    }
                }
            }

            // 1,4,7, or 8 appear 479 times.
            Console.WriteLine($"1,4,7, or 8 appear {uniqueCount} times.");
        }

        static void Part2(List<string> input) {
            int total = 0;

            foreach (string line in input) {
                string[] l = line.Split(" | ");
                string[] signals = l[0].Split(' ').OrderBy(x => x.Length).ToArray();
                string[] outputs = l[1].Split(' ');

                var mapping = new Dictionary<int, string>();

                /*              
                 aaaa
                b    c
                b    c
                 dddd
                e    f
                e    f
                 gggg
                */

                /*              
                 ....
                .    c
                .    c
                 ....
                .    f
                .    f
                 ....

                1 - 2 segments, cf
                */
                mapping[1] = signals[0];

                /*              
                 aaaa
                .    c
                .    c
                 ....
                .    f
                .    f
                 ....

                 
                7 - 3 segments, acf
                */
                mapping[7] = signals[1];

                /*              
                 ....
                b    c
                b    c
                 dddd
                .    f
                .    f
                 ....

                4 - 4 segments, bcdf
                */
                mapping[4] = signals[2];
                
                /*              
                 aaaa
                b    c
                b    c
                 dddd
                e    f
                e    f
                 gggg

                8 - 7 segments, abcdefg
                */
                mapping[8] = signals[9];

                // 2 - 5 segments, a   c d e   g -> ce
                // 3 - 5 segments, a   c d   f g -> cf
                // 5 - 5 segments, a b   d   f g -> bf
                var fiveSegments = signals.Skip(3).Take(3).ToList();

                // 0 - 6 segments, ab c   e fg -> ce
                // 6 - 6 segments, ab   d e fg -> de
                // 9 - 6 segments, ab c d   fg -> cd
                var sixSegments = signals.Skip(6).Take(3).ToList();
            
                /*              
                 aaaa    aaaa    aaaa          ....
                b    .  .    c  .    c        .    c
                b    .  .    c  .    c        .    c
                 dddd    dddd    dddd          ....
                .    f  e    .  .    f        .    f
                .    f  e    .  .    f        .    f
                 gggg    gggg    gggg          ....

                Number 3 (acdfg) is the only one with five segments to contain all the segments for 1 (cf)
                */
                mapping[3] = fiveSegments.Where(signal => mapping[1].All(signal.Contains)).First();
                fiveSegments.Remove(mapping[3]);
                
                /*              
                 aaaa    aaaa   aaaa          aaaa
                b    c  b    . b    c        .    c
                b    c  b    . b    c        .    c
                 ....    dddd   dddd          dddd
                e    f  e    f .    f        .    f
                e    f  e    f .    f        .    f
                 gggg    gggg   gggg          gggg

                Number 9 (abcdfg) is the only one with six segments to contain all the segments for 3 (acdfg) -> missing b only
                */
                mapping[9] = sixSegments.Where(signal => mapping[3].All(signal.Contains)).First();
                sixSegments.Remove(mapping[9]);

                /*              
                 aaaa    aaaa         ....
                b    c  b    .       .    c
                b    c  b    .       .    c
                 ....    dddd         ....
                e    f  e    f       .    f
                e    f  e    f       .    f
                 gggg    gggg         ....

                With 9 removed, 0 (abcefg) is the only one in those with six segments to contain the segments for 1 (cf)
                */            
                mapping[0] = sixSegments.Where(signal => mapping[1].All(signal.Contains)).First();
                sixSegments.Remove(mapping[0]);
            
                /*              
                 aaaa 
                b    .
                b    .
                 dddd 
                e    f
                e    f
                 gggg 

                The remaining item in six segments is 6
                */  
                mapping[6] = sixSegments[0];

                /*              
                 aaaa    aaaa          aaaa
                b    .  .    c        b    .
                b    .  .    c        b    .
                 dddd    dddd          dddd
                .    f  e    .        e    f
                .    f  e    .        e    f
                 gggg    gggg          gggg

                To distinguish 5 (abdfg) and 2 (acdeg), we can use 6 (abdefg) -> 6 contains all the segments of 5 but not 2
                */
                mapping[5] = fiveSegments.Where(signal => signal.All(mapping[6].Contains)).First();
                fiveSegments.Remove(mapping[5]);

                /*              
                 aaaa 
                .    c
                .    c
                 dddd 
                e    .
                e    .
                 gggg 

                The remaining item in five segments is 2
                */  
                mapping[2] = fiveSegments[0];


                /*
                Now that we've found the segments for each digit, we can remap

                e.g. for the example:
                
                acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf

                abcdeg: 0
                ab: 1
                acdfg: 2
                abcdf: 3
                abef: 4
                bcdef: 5
                bcdefg: 6
                abd: 7
                abcdefg: 8
                abcdef: 9
                */
                var digitMap = new Dictionary<string, int>();
                foreach(var kv in mapping) {
                    digitMap[string.Concat(kv.Value.OrderBy(c=>c))] = kv.Key;
                }

                // Go through the outputs, find which digit each one
                // maps to and concatenate into a single number
                string result = string.Empty;
                foreach (string output in outputs) {
                    string ordered = string.Concat(output.OrderBy(c=>c));
                    result += digitMap[ordered].ToString();
                }

                // The result is the sum of all numbers in the input
                total += int.Parse(result);
            }

            Console.WriteLine($"Total: {total}");
        }
    }
}
