using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Advdupe
{
    public struct Dupe
    {
        public Boolean Success;
        public Int32 size;
        public Int32 DateTimeOffSet;
        public String date;
        public String time;
        public String creator;
        public Dictionary<String, String> ExtraHeaderVars;
        public List<TableItem> Refrances;
        public TableItem MainItem;
    }
    public struct TableItem
    {
        public TableItem(Object obj) => this.Data = obj;
        public Object Data;
    }
    public struct Vector
    {
        public Vector(Double X, Double Y, Double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Double X, Y, Z;

        public override String ToString() => $"{this.X.ToString("N8").Replace(',', '.')}, {this.Y.ToString("N8").Replace(',', '.')}, {this.Z.ToString("N8").Replace(',', '.')}";
    }
    public struct Angle
    {
        public Angle(Double Pitch, Double Yaw, Double Roll)
        {
            this.Pitch = Pitch;
            this.Yaw = Yaw;
            this.Roll = Roll;
        }
        public Double Pitch, Yaw, Roll;

        public override String ToString() => $"{this.Pitch.ToString("N8").Replace(',', '.')}, {this.Yaw.ToString("N8").Replace(',', '.')}, {this.Roll.ToString("N8").Replace(',', '.')}";
    }
    public static class AdvancedDupe
    {
        public const Byte Revision = 5;
        public static Dupe GetDupe(FileStream FSTM, Byte Revision)
        {
            Dupe dupe = new Dupe
            {
                ExtraHeaderVars = new Dictionary<String, String>()
            };
            if (FSTM.ReadByte() == 0x0a)
            {
                while (true)
                {
                    String Key = "";
                    String Var = "";
                    Int32 x = FSTM.ReadByte();
                    if (x == 0x02)
                    {
                        FSTM.Position = FSTM.Length - dupe.size;
                        break;
                    }
                    else
                        Key += (Char)(Byte)x;

                    while (true)
                    {
                        x = FSTM.ReadByte();
                        if (x == 0x01)
                            break;
                        else
                            Key += (Char)(Byte)x;
                    }
                    while (true)
                    {
                        x = FSTM.ReadByte();
                        if (x == 0x01)
                            break;
                        else
                            Var += (Char)(Byte)x;
                    }

                    switch (Key)
                    {
                        case "size":
                        dupe.size = Int32.Parse(Var);
                        continue;
                        case "timezone":
                        dupe.DateTimeOffSet = Int32.Parse(Var);
                        continue;
                        case "date":
                        dupe.date = Var;
                        continue;
                        case "time":
                        dupe.time = Var;
                        continue;
                        case "name":
                        dupe.creator = Var;
                        continue;

                        default:
                        dupe.ExtraHeaderVars.Add(Key, Var);
                        continue;
                    }
                }
            }
            else
                throw new InvalidDataException("file stream not at header var flag");
            switch (Revision)
            {
                case 0:
                return Revision0.DeSerilize(FSTM);

                case 1:
                return Revision1.DeSerilize(FSTM);

                case 2:
                return Revision2.DeSerilize(FSTM);

                case 3:
                return Revision3.DeSerilize(FSTM);

                case 4:
                Revision4.DeSerilize(FSTM, ref dupe);
                break;

                case 5:
                Revision5.DeSerilize(FSTM, ref dupe);
                break;

                default:
                throw new IndexOutOfRangeException("Revision not supported");
            }
            return dupe;
        }
        private static class Revision0
        {
            public static Dupe DeSerilize(FileStream FSTM) => throw new NotImplementedException();
        }
        private static class Revision1
        {
            public static Dupe DeSerilize(FileStream FSTM) => throw new NotImplementedException();
        }
        private static class Revision2
        {
            public static Dupe DeSerilize(FileStream FSTM) => throw new NotImplementedException();
        }
        private static class Revision3
        {
            public static Dupe DeSerilize(FileStream FSTM) => throw new NotImplementedException();
        }
        private static class Revision4
        {
            public static void DeSerilize(FileStream FSTM, ref Dupe dupe)
            {
                Byte [ ] RawData = DeCompress(FSTM);
                dupe.Success = false;
                dupe.Refrances = new List<TableItem>();
                Int32 Index = 0;
                try
                {
                    dupe.MainItem = new TableItem(Read(RawData, ref Index, ref dupe));
                    dupe.Success = true;
                }
                catch
                {
                    dupe.Success = false;
                }
            }
            private static Object Read(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                index++;
                switch (b [ index - 1 ])
                {
                    case 0:
                    return null;

                    case 255:
                    return getTable(b, ref index, ref dupe);

                    case 254:
                    return getArray(b, ref index, ref dupe);

                    case 253:
                    return getBooleanT(b, ref index, ref dupe);

                    case 252:
                    return getBooleanF(b, ref index, ref dupe);

                    case 251:
                    return getDouble(b, ref index, ref dupe);

                    case 250:
                    return getVector(b, ref index, ref dupe);

                    case 249:
                    return GetAngle(b, ref index, ref dupe);

                    case 248:
                    return GetString(b, ref index, ref dupe);

                    case 247:
                    return GetTableRef(b, ref index, ref dupe);

                    default:
                    return GetUndef(b, ref index, ref dupe);
                }
            }
            private static Object getTable(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                Dictionary<String, Object> Items = new Dictionary<String, Object>();
                Object Key = null;
                do
                {
                    Key = Read(b, ref index, ref dupe);
                    if (Key != null)
                    {
                        Items [ Key.ToString() ] = Read(b, ref index, ref dupe);
                    }
                } while (Key != null);

                dupe.Refrances.Add(new TableItem(Items));
                return Items;
            }//255
            private static Object [ ] getArray(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                List<Object> array = new List<Object>();
                Object Var = null;
                do
                {
                    Var = Read(b, ref index, ref dupe);
                    if (Var != null)
                    {
                        array.Add(Var);
                    }
                } while (Var == null);
                Object [ ] Outp = array.ToArray();
                array = null;
                dupe.Refrances.Add(new TableItem(Outp));
                return Outp;
            }// 254
            private static Boolean getBooleanT(Byte [ ] b, ref Int32 index, ref Dupe dupe) => true;// 253
            private static Boolean getBooleanF(Byte [ ] b, ref Int32 index, ref Dupe dupe) => false;// 252
            private static Double getDouble(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                Double outp = BitConverter.ToDouble(b, index);
                index += 8;
                return outp;
            }// 251
            private static Vector getVector(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                Vector outp = new Vector
                {
                    X = BitConverter.ToDouble(b, index)
                };
                index += 8;
                outp.Y = BitConverter.ToDouble(b, index);
                index += 8;
                outp.Z = BitConverter.ToDouble(b, index);
                index += 8;
                return outp;
            }// 250
            private static Angle GetAngle(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                Angle outp = new Angle
                {
                    Pitch = BitConverter.ToDouble(b, index)
                };
                index += 8;
                outp.Yaw = BitConverter.ToDouble(b, index);
                index += 8;
                outp.Roll = BitConverter.ToDouble(b, index);
                index += 8;
                return outp;
            }// 249
            private static String GetString(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                String outp = "";
                while (true)
                {
                    if (b [ index ] == 0)
                    {
                        break;
                    }
                    outp += (Char)b [ index ];
                    index++;
                }
                index++;
                return outp;
            }// 248
            private static TableItem GetTableRef(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                Int16 Id = BitConverter.ToInt16(b, index);
                index += sizeof(Int16);
                return dupe.Refrances [ Id ];
            } // 247
            private static String GetUndef(Byte [ ] b, ref Int32 index, ref Dupe dupe)
            {
                Byte [ ] buffer = new Byte [ b [ index - 1 ] ];
                Array.Copy(b, index, buffer, 0, buffer.Length);
                index += buffer.Length;
                String Var = "";
                foreach (Byte bu in buffer)
                    Var += (Char)bu;
                return Var;
            }//0 - 246
        }// COMPLEATE!
        private static class Revision5
        {
            public static void DeSerilize(FileStream FSTM, ref Dupe dupe) => throw new NotImplementedException();
        }
        public static Byte [ ] DeCompress(FileStream FSTM)
        {
            using (MemoryStream MemStm = new MemoryStream())
            {
                SevenZip.Compression.LZMA.Decoder Decoder = new SevenZip.Compression.LZMA.Decoder();
                Byte [ ] properties = new Byte [ 5 ];
                FSTM.Read(properties, 0, 5);
                Byte [ ] fileLengthBytes = new Byte [ 8 ];
                FSTM.Read(fileLengthBytes, 0, 8);
                Decoder.SetDecoderProperties(properties);
                Decoder.Code(FSTM, MemStm, FSTM.Position - FSTM.Length, BitConverter.ToInt64(fileLengthBytes, 0), null);
                return MemStm.ToArray();
            }
        }
    }
}
