using System;
using System.Text;
using BDLib.Crypto.Hash;

namespace BDLib.Crypto
{
    public static class BDKeyGen//this is for creating secure and close to random keys to use for BDSocket or other Crypto Systems
    {
        public static string CreateStringKey()
        {
            return CreateStringKey(1024, 100);
        }
        public static string CreateStringKey(int len)
        {
            byte[] KK = new byte[len];
            Random RND = new Random();
            RND.NextBytes(KK);
            OneKeyHasher HS = new OneKeyHasher() { TheCharsetUsed = Encoding.ASCII, TheHashSize = len * 24 };
            KK = HS.Hash(KK);
            HS.TheHashSize = len;
            KK = HS.Hash(KK);
            string K = Encoding.UTF8.GetString(KK);
            KK = null;//free mem
            for(int x = 0; x < len; x++)
                K += RND.Next();
            return Encoding.Default.GetString(HS.Hash(K));
        }
        public static string CreateStringKey(int len, int Complexity)
        {
            Random RND = new Random();
            int stairs = RND.Next() / ((Complexity+4)/4);
            int doors = RND.Next();
            int Energy = RND.Next() * 3;
            string output = "";
            OneKeyHasher HS = new OneKeyHasher() { TheCharsetUsed = Encoding.ASCII, TheHashSize = 1 };
            for(int y = 0; y < stairs || Energy <= 0; y++)
            {
                for(int x = 0; x < doors || Energy <= 0; x++, Energy--)
                {
                    byte[] ff = new byte[Complexity];
                    RND.NextBytes(ff);
                    output += (char)HS.Hash(ff)[0];
                }
            }

            HS.TheHashSize = len;
            return Encoding.ASCII.GetString(HS.Hash(output));
        }

        public static byte[] CreateKey()
        {
            return CreateKey(1024, 100);
        }
        public static byte[] CreateKey(int len)
        {
            byte[] KK = new byte[len];
            Random RND = new Random();
            RND.NextBytes(KK);
            OneKeyHasher HS = new OneKeyHasher() { TheCharsetUsed = Encoding.ASCII, TheHashSize = len * 24 };
            KK = HS.Hash(KK);
            HS.TheHashSize = len;
            KK = HS.Hash(KK);
            string K = Encoding.UTF8.GetString(KK);
            KK = null;//free mem
            for (int x = 0; x < len; x++)
                K += RND.Next();
            return HS.Hash(K);
        }
        public static byte[] CreateKey(int len, int Complexity)
        {
            Random RND = new Random();
            int stairs = RND.Next() / ((Complexity + 4) / 4);
            int doors = RND.Next();
            int Energy = RND.Next() * 3;
            string output = "";
            OneKeyHasher HS = new OneKeyHasher() { TheCharsetUsed = Encoding.ASCII, TheHashSize = 1 };
            for (int y = 0; y < stairs || Energy <= 0; y++)
            {
                for (int x = 0; x < doors || Energy <= 0; x++, Energy--)
                {
                    byte[] ff = new byte[Complexity];
                    RND.NextBytes(ff);
                    output += (char)HS.Hash(ff)[0];
                }
            }

            HS.TheHashSize = len;
            return HS.Hash(output);
        }
    }
}
