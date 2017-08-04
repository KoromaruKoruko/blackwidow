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
            if (SizeInBytes < 8)
                throw new ArgumentException("Size must be grater than Int");

            Data = new byte[SizeInBytes];
            SignPos = SizeInBytes / 2;
        }
        private DynamicInt(byte[] PreData)
        {
            if (PreData.Length % 2 != 0)
                throw new ArgumentException("Size must be divisable by 2");
            if (PreData.Length < 8)
                throw new ArgumentException("Size must be grater than Int");

            Data = PreData;
            SignPos = (uint)PreData.Length / 2;
        }


        //open operations
        public void Add(uint Number)
        {
            Data = (this + Number).Data;
        }
        public void TakeAway(uint Number)
        {
            Data = (this - Number).Data;
        }
        public void Add(DynamicInt Number)
        {
            Data = (this + Number).Data;
        }
        public void TakeAway(DynamicInt Number)
        {
            Data = (this - Number).Data;
        }
        public void Set(Int32 Number)
        {
            if (Number > 0)
                Data = ToME(BitConverter.GetBytes(Number), false);
            else
                Data = ToME(BitConverter.GetBytes((-Number)), true);
        }

        //inner workings
        private byte[] ToME(byte[] x, bool isNeg)
        {
            byte[] f = new byte[Data.Length];
            Array.Copy(x, 0, f, (isNeg) ? 0 : SignPos, x.Length);
            return f;
        }
        private DynamicInt ToME(bool[] x)
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
                            output = true;
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

            return ToME(outputs);
        }
        private DynamicInt mins(byte[] x)
        {
            //throw new NotImplementedException("comming soon");
            bool[] outputs = new bool[Data.Length * 8];

            //get OnesComplament
            for (int c = 0; c < Data.Length; c++)
            {
                for (int y = 7; y > -1; y--)
                {
                    outputs[(c * 8) + y] = ((x[c] & (1 << y)) != 0) ? false : true;
                }
            }

            //get TwosComplament

            bool[] tmp = new bool[Data.Length * 8];
            for(int c = 0; c < tmp.Length; c++)
            {
                    tmp[c] = (c == ((Data.Length * 8) / 2) + 1);
            }
            x = ToME(tmp).Data;
            tmp = null;

            bool carry = false;
            for (int c = 0; c < Data.Length; c++)
            {
                for (int y = 7; y < -1; y++)
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
                            output = true;
                            carry = false;
                            break;

                        case (2):
                            output = false;
                            carry = true;
                            break;

                        case (3):
                            output = true;
                            carry = true;
                            break;

                        default:
                            throw new Exception("WTF happend??");
                    }
                    outputs[(c * 8) + y] = output;
                }
            }

            x = ToME(outputs).Data;

            //do math
            carry = false;
            for (int c = 0; c < Data.Length; c++)
            {
                for (int y = 7; y < -1; y++)
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
                            output = true;
                            carry = false;
                            break;

                        case (2):
                            output = false;
                            carry = true;
                            break;

                        case (3):
                            output = true;
                            carry = true;
                            break;

                        default:
                            throw new Exception("WTF happend??");
                    }
                    outputs[(c * 8) + y] = output;
                }
            }

            //flip result

            for (int c = 0; c < Data.Length*8; c++)
            {
                outputs[c] = (outputs[c]) ? false : true;
            }

            return ToME(outputs);
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

        public static DynamicInt operator +(DynamicInt a, int b)
        {
            if (b > 0)
                return a.add(a.ToME(BitConverter.GetBytes(b), false));
            else if (b < 0)
                return a.mins(a.ToME(BitConverter.GetBytes((-b)), false));
            else return a;
        }
        public static DynamicInt operator -(DynamicInt a, int b)
        {
            if (b > 0)
                return a.mins(a.ToME(BitConverter.GetBytes(b), false));
            else if (b < 0)
                return a.add(a.ToME(BitConverter.GetBytes((-b)), false));
            else return a;
        }

        public static DynamicInt operator +(DynamicInt a, uint b)
        {
            if (b != 0)
                return a.add(a.ToME(BitConverter.GetBytes(b), false));
            else
                return a;
        }
        public static DynamicInt operator -(DynamicInt a, uint b)
        {
            if (b != 0)
                return a.mins(a.ToME(BitConverter.GetBytes(b), false));
            else
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

