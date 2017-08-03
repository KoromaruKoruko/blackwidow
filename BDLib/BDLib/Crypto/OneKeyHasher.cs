using System;
using System.Text;

namespace BDLib.Crypto.Hash
{
    class OneKeyHasher
    {
        private static Encoding ENCODER = Encoding.ASCII;
        private static uint HashSize = 64;

        public static uint TheHashSize
        {
            get { return HashSize;}
            set
            {
                if (value > 32)
                {
                    HashSize = value;
                }
                else
                    throw new ArgumentException("min size is 32Bytes");
            }
        }

        private static Int64 GetOffset(string Text)
        {
            Int64 x = 1;
            byte[] B_Message = ENCODER.GetBytes(Text);
            Array.Reverse(B_Message);

            for (int y = 0; y < B_Message.Length; y++)
            {
                x += B_Message[y] * x / (y + 1) + x;
            }

            return x % 1248901232;
        }
        private static byte Translate(UInt64 Key, UInt64 Pass, char Pk)
        {
            byte[] Buf = new byte[8];
            UInt64 i = (((Key + Pass * 12 / 54) ^ Key)) % 8;


            Buf[i] = ENCODER.GetBytes(Pk.ToString())[0];

            UInt64 PK = BitConverter.ToUInt64(Buf, 0);

            UInt64 deff = (((Key ^ (PK / 100) / 1354) + Pass) % 256);


            switch (Pk)
            {
                case ('#'):
                    return Byte.Parse(((Key ^ PK + Pass + 531) % 256).ToString());

                case ('@'):
                    return Byte.Parse(((Key ^ PK + Pass * 64) % 256).ToString());

                case ('j'):
                    return Byte.Parse(((Key ^ PK + Pass / 5436) % 256).ToString());

                case ('ß'):
                    return Byte.Parse(((Key ^ PK + Pass + 245134) % 256).ToString());

                case ('.'):
                    return Byte.Parse(((Key ^ PK + Pass / (i + 1)) % 256).ToString());

                case ('ö'):
                    return Byte.Parse(((Key ^ PK + Pass + 83413) % 256).ToString());

                case ('3'):
                    return Byte.Parse(((Key ^ PK + Pass + 9341) % 256).ToString());

                case ('a'):
                    return Byte.Parse(((Key ^ PK + Pass * 1234) % 256).ToString());

                case ('i'):
                    return Byte.Parse(((Key ^ PK + Pass + 945) % 256).ToString());

                case ('o'):
                    return Byte.Parse(((Key ^ PK + Pass ^ 9) % 256).ToString());

                case ('0'):
                    return Byte.Parse(((Key ^ PK + Pass + 92 - i + 123) % 256).ToString());

                case ('m'):
                    return Byte.Parse(((Key ^ PK + Pass * i + 1) % 256).ToString());

                case (' '):
                    return Byte.Parse(((Key ^ PK + Pass * 83) % 256).ToString());


                default:
                    return byte.Parse(deff.ToString());
            }
        }
        public static byte[] Hash(string Text)
        {
            Int64 Offset = GetOffset(Text);

            UInt64 Key = (ulong)GetOffset(Text);

            byte[] Output = new byte[HashSize];
            byte newOutput = 0;

            Array.Reverse(Output);


            for (int I = 0; I < HashSize; I++)
            {
                for (int y = 0; y < Text.Length; y++)
                {
                    for (int x = 0; x < I + y; x++)
                    {
                        newOutput = Translate((Key + (ulong)(y + x)), (ulong)((Offset * (HashSize - (I + 1)) + 5) / ((y + 1) + (newOutput ^ newOutput + 1) + Text.Length)), Text[y]);
                    }
                }
                Output[I] = newOutput;
            }

            return Output;
        }
    }
}