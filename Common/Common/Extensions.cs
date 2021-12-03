using System;

namespace Common {
    public static class StringExtensions {
        public static int BinaryToInt32(this string str) {
            return Convert.ToInt32(str, 2);
        }
    }
}