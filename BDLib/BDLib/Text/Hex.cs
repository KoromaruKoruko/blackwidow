using System;
using System.Linq;

namespace BDLib.Text
{
    public static class Hex
    {
        public static string ToHEX(byte[] x)
        {
            return BitConverter.ToString(x)
                               .Replace("-", "")
                               .ToLower();
        }
        public static byte[] ToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
