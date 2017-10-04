using System.IO;
using BDLib.BDLibInfo;
using BDLib.Text;
using System;

namespace BDLib.Crypto.Hash
{
    public static class BDFileCheckSum
    {
        public static byte[] Get(FileStream STM, int HashSize, int offset)
        {
            long pos = STM.Position;
            STM.Position = offset;

            byte l = 0;
            byte[] PrimeEndings = new byte[] { 1, 3, 7, 9 };
            byte[] output = new byte[HashSize];

            while (STM.Position - STM.Length > 0)
            {
                int off = STM.ReadByte();
                for(int x = 0; x < HashSize; x++)
                {
                    if (l > PrimeEndings.Length) l = 0;

                    for (int y = 0; y < PrimeEndings[l]*off; x++)
                    {
                        output[x] = ByteHelpers.ByteStepUpTranslator[output[x]];
                    }

                    l++;
                }
            }
            STM.Position = pos;

            Array.Reverse(output);

            return output;
        }
    }
}
