using System;
using BDLib.BDLibInfo;

namespace BDLib.DataTypes
{

    public class S_Int
    {
        public static class Usermessages
        {
            public const string MaxValue = "the max value is a number with 2,147,483,647 digits so ye knock your self out";
            public const string MinValue = "the max value is a number with 2,147,483,647 digits so ye knock your self out";
        }
        public const S_Int Empty = null;
        private char[] Data = new char[] { '0' };

        public S_Int()
        {
            if (!Info.Moduls.Contains("DataTypes/S_Int.cs"))
                Info.Moduls.Add("DataTypes/S_Int.cs");
        } //do nothing
        public S_Int(short x)
        {
            if (!Info.Moduls.Contains("DataTypes/S_Int.cs"))
                Info.Moduls.Add("DataTypes/S_Int.cs");
            Data = x.ToString().ToCharArray();
        }
        public S_Int(int x)
        {
            if (!Info.Moduls.Contains("DataTypes/S_Int.cs"))
                Info.Moduls.Add("DataTypes/S_Int.cs");
            Data = x.ToString().ToCharArray();
        }
        public S_Int(long x)
        {
            if (!Info.Moduls.Contains("DataTypes/S_Int.cs"))
                Info.Moduls.Add("DataTypes/S_Int.cs");
            Data = x.ToString().ToCharArray();
        }

        public void add(int x)
        {
            Data = (this + x).Data;//cheep add to self
        }
        public void add(S_Int x)
        {
            Data = (this + x).Data;//cheep add to self
        }
        public void minus(int x)
        {
            Data = (this - x).Data;//cheep add to self
        }
        public void minus(S_Int x)
        {
            Data = (this - x).Data;//cheep add to self
        }

        public static S_Int Parse(byte x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        }   //8
        public static S_Int Parse(short x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        }  //16
        public static S_Int Parse(ushort x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        } //16
        public static S_Int Parse(int x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        }    //32
        public static S_Int Parse(uint x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        }   //32
        public static S_Int Parse(long x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        }   //64
        public static S_Int Parse(ulong x)
        {
            S_Int a = new S_Int();
            a.Data = x.ToString().ToCharArray();
            return a;
        }  //64
        public static S_Int Parse(string x)
        {
            for(int pos = 0; pos < x.Length; pos++)
            {
                switch(x[pos])
                {
                    case ('0'):
                    case ('1'):
                    case ('2'):
                    case ('3'):
                    case ('4'):
                    case ('5'):
                    case ('6'):
                    case ('7'):
                    case ('8'):
                    case ('9'):
                        break;

                    default:
                        throw new Exception("THIS IS NOT A NUMBER IS HAS WORDS");
                }
            }

            //if here x is only numbers
            S_Int o = new S_Int();
            o.Data = x.ToCharArray();
            return o;

        } //#

        public override string ToString()
        {
            string output = "";
            for(long x = 0; x < Data.Length; x++)
                output += Data[x].ToString();

            return output;
        }

        public bool Equals(S_Int x)
        {
           Data = x.Data;
           return true;
        }
        public bool Equals(byte x)
        {
            Data = x.ToString().ToCharArray();
            return true;
        }
        public bool Equals(short x)
        {
            Data = x.ToString().ToCharArray();
            return true;
        }
        public bool Equals(int x)
        {
            Data = x.ToString().ToCharArray();
            return true;
        }
        public bool Equals(long x)
        {
            Data = x.ToString().ToCharArray();
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }      //remove green marker (i hate it)
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }//remove green marker (i hate it)


        public static S_Int operator +(S_Int a, S_Int b)
        {
            return comb(a, b);
        }

        public static S_Int operator -(S_Int a, S_Int b)
        {
            return part(a, b);
        }

        public static S_Int operator +(S_Int a, int b)
        {
            return comb(a, Parse(b));
        }

        public static S_Int operator -(S_Int a, int b)
        {
            return part(a, Parse(b));
        }

        public static S_Int operator +(int b, S_Int a)
        {
            return comb(a, Parse(b));
        }

        public static S_Int operator -(int b, S_Int a)
        {
            return part(a, Parse(b));
        }

        private static S_Int comb(S_Int a, S_Int b)
        {
            int carry = 0;
            string output = "*";
            for (long x = 0; x <= a.Data.Length; x++)
            {
                if (x == a.Data.Length)
                {
                    if (carry > 0)
                    {
                        output = carry.ToString() + output;//finnish up add remainder to end (sould only be 1 digit large)
                        break;
                    }
                }
                else
                {
                    int num = carry;//num is carry + db + da : d=digit a=input_a b=input_b carry=RamainderOfLast-d

                    if (b.Data.Length > x)
                    {
                        num += int.Parse(b.Data[x].ToString());
                    }
                    num += int.Parse(a.Data[x].ToString());

                    if (num >= 10)
                    {
                        num -= 10;
                        carry = 1;
                    }
                    else carry = 0;
                    output = num.ToString() + output;//add digit
                }
            }
            S_Int o = new S_Int();
            o.Data = output.Replace("*","").ToCharArray();
            return o;
        }
        private static S_Int part(S_Int a, S_Int b)
        {
            short carry = 0;
            string output = "";
            for (long x = 0; x < a.Data.Length + 1; x++)
            {
                if (x == b.Data.Length)
                {
                    if (carry != 0)
                        throw new Exception("negativ numbers are not implamented yet");
                    break;
                }
                else
                {
                    short num = carry;//num is carry + db + da : d=digit a=input_a b=input_b carry=RamainderOfLast-d

                    if (b.Data.Length < x)
                    {
                        num -= short.Parse(b.Data[x].ToString());
                    }
                    num += short.Parse(a.Data[x].ToString());
                    

                    if (num < 0)
                    {
                        num += 10;
                        carry = -1;
                    }
                    output = num.ToString() + output;//add digit
                }
            }
            S_Int o = new S_Int();
            o.Data = output.ToCharArray();
            return o;
        }

        public static S_Int operator /(S_Int a, S_Int b)
        {
            S_Int o = new S_Int();
            for(long x = 0;  x < a.Data.Length; x++)
            {
                if (a - o > b)
                {
                    o += b;
                }
                else break;
            }


            return o;
        }
        public static S_Int operator *(S_Int a, S_Int b)
        {
            S_Int o = new S_Int();
            for(long x = 0; x < b.Data.Length; x++)
            {
                o += a;
            }
            return o;
        }
        public static S_Int operator /(S_Int a, int b)
        {
            S_Int o = new S_Int();
            for (long x = 0; x < a.Data.Length; x++)
            {
                if (a - o > b)
                {
                    o += b;
                }
                else break;
            }


            return o;
        }
        public static S_Int operator *(S_Int a, int b)
        {
            S_Int o = new S_Int();
            for (int x = 0; x < b; x++)
            {
                o += a;
            }
            return o;
        }
        public static S_Int operator /(int b, S_Int a)
        {
            S_Int o = new S_Int();
            for (long x = 0; x < a.Data.Length; x++)
            {
                if (a - o > b)
                {
                    o += b;
                }
                else break;
            }


            return o;
        }
        public static S_Int operator *(int b, S_Int a)
        {
            S_Int o = new S_Int();
            for (int x = 0; x < b; x++)
            {
                o += a;
            }
            return o;
        }


        public static bool operator ==(S_Int a, S_Int b)
        {
            return (a.ToString() == b.ToString());
        }
        public static bool operator !=(S_Int a, S_Int b)
        {
            return !(a == b);
        }
        public static bool operator ==(S_Int a, int b)
        {
            return (a.ToString() == b.ToString());
        }
        public static bool operator !=(S_Int a, int b)
        {
            return !(a==b);
        }
        public static bool operator ==(int b, S_Int a)
        {
            return (a.ToString() == b.ToString());
        }
        public static bool operator !=(int b, S_Int a)
        {
            return !(a == b);
        }

        public static bool operator >(S_Int a, S_Int b)
        {
            if (a.Data.Length != b.Data.Length)
            {
                if (a.Data.Length > b.Data.Length)
                    return true;
                if (a.Data.Length < b.Data.Length)
                    return false;
            }
            for (long x = 0; x < b.Data.Length; x++)
            {
                if (b.Data[x] != a.Data[x])
                {
                    return (int.Parse(a.Data[x].ToString()) > int.Parse(b.Data[x].ToString()));
                }
            }

            return false;
        }
        public static bool operator <(S_Int a, S_Int b)
        {
            if (a.Data.Length != b.Data.Length)
            {
                if (a.Data.Length > b.Data.Length)
                    return false;
                if (a.Data.Length < b.Data.Length)
                    return true;
            }
            for (long x = 0; x < b.Data.Length; x++)
            {
                if (b.Data[x] != a.Data[x])
                {
                    return (int.Parse(a.Data[x].ToString()) < int.Parse(b.Data[x].ToString()));
                }
            }

            return false;
        }
        public static bool operator >(S_Int a, int b)
        {
            char[] bdata = b.ToString().ToCharArray();

            if (a.Data.Length != bdata.Length)
            {
                if (a.Data.Length > bdata.Length)
                    return true;
                if (a.Data.Length < bdata.Length)
                    return false;
            }
            for (long x = 0; x < bdata.Length; x++)
            {
                if (bdata[x] != a.Data[x])
                {
                    return (int.Parse(a.Data[x].ToString()) > int.Parse(bdata[x].ToString()));
                }
            }

            return false;
        }
        public static bool operator <(S_Int a, int b)
        {
            char[] bdata = b.ToString().ToCharArray();

            if (a.Data.Length != bdata.Length)
            {
                if (a.Data.Length > bdata.Length)
                    return false;
                if (a.Data.Length < bdata.Length)
                    return true;
            }
            for (long x = 0; x < bdata.Length; x++)
            {
                if(bdata[x] != a.Data[x])
                {
                    return (int.Parse(a.Data[x].ToString()) < int.Parse(bdata[x].ToString()));
                }
            }

            return false;
        }
        public static bool operator >(int b, S_Int a)
        {
            char[] bdata = b.ToString().ToCharArray();

            if (a.Data.Length != bdata.Length)
            {
                if (a.Data.Length > bdata.Length)
                    return true;
                if (a.Data.Length < bdata.Length)
                    return false;
            }
            for (long x = 0; x < bdata.Length; x++)
            {
                if (bdata[x] != a.Data[x])
                {
                    return (int.Parse(bdata[x].ToString()) > int.Parse(a.Data[x].ToString()));
                }
            }

            return false;
        }
        public static bool operator <(int b, S_Int a)
        {
            char[] bdata = b.ToString().ToCharArray();

            if (a.Data.Length != bdata.Length)
            {
                if (a.Data.Length > bdata.Length)
                    return false;
                if (a.Data.Length < bdata.Length)
                    return true;
            }
            for (long x = 0; x < bdata.Length; x++)
            {
                if (bdata[x] != a.Data[x])
                {
                    return (int.Parse(bdata[x].ToString()) < int.Parse(a.Data[x].ToString()));
                }
            }

            return false;
        }

        public static S_Int operator %(S_Int a, S_Int b)
        {
            S_Int o = a / b;//divide it 'it auto rounds'
            o = a - (o * b);//take away o*b from a becaouse if o is a remainder IE a/b is not a whole number then we will subtrace the closest number to a from a so that the remainder is left IE diferance between closest rounded and beginning number
            return o;
        }
    }
}
