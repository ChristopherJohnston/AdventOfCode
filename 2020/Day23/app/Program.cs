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
            // input = "389125467";
            LinkedList<int> circle = new LinkedList<int>(input.Select(c => int.Parse(c.ToString())));
            LinkedListNode<int> current = circle.First;

            // Map values to list node so we can find the "destination" node.
            Dictionary<int, LinkedListNode<int>> valueMap = new Dictionary<int, LinkedListNode<int>>();
            while (current != null) {
                valueMap[current.Value] = current;
                current = current.Next;
            }

            current = circle.First;

            for (int move=0; move<100; move++) {

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
                    v = (v > valueMap.Keys.Min()) ? v - 1 : valueMap.Keys.Max();
                } while (clockwise.Where(c=>c.Value==v).Count() > 0 || v == current.Value);
                
                LinkedListNode<int> destination = valueMap[v];

                // Insert at the destination
                for (int i=0; i<3; i++) {
                    circle.AddAfter(destination, clockwise[i]);
                    destination = destination.Next ?? circle.First;
                }

                current = current.Next ?? circle.First;                
            }

            // Get order from 1
            LinkedListNode<int> node = valueMap[1].Next ?? circle.First;
            for (int i=0; i<valueMap.Keys.Count-1; i++) {
                Console.WriteLine(node.Value);
                node=node.Next ?? circle.First;
            }
        }
    }
}
