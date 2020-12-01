using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            var target = 2020;

            // Open the file and create an array of figures
            List<int> lines = File.ReadAllLines(@"input.txt").Select(int.Parse).ToList();

            lines.Sort();
            int[] arr = lines.ToArray();
            iterate(target, arr);       

            int[] values = lines.ToArray();
            HashSet<int> h = new HashSet<int>(values);
            find(target, values, h);
            find3(target, values, h);
        }

        static void find(int target, int[] arr, HashSet<int> h) {
            foreach (var i in arr) {
                var remainder = target - i;
                if (h.Contains(remainder)) {
                    Console.WriteLine("Result is {0} * {1} = {2}", i, remainder, i*remainder);
                    break;
                }
            }
        }

        static void find3(int target, int[] arr, HashSet<int> h) {
            for (int i=0; i<arr.Length; i++) {
                var n1 = arr[i];
                var remainder = target - n1;
                for (int j=i; j<arr.Length; j++) {
                    var n2 = arr[j];
                    if (h.Contains(remainder - n2)) {
                        Console.WriteLine("Result is {0} * {1} * {2} = {3}", n1, n2, remainder-n2, n1*n2*(remainder-n2));
                        return;
                    }
                }
            }
        }

        static void iterate(int target, int[] arr) {
            // nested iteration to find the result
            for (var i=0; i<arr.Length; i++)
            {
                var v1 = arr[i];

                for (var j=i; j<arr.Length; j++) {
                    var v2 = arr[j];

                    for (var k=j; k<arr.Length; k++) {
                        var v3 = arr[k];

                        if (v1+v2+v3 == target) {
                            Console.WriteLine("Result is {0} * {1} * {2} = {3}", v1, v2, v3, v1*v2*v3);
                            return;
                        }
                        else if (v1+v2+v3 > target) {
                            break;
                        }
                    }

                    if (v1+v2 > target) {
                        break;
                    }
                }

                if (v1 > target) {
                    Console.WriteLine("There is no answer");
                    return;
                }
            }
        }
    }
}
