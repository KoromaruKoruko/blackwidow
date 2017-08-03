
using System;

namespace BDLib.Text
{
    public static class Hex
    {
        public static string ToHEX(byte[] x)
        {
            return BitConverter.ToString(x).Replace("-", "").ToLower();
        }
    }
}