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
            if (SizeInBytes < 4)
                throw new ArgumentException("Size must be grater than Int");

            Data = new byte[SizeInBytes];
            SignPos = SizeInBytes / 2;
        }
        private DynamicInt(byte[] PreData)
        {
            if (PreData.Length % 2 != 0)
                throw new ArgumentException("Size must be divisable by 2");
            if (PreData.Length < 4)
                throw new ArgumentException("Size must be grater than Int");

            Data = PreData;
            SignPos = (uint)PreData.Length / 2;
        }


        //open operations
        public void Add(uint Number)
        {
            byte[] buf = BitConverter.GetBytes(Number);
            Data = add(ToME(buf,false)).Data;
        }
        public void TakeAway(uint Number)
        {
            byte[] buf = BitConverter.GetBytes(Number);
            Data = mins(ToME(buf, false)).Data;
        }

        //inner workings
        private byte[] ToME(byte[] x, bool isNeg)
        {
            //x is 4
            byte[] f = new byte[Data.Length];
            Array.Copy(x, 0, f, (isNeg) ? 0 : SignPos, 4);
            return f;
        }
        private DynamicInt ToMe(bool[] x)
        {
            byte[] f = new byte[(x.Length / 8)];
            (new BitArray(x)).CopyTo(f, 0);
            return new DynamicInt(f);

        }

        private DynamicInt add(byte[] x)
        {
            //x is Data.length ie this Ints Size
            bool carry = false;
            bool[] outputs = new bool[Data.Length*8];

            for (int c = 0; c < Data.Length; c++)
            {
                for (int y = 0; y < 8; y++)
                {
                    bool X_bit  = (x[c] & (1 << y)) != 0;
                    bool MY_bit = ((Data[c] & (1 << y)) != 0);
                    int f = 0;

                    if (MY_bit) f++;
                    if (X_bit) f++;
                    if (carry) f++;
                    bool output = false;

                    switch(f)
                    {
                        case (0):
                            output = false;
                            carry = false;
                            break;

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

            return ToMe(outputs);
        }
        private DynamicInt mins(byte[] x)
        {
            throw new NotImplementedException("comming soon");
            bool[] outputs = new bool[Data.Length * 8];
            bool carry = false;

            for (int c = Data.Length-1; c > 0; c--)
            {
                for (int y = 7; y > 0; y--)
                {
                    bool X_bit = (x[c] & (1 << y)) != 0;
                    bool MY_bit = ((Data[c] & (1 << y)) != 0);
                    int f = 0;

                    if (MY_bit) f++;
                    if (X_bit) f++;
                    if (carry) f++;
                    bool output = false;

                    switch (f)
                    {
                        case (0):
                            output = false;
                            carry = false;
                            break;

                        case (1):
                            if (X_bit)
                            {
                                output = false;
                                carry = true;
                            }
                            else if(MY_bit)
                            {
                                output = true;
                                carry = false;
                            }
                            else if(carry && c<SignPos)
                            {
                                carry = false;
                                output = true;
                            }
                            break;

                        case (2):
                            if (carry && MY_bit)
                                output = false;
                            else
                                output = true;
                            carry = false;
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
            
            return ToMe(outputs);
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
            return a.add(b.Data);
        }
        public static DynamicInt operator -(DynamicInt a, DynamicInt b)
        {
            return a.mins(b.Data);
        }

        public static DynamicInt operator +(DynamicInt a, Int64 b)
        {
            if (b < 0)
                return a.mins(a.ToME(BitConverter.GetBytes(-b), true));
            else
                return a.add(a.ToME(BitConverter.GetBytes(b), false));
        }
        public static DynamicInt operator -(DynamicInt a, Int64 b)
        {
            if (b > 0)
                return a.mins(a.ToME(BitConverter.GetBytes(-b), true));
            else
                return a.add(a.ToME(BitConverter.GetBytes(b), false));
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
