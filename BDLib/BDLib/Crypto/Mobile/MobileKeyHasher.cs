using System;
using System.Text;
using BDLib.Text;

namespace BDLib.Crypto.Mobile
{
    public class MobileKeyHasher
    {
        private static readonly Encoding ECode = Encoding.UTF8;
        private int Layers;
        private int HashSize;
        public MobileKeyHasher(int Layers, int HashSize)
        {
            this.Layers = Layers;
            this.HashSize = HashSize;
        }
        private byte[] Hash(byte[] input)
        {
            for (int x = 0; x < Layers; x++)
            {
                for (int y = 0; y < HashSize; y++)
                {
                    for (int z = 0; z < input.Length; z++)
                    {
                        input[y] = (byte)(((((input[(123 + z << 2) % input.Length] + x - y * z) + (input[y] * 12)) << input[(1 << (x % 16)) % input.Length]) + (1231251 << (input[0] % 16) | 123541)) % 256);
                    }
                }
            }
            return input;
        }

        public byte[] Compute(byte[] x)
        {
            return Hash(x);
        }
        public byte[] Compute(String x)
        {
            return Hash(ECode.GetBytes(x));
        }

        public string ComputeHex(String x)
        {
            return Hex.ToHEX(Hash(ECode.GetBytes(x)));
        }
        public string ComputeHex(byte[] x)
        {
            return Hex.ToHEX(Hash(x));
        }
    }
}
