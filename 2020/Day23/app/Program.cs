using System;
using System.Linq;
using System.Collections.Generic;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "247819356";
            // input = "389125467"; // Example
            Part1(input);
            Part2(input);
        }

        static void Part1(string input) {            
            LinkedList<int> circle = new LinkedList<int>(input.Select(c => int.Parse(c.ToString())));

            // Part 1
            Game(circle, 100);

            // Get order from 1
            LinkedListNode<int> node = circle.Find(1).Next ?? circle.First;
            string result = string.Empty;
            for (int i=0; i<8; i++) {
                result += node.Value;
                node=node.Next ?? circle.First;
            }
            Console.WriteLine(result);
        }

        static void Part2(string input) {
            List<int> values = input.Select(c => int.Parse(c.ToString())).ToList();
            for (int i=10; i<=1000000; i++) {
                values.Add(i);
            }

            LinkedList<int> circle = new LinkedList<int>(values);

            Game(circle, 10000000);
            LinkedListNode<int> node1 = circle.Find(1).Next ?? circle.First;
            LinkedListNode<int> node2 = node1.Next ?? circle.First;

            Console.WriteLine((long)node1.Value * (long)node2.Value);
        }

        static void Game(LinkedList<int> circle, long nMoves) {
            LinkedListNode<int> current = circle.First;

            // Map values to list node so we can find the "destination" node.
            Dictionary<int, LinkedListNode<int>> valueMap = new Dictionary<int, LinkedListNode<int>>();
            while (current != null) {
                valueMap[current.Value] = current;
                current = current.Next;
            }
            
            current = circle.First;

            // These are slow so keep out of the loop!
            int minValue = valueMap.Keys.Min();
            int maxValue = valueMap.Keys.Max();

            for (long move=0; move<nMoves; move++) {
                // Get the three nodes clockwise, wrapping round to the first
                LinkedListNode<int>[] clockwise = new LinkedListNode<int>[3];
                LinkedListNode<int> n = current;
                for (int i=0;i<3;i++) {
                    n = n.Next ?? circle.First;
                    clockwise[i] = n;
                }

                // Remove them from the circle
                foreach (var c in clockwise) {
                    circle.Remove(c);
                }

                // Find the destination
                int v = current.Value;
                do {
                    v = (v > minValue) ? v - 1 : maxValue;
                } while (clockwise.Where(c=>c.Value==v).Count() > 0 || v == current.Value);
                
                LinkedListNode<int> destination = valueMap[v];

                // Insert at the destination
                for (int i=0; i<3; i++) {
                    circle.AddAfter(destination, clockwise[i]);
                    destination = destination.Next ?? circle.First;
                }

                current = current.Next ?? circle.First;                
            }
        }
    }
}
