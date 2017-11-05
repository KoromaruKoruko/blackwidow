using System;
using System.Linq;

namespace BDLib.Text
{
    public static class Hex
    {
        /// <summary>
        /// converts a byte array to a hex string
        /// </summary>
        /// <param name="x">byte array to convert</param>
        /// <returns>hex string of x</returns>
        public static string ToHEX(byte[] x)
        {
            return BitConverter.ToString(x)
                                .Replace("-", "")
                                .ToLower();
        }
        /// <summary>
        /// converts a hex string to a byte array
        /// </summary>
        /// <param name="hex">hex string to convert</param>
        /// <returns>byte array of hex</returns>
        public static byte[] ToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}