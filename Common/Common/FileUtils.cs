using System;
using System.IO;
using System.Collections.Generic;

namespace Common
{
    public static class FileUtils
    {
        public static IEnumerable<string> ParseInput(string file) {
            foreach (string line in File.ReadAllLines(file)) {
                yield return line;
            }
        }

        public static void SayHello() {
            Console.WriteLine("Hello");
        }
    }
}
