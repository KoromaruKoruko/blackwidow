using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Advdupe
{
    public enum AdvDupeObjectType
    {
        Null = 0,
        String,
        Number,
        Array,
        Table,
        Vector,
        Angle,
        Boolean,
    }
    public class AdvDupeObject
    {
        public readonly AdvDupeObjectType Type;
        // All Data Types Here Can Be Unallocated
        private Vector _TypeVec;
        private Angle _TypeAng;
        private AdvDupeObject [ ] _TypeArray;
        private Table _TypeTable;
        private Boolean? _TypeBoolean;
        private Double? _TypeDouble;
        private String _TypeString;

#warning untested auto refrance resolving here!
        public Vector TypeVec
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeVec;
                else
                    return this._Ref.Refrance.TypeVec;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeVec = value;
                else
                    this._Ref.Refrance.TypeVec = value;
            }
        }
        public Angle TypeAng
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeAng;
                else
                    return this._Ref.Refrance.TypeAng;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeAng = value;
                else
                    this._Ref.Refrance.TypeAng = value;
            }
        }
        public AdvDupeObject [ ] TypeArray
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeArray;
                else
                    return this._Ref.Refrance.TypeArray;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeArray = value;
                else
                    this._Ref.Refrance.TypeArray = value;
            }
        }
        public Table TypeTable
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeTable;
                else
                    return this._Ref.Refrance.TypeTable;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeTable = value;
                else
                    this._Ref.Refrance.TypeTable = value;
            }
        }
        public Boolean? TypeBoolean
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeBoolean;
                else
                    return this._Ref.Refrance.TypeBoolean;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeBoolean = value;
                else
                    this._Ref.Refrance.TypeBoolean = value;
            }
        }
        public Double? TypeDouble
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeDouble;
                else
                    return this._Ref.Refrance.TypeDouble;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeDouble = value;
                else
                    this._Ref.Refrance.TypeDouble = value;
            }
        }
        public String TypeString
        {
            get
            {
                if (this._Ref is null)
                    return this._TypeString;
                else
                    return this._Ref.Refrance.TypeString;
            }
            set
            {
                if (this._Ref is null)
                    this._TypeString = value;
                else
                    this._Ref.Refrance.TypeString = value;
            }
        }

        public ref Vector RefTypeVec
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeVec;
                else
                    return ref this._Ref.Refrance.RefTypeVec;
            }
        }
        public ref Angle RefTypeAng
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeAng;
                else
                    return ref this._Ref.Refrance.RefTypeAng;
            }
        }
        public ref AdvDupeObject [ ] RefTypeArray
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeArray;
                else
                    return ref this._Ref.Refrance.RefTypeArray;
            }
        }
        public ref Table RefTypeTable
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeTable;
                else
                    return ref this._Ref.Refrance.RefTypeTable;
            }
        }
        public ref Boolean? RefTypeBoolean
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeBoolean;
                else
                    return ref this._Ref.Refrance.RefTypeBoolean;
            }
        }
        public ref Double? RefTypeDouble
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeDouble;
                else
                    return ref this._Ref.Refrance.RefTypeDouble;
            }
        }
        public ref String RefTypeString
        {
            get
            {
                if (this._Ref is null)
                    return ref this._TypeString;
                else
                    return ref this._Ref.Refrance.RefTypeString;
            }
        }

        private readonly AdvDupeRefrence _Ref;

#warning TODO: make non generic Constructors

        public AdvDupeObject(Object BaseObj = null)
        {
            if (BaseObj is null)
            {
                this.Type = AdvDupeObjectType.Null;
            }
            else
            {
                Type BT = BaseObj.GetType();

                if (BT == typeof(Angle))
                {
                    this.Type = AdvDupeObjectType.Angle;
                    this._TypeAng = (Angle)BaseObj;
                }
                else if (BT == typeof(Vector))
                {
                    this.Type = AdvDupeObjectType.Vector;
                    this._TypeVec = (Vector)BaseObj;
                }
                else if (BT == typeof(AdvDupeObject [ ]))
                {
                    this.Type = AdvDupeObjectType.Array;
                    this._TypeArray = (AdvDupeObject [ ])BaseObj;
                }
                else if (BT == typeof(Table))
                {
                    this.Type = AdvDupeObjectType.Table;
                    this._TypeTable = (Table)BaseObj;
                }
                else if (BT == typeof(Boolean))
                {
                    this.Type = AdvDupeObjectType.Boolean;
                    this._TypeBoolean = (Boolean)BaseObj;
                }
                else if (BT == typeof(Double))
                {
                    this.Type = AdvDupeObjectType.Number;
                    this._TypeDouble = (Double)BaseObj;
                }
                else if (BT == typeof(String))
                {
                    this.Type = AdvDupeObjectType.String;
                    this._TypeString = (String)BaseObj;
                }
                else if (BT == typeof(AdvDupeRefrence))
                {
                    AdvDupeRefrence Ref = (AdvDupeRefrence)BaseObj;
                    this.Type = Ref.Refrance.Type;
                    this._Ref = Ref;
                }
                else
                    throw new InvalidCastException();
            }
        }
        public AdvDupeObject(Double BaseObj)
        {
            this.Type = AdvDupeObjectType.Number;
            this._TypeDouble = BaseObj;
        }
        public AdvDupeObject(Vector BaseObj)
        {
            this.Type = AdvDupeObjectType.Vector;
            this._TypeVec = BaseObj;
        }
        public AdvDupeObject(Boolean BaseObj)
        {
            this.Type = AdvDupeObjectType.Boolean;
            this._TypeBoolean = BaseObj;
        }
        public AdvDupeObject(Angle BaseObj)
        {
            this.Type = AdvDupeObjectType.Angle;
            this._TypeAng = BaseObj;
        }
        public AdvDupeObject(String BaseObj)
        {
            this.Type = AdvDupeObjectType.String;
            this._TypeString = BaseObj;
        }
        public AdvDupeObject(Table BaseObj)
        {
            this.Type = AdvDupeObjectType.Table;
            this._TypeTable = BaseObj;
        }
        public AdvDupeObject(AdvDupeObject [ ] BaseObj)
        {
            this.Type = AdvDupeObjectType.Array;
            this._TypeArray = BaseObj;
        }

        public Object GetRaw()
        {
            switch (this.Type)
            {
                case AdvDupeObjectType.Angle:
                return this.TypeAng;

                case AdvDupeObjectType.Array:
                return this.TypeArray;

                case AdvDupeObjectType.Boolean:
                return this.TypeBoolean;

                case AdvDupeObjectType.Number:
                return this.TypeDouble;

                case AdvDupeObjectType.String:
                return this.TypeString;

                case AdvDupeObjectType.Table:
                return this.TypeTable;

                case AdvDupeObjectType.Vector:
                return this.TypeVec;

                default:
                return null;
            }
        }

        public override String ToString() => this.GetRaw().ToString();
        public override Boolean Equals(Object obj) => this.GetRaw().Equals(obj);
        public override Int32 GetHashCode() => this.GetRaw().GetHashCode();

#warning TODO: work on operators

        public static Boolean operator ==(AdvDupeObject left, AdvDupeObject right)
        {
            if (right is null)
                return left.GetRaw() == null;
            if (left is null)
                return right.GetRaw() == null;
            return left.GetRaw() == right.GetRaw();
        }
        public static Boolean operator ==(AdvDupeObject left, Object right) =>
            left.GetRaw() == right;
        public static Boolean operator !=(AdvDupeObject left, AdvDupeObject right)
        {
            if (right is null)
                return left.GetRaw() != null;
            if (left is null)
                return right.GetRaw() != null;
            return left.GetRaw() != right.GetRaw();
        }
        public static Boolean operator !=(AdvDupeObject left, Object right) =>
            left.GetRaw() != right;
    }

    public struct DupeInfo
    {
        public Int32 size;
        public Int32 Revision;
        public Int32 DateTimeOffSet;
        public String date;
        public String time;
        public String creator;
        public String FilePath;
        public Dictionary<String, String> ExtraHeaderVars;
    }
    public class Dupe
    {
        public DupeInfo Info;
        public Table DupeData;
        private AdvDupeObject [ ] RefrancedObjects;
        public Dupe(String FilePath)
        {
            FileStream FSTM;
            try
            {
                FSTM = File.OpenRead(FilePath);
            }
            catch
            {
                throw new FileNotAvailableException("Dupe Dupe.c_tor(String)");
            }
            using (FSTM)
            {
                this.Info = new DupeInfo
                {
                    ExtraHeaderVars = new Dictionary<String, String>(),
                    FilePath = FilePath
                };
                {
                    Byte [ ] buffer = new Byte [ 5 ];
                    if (FSTM.Read(buffer, 0, 5) != 5)
                        throw new FileNotDupeFileException("Dupe Dupe.c_tor(String)");
                    if (buffer [ 0 ] == 'A' && buffer [ 1 ] == 'D' && buffer [ 2 ] == '2' && buffer [ 3 ] == 'F')
                        this.Info.Revision = buffer [ 4 ];
                    else if (buffer [ 0 ] == '[' && buffer [ 1 ] == 'I' && buffer [ 2 ] == 'n' && buffer [ 3 ] == 'f')
#warning unknown outcome HERE!
                        this.Info.Revision = 0 | buffer [ 4 ];
                    else
                        throw new FileNotDupeFileException("Dupe Dupe.c_tor(String)");
                } // validate magic number
                while (true)
                {
                    String Key = "";
                    String Var = "";
                    Int32 x = FSTM.ReadByte();
                    if (x == 0x02)
                    {
                        FSTM.Position = FSTM.Length - this.Info.size;
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
                    switch (Key.Trim('\n'))
                    {
                        case "size":
                        this.Info.size = Int32.Parse(Var);
                        continue;
                        case "timezone":
                        this.Info.DateTimeOffSet = Int32.Parse(Var);
                        continue;
                        case "date":
                        this.Info.date = Var;
                        continue;
                        case "time":
                        this.Info.time = Var;
                        continue;
                        case "name":
                        this.Info.creator = Var;
                        continue;

                        default:
                        this.Info.ExtraHeaderVars.Add(Key, Var);
                        continue;
                    }
                } // get header infomation
                this.RefrancedObjects = new AdvDupeObject [ 0 ];
                switch (this.Info.Revision)
                {
                    case 4:
                    {
                        AdvancedDupeRevisions.Revision4.DeSerilize(FSTM, this);
                    }
                    break;

                    default:
                    throw new AdvancedDupeRevisionNotSupported("Dupe.c_tor(String)");
                }
            }
        }
        private static class AdvancedDupeRevisions
        {
            public const Byte Revision = 5;
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
            public static class Revision4
            {
                public static void DeSerilize(FileStream FSTM, Dupe dupe)
                {
                    Byte [ ] RawData = DeCompress(FSTM);
                    Int32 Index = 0;
                    AdvDupeObject Obj = Read(RawData, ref Index, dupe);
                    if (Obj.Type != AdvDupeObjectType.Table)
                        throw new DataMalFormedException("Dupe.AdvancedDupeRevisions.Revision4.DeSerilize(FileStream,Dupe)");
                    dupe.DupeData = Obj.TypeTable;
                }
                private static AdvDupeObject Read(Byte [ ] b, ref Int32 index, Dupe dupe)
                {
                    index++;
                    switch (b [ index - 1 ])
                    {
                        case 0:
                        return null;

                        case 255:
                        return getTable(b, ref index, dupe);

                        case 254:
                        return getArray(b, ref index, dupe);

                        case 253:
                        return getBooleanT(b, ref index);

                        case 252:
                        return getBooleanF(b, ref index);

                        case 251:
                        return getDouble(b, ref index);

                        case 250:
                        return getVector(b, ref index);

                        case 249:
                        return GetAngle(b, ref index);

                        case 248:
                        return GetString(b, ref index);

                        case 247:
                        return GetTableRef(b, ref index, dupe);

                        default:
                        return GetUndef(b, ref index);
                    }
                }
                private static AdvDupeObject getTable(Byte [ ] b, ref Int32 index, Dupe dupe)
                {
                    AdvDupeObject t = new AdvDupeObject(new Table());
                    Object Key = null;
                    do
                    {
                        Key = Read(b, ref index, dupe);
                        if (Key != null)
                            t.TypeTable.Add(Key.ToString(), Read(b, ref index, dupe));
                    } while (Key != null);
                    Int32 Size = dupe.RefrancedObjects.Length;
                    Array.Resize(ref dupe.RefrancedObjects, Size + 1);
                    dupe.RefrancedObjects [ Size ] = t;
                    return new AdvDupeObject(new AdvDupeRefrence(dupe.RefrancedObjects [ Size ]));
                }//255
                private static AdvDupeObject getArray(Byte [ ] b, ref Int32 index, Dupe dupe)
                {
                    AdvDupeObject a = new AdvDupeObject(new AdvDupeObject [ 0 ]);
                    AdvDupeObject Var = null;
                    do
                    {
                        Var = Read(b, ref index, dupe);
                        if (Var != null)
                        {
                            Int32 size = a.TypeArray.Length;
                            Array.Resize(ref a.RefTypeArray, size + 1);
                            a.TypeArray [ size ] = Var;
                        }
                    } while (Var == null);
                    Int32 Size = dupe.RefrancedObjects.Length;
                    Array.Resize(ref dupe.RefrancedObjects, Size + 1);
                    dupe.RefrancedObjects [ Size ] = a;
                    return new AdvDupeObject(new AdvDupeRefrence(dupe.RefrancedObjects [ Size ]));
                }// 254
                private static AdvDupeObject getBooleanT(Byte [ ] b, ref Int32 index) => new AdvDupeObject(true);// 253
                private static AdvDupeObject getBooleanF(Byte [ ] b, ref Int32 index) => new AdvDupeObject(false);// 252
                private static AdvDupeObject getDouble(Byte [ ] b, ref Int32 index)
                {
                    AdvDupeObject outp = new AdvDupeObject(BitConverter.ToDouble(b, index));
                    index += 8;
                    return outp;
                }// 251
                private static AdvDupeObject getVector(Byte [ ] b, ref Int32 index)
                {
                    Double X = BitConverter.ToDouble(b, index);
                    index += 8;
                    Double Y = BitConverter.ToDouble(b, index);
                    index += 8;
                    Double Z = BitConverter.ToDouble(b, index);
                    index += 8;

                    return new AdvDupeObject(new Vector(X, Y, Z));
                }// 250
                private static AdvDupeObject GetAngle(Byte [ ] b, ref Int32 index)
                {
                    Double Pitch = BitConverter.ToDouble(b, index);
                    index += 8;
                    Double Yaw = BitConverter.ToDouble(b, index);
                    index += 8;
                    Double Roll = BitConverter.ToDouble(b, index);
                    index += 8;

                    return new AdvDupeObject(new Angle(Pitch, Yaw, Roll));
                }// 249
                private static AdvDupeObject GetString(Byte [ ] b, ref Int32 index)
                {
                    AdvDupeObject outp = new AdvDupeObject("");
                    while (true)
                    {
                        if (b [ index ] == 0)
                            break;
                        outp.TypeString += (Char)b [ index ];
                        index++;
                    }
                    index++;
                    return outp;
                }// 248
                private static AdvDupeObject GetTableRef(Byte [ ] b, ref Int32 index, Dupe dupe)
                {
                    Int16 Id = BitConverter.ToInt16(b, index);
                    index += sizeof(Int16);

                    return new AdvDupeObject(new AdvDupeRefrence(dupe.RefrancedObjects [ Id ]));
                } // 247
                private static AdvDupeObject GetUndef(Byte [ ] b, ref Int32 index)
                {
                    Byte [ ] buffer = new Byte [ b [ index - 1 ] ];
                    Array.Copy(b, index, buffer, 0, buffer.Length);
                    index += buffer.Length;
                    AdvDupeObject Var = new AdvDupeObject("");
                    foreach (Byte bu in buffer)
                        Var.TypeString += (Char)bu;
                    return Var;
                }//0 - 246
            }// COMPLEATE!
            private static class Revision5
            {
                public static void DeSerilize(FileStream FSTM, ref Dupe dupe) => throw new NotImplementedException();
            }
            public static Byte [ ] DeCompress(FileStream FSTM)
            {
                try
                {
                    using (MemoryStream MemStm = new MemoryStream())
                    {
                        SevenZip.Compression.LZMA.Decoder Decoder = new SevenZip.Compression.LZMA.Decoder();
                        Byte [ ] properties = new Byte [ 5 ];
                        FSTM.Read(properties, 0, 5);
                        Byte [ ] fileLengthBytes = new Byte [ 8 ];
                        FSTM.Read(fileLengthBytes, 0, 8);
                        Decoder.SetDecoderProperties(properties);
                        long len = BitConverter.ToInt64(fileLengthBytes, 0);
                        if (len == 0)
                            throw new DataMalFormedException("Byte[] Dupe.AdvancedDupeRevisions.DeCompress(System.IO.FileStream)");
                        Decoder.Code(FSTM, MemStm, FSTM.Position - FSTM.Length, len, null);
                        return MemStm.ToArray();
                    }
                }
                catch (AdvancedDupeLibraryException)
                {
                    throw;
                }
                catch
                {
                    throw new LMZAException();
                }
            }
        }
    }
    public class Table
    {
        public AdvDupeObject this [ String Key ]
        {
            get
            {
                if (Key is null)
                    return new AdvDupeObject();
                for (Int32 x = 0; x < this._Keys.Length; x++)
                {
                    if (this._Keys [ x ] == Key)
                        return this._Values [ x ];
                }
                return new AdvDupeObject();
            }
        }
        public AdvDupeObject this [ Int32 index ]
        {
            get
            {
                if (index < 0)
                    return new AdvDupeObject();
                AdvDupeObject Obj = this._Values [ index ];
                if (Obj is null)
                    return new AdvDupeObject();
                else
                    return Obj;
            }
        }

        public Int32 Count => this._Keys.Length;
        public Int32 Length => this._Keys.Length;

        public String [ ] Keys
        {
            get
            {
                String [ ] Copy_Keys = new String [ this._Keys.Length ];
                this._Keys.CopyTo(Copy_Keys, 0);
                return Copy_Keys;
            }
        }
        public AdvDupeObject [ ] Values
        {
            get
            {
                AdvDupeObject [ ] Copy_Values = new AdvDupeObject [ this._Values.Length ];
                this._Values.CopyTo(Copy_Values, 0);
                return Copy_Values;
            }
        }

        private String [ ] _Keys;
        private AdvDupeObject [ ] _Values;
        private Task CleanUp;

        public Table()
        {
            this._Keys = new String [ 0 ];
            this._Values = new AdvDupeObject [ 0 ];
            this.CleanUp = new Task(this.CleanArraysUp);
        }
        public void Add(String Key, AdvDupeObject Value)
        {
            Int32 ind = this._Keys.Length;
            Array.Resize(ref this._Keys, ind + 1);
            Array.Resize(ref this._Values, this._Keys.Length);

            this._Keys [ ind ] = Key;
            this._Values [ ind ] = Value;
        }
        public void Remove(String Key)
        {
            for (Int32 x = 0; x < this._Keys.Length; x++)
                if (this._Keys [ x ] == Key)
                {
                    this._Keys [ x ] = null;
                    this._Values [ x ] = null;
                    lock (this.CleanUp)
                        if (this.CleanUp.Status != TaskStatus.Running)
                        {
                            if (this.CleanUp.Status == TaskStatus.WaitingForActivation)
                                this.CleanUp = new Task(this.CleanArraysUp);
                            this.CleanUp.Start();
                        }
                    break;
                }
        }
        private void CleanArraysUp()// Remove null spaces
        {
            Int32 [ ] Indexes = new Int32 [ 0 ];
            Int32 LowestIndex = Int32.MaxValue;
            Int32 HighestIndex = -1;
            for (Int32 x = 0; x < this._Keys.Length; x++)
            {
                if (this._Keys [ x ] == null)
                {
                    if (x > HighestIndex)
                        HighestIndex = x;
                    if (x < LowestIndex)
                        LowestIndex = x;
                    Int32 Len = Indexes.Length;
                    Array.Resize(ref Indexes, Len + 1);
                    Indexes [ Len ] = x;
                }
            } // scan for blanks
            Int32 NewLen = this._Keys.Length - Indexes.Length;
            String [ ] Copy_Keys = new String [ NewLen ];
            AdvDupeObject [ ] Copy_Values = new AdvDupeObject [ NewLen ];
            switch (Indexes.Length)
            {
                case 1:
                if (HighestIndex == NewLen) // remove last object
                {
                    Array.Copy(this._Keys, 0, Copy_Keys, 0, NewLen);
                    Array.Copy(this._Values, 0, Copy_Values, 0, NewLen);
                    break;
                }
                if (LowestIndex == 0) // remove first object
                {
                    Array.Copy(this._Keys, 1, Copy_Keys, 0, NewLen);
                    Array.Copy(this._Values, 1, Copy_Values, 0, NewLen);
                    break;
                }
                {
                    Int32 pos = 0;
                    Int32 ind = 0;
                    for (; ind < this._Keys.Length; ind++)
                        if (ind == LowestIndex)
                            break;
                        else
                        {
                            Copy_Keys [ ind ] = this._Keys [ pos ];
                            Copy_Values [ ind ] = this._Values [ pos ];
                            pos++;
                        }
                    Int32 rest = Copy_Keys.Length - pos;
                    Array.Copy(this._Keys, ind, Copy_Keys, pos, rest);
                    Array.Copy(this._Values, ind, Copy_Values, pos, rest);
                }
                break;

                case 2:
                if (HighestIndex == NewLen && LowestIndex == 0) // remove last object
                {
                    Array.Copy(this._Keys, 0, Copy_Keys, 0, NewLen);
                    Array.Copy(this._Values, 0, Copy_Values, 0, NewLen);
                    break;
                }
                {
                    Int32 pos = 0;
                    Int32 ind = 0;
                    for (; ind < this._Keys.Length; ind++)
                    {
                        if (ind == LowestIndex)
                            break;
                        else
                        {
                            Copy_Keys [ pos ] = this._Keys [ ind ];
                            Copy_Values [ pos ] = this._Values [ ind ];
                            pos++;
                        }
                    }
                    for (; ind < this._Keys.Length; ind++)
                    {
                        if (ind == HighestIndex)
                            break;
                        else
                        {
                            Copy_Keys [ pos ] = this._Keys [ ind ];
                            Copy_Values [ pos ] = this._Values [ ind ];
                            pos++;
                        }
                    }
                    Int32 rest = Copy_Keys.Length - pos;
                    Array.Copy(this._Keys, ind, Copy_Keys, pos, rest);
                    Array.Copy(this._Values, ind, Copy_Values, pos, rest);
                }
                break;

                default:
                {
                    Boolean W;
                    for (Int32 ind = 0, pos = 0; ind < this._Keys.Length; ind++)
                    {
                        W = true;
                        for (Int32 x = 0; x < Indexes.Length; x++)
                            if (Indexes [ x ] == ind)
                            {
                                W = false;
                                break;
                            }
                        if (!W)
                            continue;
                        Copy_Keys [ pos ] = this._Keys [ ind ];
                        Copy_Values [ pos ] = this._Values [ pos ];
                        pos++;
                    }
                }
                break;
            } // remove blanks
            lock (this._Keys)
            {
                this._Keys = Copy_Keys;
                this._Values = Copy_Values;
            }
        }
    }
    public class Vector
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
    public class Angle
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
    public class AdvDupeRefrence
    {
        public AdvDupeRefrence(AdvDupeObject obj) => this._Ref = new WeakReference<AdvDupeObject>(obj);
        private readonly WeakReference<AdvDupeObject> _Ref;
        public AdvDupeObject Refrance
        {
            get
            {
                if (this._Ref.TryGetTarget(out AdvDupeObject obj))
                    return obj;
                else
                    throw new DataObjectDisposedException("AdvDupeObject DupeRefrence.Refrance._get()");
            }
        }

        public override String ToString() => this.Refrance.ToString();
    }

    #region Exceptions
    public enum AdvancedDupeErrorCodes : Byte
    {
        FileMalFormed = 13,
        FileNotDupeFile = 14,
        FileNotAvailable = 15,
        TypeUnknown = 16,
        RevisionUnSupported = 17,
        LibraryOutDated = 18,
        LMZA_Failed = 20,

        TableDisposed = 50,
        RefranceDisposed = 51,
    }

    public class AdvancedDupeLibraryException : Exception
    {
        public AdvancedDupeLibraryException(AdvancedDupeErrorCodes Code, String Message, String Function) : base($"ErrorCode:\"{Code.ToString()}:{(Byte)Code}\" InFunc:\"{Function}\"\n Message:\"{Message}\"")
        {
        }
    }
    public class DataMalFormedException : AdvancedDupeLibraryException
    {
        public DataMalFormedException(String Function) : base(AdvancedDupeErrorCodes.FileMalFormed, "the data is missing KEY data points!", Function)
        {

        }
    }
    public class DataTableDisposedException : AdvancedDupeLibraryException
    {
        public DataTableDisposedException(String Function) : base(AdvancedDupeErrorCodes.TableDisposed, "Table Doesn't Exist no more, All Containing Data was released!", Function)
        {

        }
    }
    public class DataObjectDisposedException : AdvancedDupeLibraryException
    {
        public DataObjectDisposedException(String Function) : base(AdvancedDupeErrorCodes.RefranceDisposed, "the Refranced Object Doesn't Exist no more, All Containing Data was released!", Function)
        {

        }
    }
    public class AdvancedDupeRevisionNotSupported : AdvancedDupeLibraryException
    {
        public AdvancedDupeRevisionNotSupported(String Function) : base(AdvancedDupeErrorCodes.RevisionUnSupported, "this revision is unsupported!", Function)
        {

        }
    }
    public class LMZAException : AdvancedDupeLibraryException
    {
        public LMZAException() : base(AdvancedDupeErrorCodes.LMZA_Failed, "Failed to uncompress dupe data", "Byte[] AdvancedDupe.DeCompress(System.IO.FileStream)")
        {

        }
    }
    public class FileNotAvailableException : AdvancedDupeLibraryException
    {
        public FileNotAvailableException(String Function) : base(AdvancedDupeErrorCodes.FileNotAvailable, "Unable to access file", Function)
        {

        }
    }
    public class FileNotDupeFileException : AdvancedDupeLibraryException
    {
        public FileNotDupeFileException(String Function) : base(AdvancedDupeErrorCodes.FileNotDupeFile, "this is not a valid dupe file", Function)
        {

        }
    }
    #endregion

}
