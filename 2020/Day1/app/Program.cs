using System;
using System.Collections.Generic;
using System.IO;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            var target = 2020;

            // Open the file
            var file = File.OpenText(@"input.txt");
            var lines = file.ReadToEnd().Split("\n");
            file.Close();

            // Read the values into an integer list
            List<int> values = new List<int>();
            foreach (var line in lines) {
                values.Add(Int32.Parse(line));
            }

            // Sort the list
            values.Sort();

            var arr = values.ToArray();

            // iterate
            for (var i=0; i<arr.Length; i++)
            {
                var v1 = arr[i];

                for (var j=i; j<arr.Length; j++) {
                    var v2 = arr[j];

                    if (v1+v2 == target) {
                        Console.WriteLine("Result is {0}", v1*v2);
                        return;
                    }
                    else if (v1+v2 > target) {
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
