using System;
using System.Collections;

namespace BDLib.DataTypes
{
    public class DynamicInt
    {
        //actuall Data
        private byte[] Data;
        private uint SignPos;

        //constructors
        public DynamicInt(uint SizeInBytes)
        {
            if (SizeInBytes % 2 != 0)
                throw new ArgumentException("Size must be divisable by 2");

            Data = new byte[SizeInBytes];
            SignPos = SizeInBytes / 2;
        }

        //open operations
        public void Add(Int64 Number)
        {
            byte[] buf = BitConverter.GetBytes(Number);
            byte[] Lower = new byte[4];
            byte[] Upper = new byte[4];
            Array.Copy(buf, Lower, 4);
            Array.Copy(buf, 4, Upper, 0, 4);
            bool isMinus = true;

            foreach (byte b in Upper)
                if (b > 0)
                    isMinus = false;

            if (isMinus)
                mins(ToME(Lower,true));
            else
                add(ToME(Upper,false));
        }
        public void TakeAway(Int64 Number)
        {
            byte[] buf = BitConverter.GetBytes(Number);
            byte[] Lower = new byte[4];
            byte[] Upper = new byte[4];
            Array.Copy(buf, Lower, 4);
            Array.Copy(buf, 4, Upper, 0, 4);
            bool isMinus = true;

            foreach (byte b in Upper)
                if (b > 0)
                    isMinus = false;

            if (isMinus)
                mins(ToME(Upper, false));
            else
                add(ToME(Lower, true));
        }

        //inner workings
        private byte[] ToME(byte[] x, bool isNeg)
        {
            //x is 4
            byte[] f = new byte[Data.Length];
            Array.Copy(x, 0, f, (isNeg) ? 0 : SignPos, 4);
            return f;
        }
        private void   IsME(bool[] x)
        {
            (new BitArray(x)).CopyTo(Data,0);
        }
        private void add(byte[] x)
        {
            //x is Data.length ie this Ints Size
            bool carry = false;
            bool[] outputs = new bool[Data.Length*8];

            for (int c = 0; c < Data.Length; c++)
            {
                for (int y = 0; y < 8; y++)
                {
                    bool X_bit  = (x[c] & (1 << y)) != 0;
                    bool MY_bit = ((Data[SignPos + c] & (1 << y)) != 0);
                    int f = 0;

                    if (MY_bit) f++;
                    if (X_bit) f++;
                    if (MY_bit) f++;
                    bool output = false;

                    switch(f)
                    {
                        case(1):
                            output = true;
                            carry = false;
                            break;

                        case (2):
                            output = false;
                            carry = true;
                            break;

                        case (3):
                            output = false;
                            carry = true;
                            break;

                        default:
                            throw new Exception("WTF happend??");
                    }
                    outputs[(c * 8) + y] = output;
                }
            }
            if (carry)
                throw new OverflowException();

            IsME(outputs);
        }
        private void mins(byte[] x)
        {
            //x is Data.length ie this Ints Size
            throw new NotImplementedException("comming soon, as soon as i can figure out a way to do it in code");
            bool[] outputs = new bool[Data.Length * 8];

            //CODE

            IsME(outputs);
        }

        //overrides
        public override string ToString()
        {
            string output = "";

            for(int x = 0; x < Data.Length; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    output += ((Data[x] & (1 << y)) != 0) ? "1" : "0";
                }
            }
            return output;
        }


        //operators
        public static DynamicInt operator +(DynamicInt a, DynamicInt b)
        {
            a.add(b.Data);
            return a;
        }
        public static DynamicInt operator -(DynamicInt a, DynamicInt b)
        {
            a.mins(b.Data);
            return a;
        }

        public static DynamicInt operator +(DynamicInt a, Int64 b)
        {
            a.Add(b);
            return a;
        }
        public static DynamicInt operator -(DynamicInt a, Int64 b)
        {
            a.TakeAway(b);
            return a;
        }

        public static bool       operator <(DynamicInt a, DynamicInt b)
        {
            UInt64 o = 0, p = 0;
            
            for(int x = a.Data.Length-1; x > 0; x--)
            {
                for(int y = 7; y > 0; y--)
                {
                    if((a.Data[x] & (1 << y)) != 0) o = UInt64.Parse(((x*8)+y).ToString());
                }
            }
            for (int x = b.Data.Length - 1; x > 0; x--)
            {
                for (int y = 7; y > 0; y--)
                {
                    if ((b.Data[x] & (1 << y)) != 0) p = UInt64.Parse(((x * 8) + y).ToString());
                }
            }

            return o < p;
        }
        public static bool       operator >(DynamicInt a, DynamicInt b)
        {
            UInt64 o = 0, p = 0;

            for (int x = a.Data.Length - 1; x > 0; x--)
            {
                for (int y = 7; y > 0; y--)
                {
                    if ((a.Data[x] & (1 << y)) != 0) o = UInt64.Parse(((x * 8) + y).ToString());
                }
            }
            for (int x = b.Data.Length - 1; x > 0; x--)
            {
                for (int y = 7; y > 0; y--)
                {
                    if ((b.Data[x] & (1 << y)) != 0) p = UInt64.Parse(((x * 8) + y).ToString());
                }
            }

            return o > p;
        }
    }
}
