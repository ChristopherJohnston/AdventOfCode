using System;
using System.Collections.Generic;

namespace Common {
    public static class StringExtensions {
        ///
        /// <summary>Converts the string from binary to an integer (e.g. 1101 -> 13)</summary>
        ///
        public static int BinaryToInt32(this string str) {
            return Convert.ToInt32(str, 2);
        }

        ///
        /// <summary>Writes the string to console.</summary>
        ///
        public static void ToConsole(this string str) {
            Console.WriteLine(str);
        }

        public static IEnumerable<T> SplitToList<T>(this string str, char separator=',') {
            List<T> output = new List<T>();
            foreach (string val in str.Split(separator)) {
                output.Add((T)Convert.ChangeType(val, typeof(T)));
            }
            return output;
        }
    }
}