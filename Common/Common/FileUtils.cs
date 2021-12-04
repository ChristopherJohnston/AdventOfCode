using System;
using System.IO;
using System.Collections.Generic;

namespace Common
{
    public static class FileUtils
    {
        ///
        /// <summary>Parses the input file line by line</summary>
        /// <param name="file">Path to the input file.</param>
        ///
        public static IEnumerable<string> ParseInput(string file) {
            foreach (string line in File.ReadAllLines(file)) {
                yield return line;
            }
        }
    }
}
