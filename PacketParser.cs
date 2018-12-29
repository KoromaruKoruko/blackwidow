using System;
using System.Collections.Generic;
using System.Text;

namespace BDMTUS
{
    public enum PacketDataTypes : Byte
    {
        Byte = 0,
        Short = 1,
        Int = 2,
        Long = 3,
        UShort = 4,
        UInt = 5,
        ULong = 6,
        MessageType = 7,
        Version = 8,
        Chunk = 9,
        ByteArray = 9,
        String = 10,
        // 200 + is parser specific 
        Nothing = 254,
        NONE = 255
    }
    public class PacketParser
    {
        public PacketParser(Byte [ ] Packet) => this.Packet = Packet;
        private Int32 _Position = 0;
        private readonly Byte [ ] Packet;
        public Boolean EndOfPacket => this._Position == this.Packet.Length;
        public Int32 Position => _Position;

        public Byte GetByte()
        {
            Byte Outp = this.Packet [ this._Position ];
            this._Position++;
            return Outp;
        }
        public Int16 GetShort()
        {
            Int16 Outp = BitConverter.ToInt16(this.Packet, this._Position);
            this._Position += 2;
            return Outp;
        }
        public Int32 GetInt()
        {
            Int32 Outp = BitConverter.ToInt32(this.Packet, this._Position);
            this._Position += 4;
            return Outp;
        }
        public Int64 GetLong()
        {
            Int64 Outp = BitConverter.ToInt64(this.Packet, this._Position);
            this._Position += 8;
            return Outp;
        }
        public UInt16 GetUShort()
        {
            UInt16 Outp = BitConverter.ToUInt16(this.Packet, this._Position);
            this._Position += 2;
            return Outp;
        }
        public UInt32 GetUInt()
        {
            UInt32 Outp = BitConverter.ToUInt32(this.Packet, this._Position);
            this._Position += 4;
            return Outp;
        }
        public UInt64 GetULong()
        {
            UInt64 Outp = BitConverter.ToUInt64(this.Packet, this._Position);
            this._Position += 8;
            return Outp;
        }

        public MessageType GetMessageType()
        {
            MessageType Outp = (MessageType)this.Packet [ this._Position ];
            this._Position++;
            return Outp;
        }
        public Version GetVersion()
        {
            Version Outp = new Version(BitConverter.ToUInt16(this.Packet, this._Position), BitConverter.ToUInt16(this.Packet, this._Position + 2), BitConverter.ToUInt16(this.Packet, this._Position + 4), BitConverter.ToUInt16(this.Packet, this._Position + 6));
            this._Position += 8;
            return Outp;
        }

        public Byte [ ] GetChunk()
        {
            int size = BitConverter.ToInt32(Packet, _Position);
            _Position += 4;
            byte [ ] Outp = new byte [ size ];
            Array.Copy(Packet, _Position, Outp, 0, size);
            _Position += size;
            return Outp;
        }
        public Byte [ ] GetByteArray()
        {
            int size = BitConverter.ToInt32(Packet, _Position);
            _Position += 4;
            byte [ ] Outp = new byte [ size ];
            Array.Copy(Packet, _Position, Outp, 0, size);
            _Position += size;
            return Outp;
        }

        public String GetString(Encoding Coding = null)
        {
            if (Coding is null)
                Coding = Encoding.UTF8;
            List<Byte> buffer = new List<Byte>();
            while (this._Position < this.Packet.Length)
            {
                if (this.Packet [ this._Position ] != 0)
                {
                    buffer.Add(this.Packet [ this._Position ]);
                    this._Position++;
                }
                else
                    break;
            }
            this._Position++;
            return Coding.GetString(buffer.ToArray());
        }
    }
    public class DynamicSerializedPacketDataParser
    {
        public DynamicSerializedPacketDataParser(Byte [ ] Packet) => this.Packet = Packet;
        private Int32 _Position = 0;
        private readonly Byte [ ] Packet;
        public Boolean EndOfPacket => this._Position == this.Packet.Length;
        public Int32 Position => _Position;
        public byte [ ] LastGetPacketData;


        public Byte GetByte()
        {
            Byte Outp = this.Packet [ this._Position ];
            LastGetPacketData = new byte [ ] { Outp };
            this._Position++;
            return Outp;
        }
        public Int16 GetShort()
        {
            Int16 Outp = BitConverter.ToInt16(this.Packet, this._Position);
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ] };
            this._Position += 2;
            return Outp;
        }
        public Int32 GetInt()
        {
            Int32 Outp = BitConverter.ToInt32(this.Packet, this._Position);
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ], Packet [ _Position + 2 ], Packet [ _Position + 3 ] };
            this._Position += 4;
            return Outp;
        }
        public Int64 GetLong()
        {
            Int64 Outp = BitConverter.ToInt64(this.Packet, this._Position);
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ], Packet [ _Position + 2 ], Packet [ _Position + 3 ], Packet [ _Position + 4 ], Packet [ _Position + 5 ], Packet [ _Position + 6 ], Packet [ _Position + 7 ] };
            this._Position += 8;
            return Outp;
        }
        public UInt16 GetUShort()
        {
            UInt16 Outp = BitConverter.ToUInt16(this.Packet, this._Position);
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ] };
            this._Position += 2;
            return Outp;
        }
        public UInt32 GetUInt()
        {
            UInt32 Outp = BitConverter.ToUInt32(this.Packet, this._Position);
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ], Packet [ _Position + 2 ], Packet [ _Position + 3 ] };
            this._Position += 4;
            return Outp;
        }
        public UInt64 GetULong()
        {
            UInt64 Outp = BitConverter.ToUInt64(this.Packet, this._Position);
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ], Packet [ _Position + 2 ], Packet [ _Position + 3 ], Packet [ _Position + 4 ], Packet [ _Position + 5 ], Packet [ _Position + 6 ], Packet [ _Position + 7 ] };
            this._Position += 8;
            return Outp;
        }

        public MessageType GetMessageType()
        {
            MessageType Outp = (MessageType)this.Packet [ this._Position ];
            LastGetPacketData = new byte [ ] { Packet [ _Position ] };
            this._Position++;
            return Outp;
        }
        public Version GetVersion()
        {
            Version Outp = new Version(BitConverter.ToUInt16(this.Packet, this._Position), BitConverter.ToUInt16(this.Packet, this._Position + 2), BitConverter.ToUInt16(this.Packet, this._Position + 4), BitConverter.ToUInt16(this.Packet, this._Position + 6));
            LastGetPacketData = new byte [ ] { Packet [ _Position ], Packet [ _Position + 1 ], Packet [ _Position + 2 ], Packet [ _Position + 3 ], Packet [ _Position + 4 ], Packet [ _Position + 5 ], Packet [ _Position + 6 ], Packet [ _Position + 7 ] };
            this._Position += 8;
            return Outp;
        }

        public Byte [ ] GetChunk()
        {
            int size = BitConverter.ToInt32(Packet, _Position);
            _Position += 4;
            byte [ ] Outp = new byte [ size ];
            Array.Copy(Packet, _Position, Outp, 0, size);
            _Position += size;
            LastGetPacketData = Outp;
            return Outp;
        }
        public Byte [ ] GetByteArray()
        {
            int size = BitConverter.ToInt32(Packet, _Position);
            _Position += 4;
            byte [ ] Outp = new byte [ size ];
            Array.Copy(Packet, _Position, Outp, 0, size);
            _Position += size;
            LastGetPacketData = Outp;
            return Outp;
        }

        public String GetString(Encoding Coding = null)
        {
            if (Coding is null)
                Coding = Encoding.UTF8;
            List<Byte> buffer = new List<Byte>();
            while (this._Position < this.Packet.Length)
            {
                if (this.Packet [ this._Position ] != 0)
                {
                    buffer.Add(this.Packet [ this._Position ]);
                    this._Position++;
                }
                else
                    break;
            }
            this._Position++;
            LastGetPacketData = buffer.ToArray();
            return Coding.GetString(LastGetPacketData);
        }
    }

    public struct SerializedValue
    {
        public SerializedValue(Object Data, PacketDataTypes Type)
        {
            this.Data = Data;
            this.Type = Type;
        }
        public readonly PacketDataTypes Type;
        public readonly Object Data;
    }
    public class SerializedPacket
    {
        [System.Runtime.CompilerServices.IndexerName("ArgPos")]
        public object this [ Int32 index ] => this._Data [ index ].Data;
        public SerializedValue [ ] Data => _Data;
        private SerializedValue [ ] _Data;


        public SerializedPacket(Byte [ ] Packet, Byte [ ] Template)
        {
            DecompressPacket(Packet, Template);
        }
        public SerializedPacket(Byte [ ] PacketWithEmbededTemplate)
        {
            PacketParser Parser = new PacketParser(PacketWithEmbededTemplate);
            DecompressPacket(null, Parser.GetChunk(), Parser);
        }
        public SerializedPacket(SerializedValue [ ] Data)
        {
            this._Data = Data;
        }
        private void DecompressPacket(byte [ ] Packet, byte [ ] Template, PacketParser Parser = null)
        {
            if (Parser == null)
                Parser = new PacketParser(Packet);

            int TemplatePosition = 0;
            _Data = new SerializedValue [ Template.Length ];
            while (!Parser.EndOfPacket && TemplatePosition < Template.Length)
            {
                switch (Template [ TemplatePosition ])
                {
                    case 0:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetByte(), PacketDataTypes.Byte);
                    break;
                    case 1:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetShort(), PacketDataTypes.Short);
                    break;
                    case 2:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetInt(), PacketDataTypes.Int);
                    break;
                    case 3:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetLong(), PacketDataTypes.Long);
                    break;
                    case 4:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetUShort(), PacketDataTypes.UShort);
                    break;
                    case 5:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetUInt(), PacketDataTypes.UInt);
                    break;
                    case 6:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetULong(), PacketDataTypes.ULong);
                    break;
                    case 7:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetULong(), PacketDataTypes.MessageType);
                    break;
                    case 8:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetULong(), PacketDataTypes.Version);
                    break;
                    case 9:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetULong(), PacketDataTypes.Chunk);
                    break;
                    case 10:
                    Data [ TemplatePosition ] = new SerializedValue(Parser.GetULong(), PacketDataTypes.String);
                    break;
                    default:
                    throw new Exception("Unknown DataType, CANT PARSE!");
                }
                TemplatePosition++;
            }
            if (!Parser.EndOfPacket)
                throw new Exception("Template Does not match template structure");
        }

    }
    public class PreDefinedSerializedPacketParser
    {
        byte [ ] Template;
        public PreDefinedSerializedPacketParser(byte [ ] Template, bool IsEmbededInPacket = false)
        {
            if (IsEmbededInPacket)
                this.Template = (new PacketParser(Template)).GetChunk();
            else
                this.Template = Template;
        }
        // DOES NOT CHECK PRE EXISTSING TEMPLATE
        public SerializedPacket Parse(byte [ ] Packet, bool HasEmbededTemplate = false)
        {
            byte [ ] PAC = null;
            if (HasEmbededTemplate)
            {
                int EOT = 0; // EndOfTemplate
                while (true)
                    if (Packet [ EOT ] == 0)
                        break;
                    else
                        EOT++;

                PAC = new byte [ Packet.Length - EOT ];
                Array.Copy(Packet, EOT, PAC, 0, PAC.Length);
            }
            else
                PAC = Packet;

            return new SerializedPacket(PAC, Template);
        }
    }

    public struct PacketBuilderBuiltPacket
    {
        public Byte [ ] Packet;
        public Byte [ ] Template;
    }
    public class SerializedPacketBuilder
    {
        private struct PacketChunk
        {
            public PacketDataTypes Type;
            public Byte [ ] RawObj;
        }
        private readonly List<PacketChunk> Packet;
        public SerializedPacketBuilder(Int32 Size = -1)
        {
            if (Size > 0)
                this.Packet = new List<PacketChunk>(Size);
            else
                this.Packet = new List<PacketChunk>();
        }

        public PacketBuilderBuiltPacket Build()
        {
            List<Byte> Packet = new List<Byte>();
            List<Byte> Template = new List<Byte>();

            lock (Packet)
                foreach (PacketChunk Chunk in this.Packet)
                {
                    Packet.AddRange(Chunk.RawObj);
                    Template.Add((Byte)Chunk.Type);
                }

            return new PacketBuilderBuiltPacket() { Packet = Packet.ToArray(), Template = Template.ToArray() };
        }
        public Byte [ ] BuildWithEmbededTemplate()
        {
            List<Byte> Packet = new List<Byte>();
            Int32 pos = 0;
            lock (Packet)
                foreach (PacketChunk Chunk in this.Packet)
                {
                    Packet.AddRange(Chunk.RawObj);
                    Packet.Insert(pos, (Byte)Chunk.Type);
                    pos++;
                }

            return Packet.ToArray();
        }
        public Byte [ ] BuildPacket()
        {
            List<Byte> Packet = new List<Byte>();
            lock (Packet)
                foreach (PacketChunk Chunk in this.Packet)
                    Packet.AddRange(Chunk.RawObj);
            return Packet.ToArray();
        }
        public Byte [ ] BuildTemplate()
        {
            List<Byte> Template = new List<Byte>();
            lock (this.Packet)
                foreach (PacketChunk Chunk in this.Packet)
                    Template.Add((Byte)Chunk.Type);
            return Template.ToArray();
        }

        public void Add(byte V) =>
            Packet.Add(new PacketChunk() { RawObj = new byte [ ] { V }, Type = PacketDataTypes.Byte });
        public void Add(short V) =>
            Packet.Add(new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.Short });
        public void Add(int V) =>
            Packet.Add(new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.Int });
        public void Add(long V) =>
            Packet.Add(new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.Long });
        public void Add(ushort V) =>
            Packet.Add(new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.UShort });
        public void Add(uint V) =>
            Packet.Add(new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.UInt });
        public void Add(ulong V) =>
            Packet.Add(new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.ULong });
        public void Add(MessageType V) =>
            Packet.Add(new PacketChunk() { RawObj = new byte [ ] { (byte)V }, Type = PacketDataTypes.MessageType });
        public void Add(Version V)
        {
            byte [ ] Data = new byte [ 8 ];
            BitConverter.GetBytes((ushort)V.Major).CopyTo(Data, 0);
            BitConverter.GetBytes((ushort)V.Minor).CopyTo(Data, 2);
            BitConverter.GetBytes((ushort)V.Build).CopyTo(Data, 4);
            BitConverter.GetBytes((ushort)V.Revision).CopyTo(Data, 6);
            Packet.Add(new PacketChunk() { RawObj = Data, Type = PacketDataTypes.Version });
        }
        public void Add(byte [ ] V)
        {
            byte [ ] Data = new byte [ 4 + V.Length ];
            BitConverter.GetBytes(V.Length).CopyTo(Data, 0);
            V.CopyTo(Data, 4);
            Packet.Add(new PacketChunk() { RawObj = Data, Type = PacketDataTypes.Chunk });
        }
        public void Add(string V, Encoding Coding = null)
        {
            if (Coding is null)
                Coding = Encoding.UTF8;
            Packet.Add(new PacketChunk() { RawObj = Coding.GetBytes(V), Type = PacketDataTypes.String });
        }

        public void Insert(int pos, byte V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = new byte [ ] { V }, Type = PacketDataTypes.Byte });
        public void Insert(int pos, short V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.Short });
        public void Insert(int pos, int V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.Int });
        public void Insert(int pos, long V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.Long });
        public void Insert(int pos, ushort V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.UShort });
        public void Insert(int pos, uint V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.UInt });
        public void Insert(int pos, ulong V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = BitConverter.GetBytes(V), Type = PacketDataTypes.ULong });
        public void Insert(int pos, MessageType V) =>
            Packet.Insert(pos, new PacketChunk() { RawObj = new byte [ ] { (byte)V }, Type = PacketDataTypes.MessageType });
        public void Insert(int pos, Version V)
        {
            byte [ ] Data = new byte [ 8 ];
            BitConverter.GetBytes((ushort)V.Major).CopyTo(Data, 0);
            BitConverter.GetBytes((ushort)V.Minor).CopyTo(Data, 2);
            BitConverter.GetBytes((ushort)V.Build).CopyTo(Data, 4);
            BitConverter.GetBytes((ushort)V.Revision).CopyTo(Data, 6);
            Packet.Insert(pos, new PacketChunk() { RawObj = Data, Type = PacketDataTypes.Version });
        }
        public void Insert(int pos, byte [ ] V)
        {
            byte [ ] Data = new byte [ 4 + V.Length ];
            BitConverter.GetBytes(V.Length).CopyTo(Data, 0);
            V.CopyTo(Data, 4);
            Packet.Insert(pos, new PacketChunk() { RawObj = Data, Type = PacketDataTypes.Chunk });
        }
        public void Insert(int pos, string V, Encoding Coding = null)
        {
            if (Coding is null)
                Coding = Encoding.UTF8;
            Packet.Insert(pos, new PacketChunk() { RawObj = Coding.GetBytes(V), Type = PacketDataTypes.String });
        }
    }
    public class SerializedTemplateBuilder
    {
        List<PacketDataTypes> Template = new List<PacketDataTypes>();
        public byte [ ] Build()
        {
            List<Byte> Template = new List<Byte>();
            lock (this.Template)
                foreach (PacketDataTypes T in this.Template)
                    Template.Add((Byte)T);
            return Template.ToArray();
        }

        public void AddByte() =>
            Template.Add(PacketDataTypes.Byte);
        public void AddShort() =>
            Template.Add(PacketDataTypes.Short);
        public void AddInt() =>
            Template.Add(PacketDataTypes.Int);
        public void AddLong() =>
            Template.Add(PacketDataTypes.Long);
        public void AddUShort() =>
            Template.Add(PacketDataTypes.UShort);
        public void AddUInt() =>
            Template.Add(PacketDataTypes.UInt);
        public void AddULong() =>
            Template.Add(PacketDataTypes.ULong);
        public void AddMessageType() =>
            Template.Add(PacketDataTypes.MessageType);
        public void AddVersion() =>
            Template.Add(PacketDataTypes.Version);
        public void AddChunk() =>
            Template.Add(PacketDataTypes.Chunk);
        public void AddByteArray() =>
            Template.Add(PacketDataTypes.Chunk);
        public void AddString() =>
            Template.Add(PacketDataTypes.String);

        public void InsertByte(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Byte);
        public void InsertShort(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Short);
        public void InsertInt(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Int);
        public void InsertLong(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Long);
        public void InsertUShort(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.UShort);
        public void InsertUInt(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.UInt);
        public void InsertULong(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.ULong);
        public void InsertMessageType(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.MessageType);
        public void InsertVersion(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Version);
        public void InsertChunk(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Chunk);
        public void InsertByteArray(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.Chunk);
        public void InsertString(int Pos) =>
            Template.Insert(Pos, PacketDataTypes.String);

    }


    public enum DynamicSerializedPacketConditionOperators
    {
        IS = 0,
        IS_NOT = 1,
        //IS_OR = 2,
        //IS_NOT_OR = 3,
        IS_AND = 4,
        IS_NOT_AND = 5,
    }
    public class DynamicSerializedPacketCondition
    {
        public DynamicSerializedPacketConditionOperators MainOperation;
        public int LeftADDR;
        public bool LeftADDR_Rel;
        public byte [ ] Right;

        public DynamicSerializedPacketCondition SubOperation;
        public TemplateChunk [ ] InsertTemplate;
        public TemplateChunk [ ] ElseTemplate;
    }
    public struct TemplateChunk
    {
        public DynamicSerializedPacketCondition Condition;
        public PacketDataTypes ChunkType;
    }
    public class DynamicSerializedPacketParser
    {
        int Position = 0;
        List<TemplateChunk> Chunks = new List<TemplateChunk>();
        public DynamicSerializedPacketParser(byte [ ] Template)
        {
            int Position = 0;

            void CompileSubOperation(ref DynamicSerializedPacketCondition Parent)
            {
                Parent.SubOperation = new DynamicSerializedPacketCondition();
                Parent.SubOperation.MainOperation = (DynamicSerializedPacketConditionOperators)Template [ Position ];
                Position++;
                Parent.SubOperation.Right = new byte [ BitConverter.ToInt32(Template, Position) ];
                Position += 4;
                Parent.SubOperation.LeftADDR = BitConverter.ToInt32(Template, Position);
                Position += 4;
                Parent.SubOperation.LeftADDR_Rel = Template [ Position ] == 0 ? false : true;
                Position += 1;
                Array.Copy(Template, Position, Parent.SubOperation.Right, 0, Parent.SubOperation.Right.Length);
                Position += Parent.SubOperation.Right.Length;
                if (Parent.SubOperation.MainOperation == DynamicSerializedPacketConditionOperators.IS_AND ||
                    Parent.SubOperation.MainOperation == DynamicSerializedPacketConditionOperators.IS_NOT_AND)
                    CompileSubOperation(ref Parent.SubOperation);

            }
            TemplateChunk CompileChunk()
            {
                TemplateChunk Chunk = new TemplateChunk();
                if (Template [ Position ] == 255)// PacketDataTypes.NONE == PatDataTypes.255_DefineCondition
                {
                    Position++;
                    Chunk.Condition = new DynamicSerializedPacketCondition();
                    Chunk.Condition.MainOperation = (DynamicSerializedPacketConditionOperators)Template [ Position ];
                    Position++;
                    Chunk.Condition.Right = new byte [ BitConverter.ToInt32(Template, Position) ];
                    Position += 4;
                    Chunk.Condition.LeftADDR = BitConverter.ToInt32(Template, Position);
                    Position += 4;
                    Chunk.Condition.LeftADDR_Rel = Template [ Position ] == 0 ? false : true;
                    Position += 1;
                    Array.Copy(Template, Position, Chunk.Condition.Right, 0, Chunk.Condition.Right.Length);
                    Position += Chunk.Condition.Right.Length;
                    if (Chunk.Condition.MainOperation == DynamicSerializedPacketConditionOperators.IS_AND ||
                        Chunk.Condition.MainOperation == DynamicSerializedPacketConditionOperators.IS_NOT_AND)
                        CompileSubOperation(ref Chunk.Condition.SubOperation);

                    Chunk.Condition.InsertTemplate = new TemplateChunk [ BitConverter.ToInt32(Template, Position) ];
                    Position += 4;
                    for (int x = 0; x < Chunk.Condition.InsertTemplate.Length; x++)
                        Chunk.Condition.InsertTemplate [ x ] = CompileChunk();

                    Chunk.Condition.ElseTemplate = new TemplateChunk [ BitConverter.ToInt32(Template, Position) ];
                    Position += 4;
                    for (int x = 0; x < Chunk.Condition.ElseTemplate.Length; x++)
                        Chunk.Condition.ElseTemplate [ x ] = CompileChunk();
                }
                else
                    Chunk.ChunkType = (PacketDataTypes)Template [ Position ];
                return Chunk;
            }
            while (true)
            {
                Chunks.Add(CompileChunk());
                if (Position == Template.Length)
                    break;
            }
        }
        public SerializedPacket Parse(byte [ ] Packet)
        {
            Stack<TemplateChunk> Trace = new Stack<TemplateChunk>();
            int Pos = 0;
            List<SerializedValue> Data = new List<SerializedValue>();
            List<byte [ ]> DataBlocks = new List<byte [ ]>();
            DynamicSerializedPacketDataParser Parser = new DynamicSerializedPacketDataParser(Packet);
            TemplateChunk [ ] ResolveCondition(DynamicSerializedPacketCondition Condition)
            {
                byte [ ] Data1;
                if (Condition.LeftADDR_Rel)
                    Data1 = DataBlocks [ Data.Count - Condition.LeftADDR ];
                else
                    Data1 = DataBlocks [ Condition.LeftADDR ];

                switch (Condition.MainOperation)
                {
                    case DynamicSerializedPacketConditionOperators.IS:
                    {
                        if (AreSame(Data1, Condition.Right))
                            return Condition.InsertTemplate;
                        else
                            return Condition.ElseTemplate;
                    }

                    case DynamicSerializedPacketConditionOperators.IS_NOT:
                    {
                        if (AreSame(Data1, Condition.Right))
                            return Condition.ElseTemplate;
                        else
                            return Condition.InsertTemplate;
                    }

                    case DynamicSerializedPacketConditionOperators.IS_AND:
                    {
                        if (AreSame(Data1, Condition.Right))
                            return ResolveCondition(Condition.SubOperation);
                        else
                            return Condition.ElseTemplate;

                    }

                    case DynamicSerializedPacketConditionOperators.IS_NOT_AND:
                    {
                        if (AreSame(Data1, Condition.Right))
                            return Condition.ElseTemplate;
                        else
                            return ResolveCondition(Condition.SubOperation);
                    }

                    default:
                    return null;
                }
            }

            while (Packet.Length > Parser.Position && Pos < Chunks.Count)
            {
                if (Trace.Count == 0)
                {
                    Trace.Push(this.Chunks [ Pos ]);
                    Pos++;
                }
                TemplateChunk WorkingChunk = Trace.Pop();
                if (WorkingChunk.Condition != null)
                {
                    TemplateChunk [ ] Resolve = ResolveCondition(WorkingChunk.Condition);
                    if (Resolve != null)
                        for (int x = Resolve.Length - 1; x > -1; x--)
                            Trace.Push(Resolve [ x ]);
                }
                else
                    switch ((byte)WorkingChunk.ChunkType)
                    {
                        case 0:
                        Data.Add(new SerializedValue(Parser.GetByte(), PacketDataTypes.Byte));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 1:
                        Data.Add(new SerializedValue(Parser.GetShort(), PacketDataTypes.Short));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 2:
                        Data.Add(new SerializedValue(Parser.GetInt(), PacketDataTypes.Int));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 3:
                        Data.Add(new SerializedValue(Parser.GetLong(), PacketDataTypes.Long));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 4:
                        Data.Add(new SerializedValue(Parser.GetUShort(), PacketDataTypes.UShort));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 5:
                        Data.Add(new SerializedValue(Parser.GetUInt(), PacketDataTypes.UInt));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 6:
                        Data.Add(new SerializedValue(Parser.GetULong(), PacketDataTypes.ULong));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 7:
                        Data.Add(new SerializedValue(Parser.GetULong(), PacketDataTypes.MessageType));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 8:
                        Data.Add(new SerializedValue(Parser.GetULong(), PacketDataTypes.Version));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 9:
                        Data.Add(new SerializedValue(Parser.GetULong(), PacketDataTypes.Chunk));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 10:
                        Data.Add(new SerializedValue(Parser.GetULong(), PacketDataTypes.String));
                        DataBlocks.Add(Parser.LastGetPacketData);
                        break;
                        case 254:
                        break;
                        case 255:
                        throw new Exception("Compiler Failed to Compile the template! 'this is done at this.ctor(...)'");
                        default:
                        throw new Exception("Unknown DataType, CANT PARSE!");
                    }
            }
            return new SerializedPacket(Data.ToArray());
        }
        private static bool AreSame(byte [ ] Data1, byte [ ] Data2)
        {
            if (Data1.Length != Data2.Length)
                return false;
            for (int x = 0; x < 0; x++)
            {
                if (Data1 [ x ] != Data2 [ x ])
                    return false;
            }
            return true;
        }
    }
    public class DynamicSerializedPacketBuilder
    {
        List<TemplateChunk> Chunks = new List<TemplateChunk>();
        public byte [ ] Build() =>
            Build(Chunks.ToArray());

        public void AddFullChunk(TemplateChunk Chunk) =>
            Chunks.Add(Chunk);
        public void InsertFullChunk(int pos, TemplateChunk Chunk) =>
            Chunks.Insert(pos, Chunk);

        public void AddRangeFullChunk(TemplateChunk [ ] Chunk) =>
            Chunks.AddRange(Chunk);
        public void InsertRangeFullChunk(int pos, TemplateChunk [ ] Chunk) =>
            Chunks.InsertRange(pos, Chunk);

        public static byte [ ] Build(TemplateChunk [ ] Template)
        {
            List<byte> Output = new List<byte>();

            void BuildSubOperation(DynamicSerializedPacketCondition Parent)
            {
                Output.Add((byte)Parent.SubOperation.MainOperation);
                Output.AddRange(BitConverter.GetBytes((int)Parent.SubOperation.Right.Length));
                Output.AddRange(BitConverter.GetBytes((int)Parent.SubOperation.LeftADDR));
                Output.Add((byte)(Parent.SubOperation.LeftADDR_Rel ? 1 : 0));
                Output.AddRange(Parent.SubOperation.Right);
                if (Parent.SubOperation.MainOperation == DynamicSerializedPacketConditionOperators.IS_AND ||
                    Parent.SubOperation.MainOperation == DynamicSerializedPacketConditionOperators.IS_NOT_AND)
                    BuildSubOperation(Parent.SubOperation.SubOperation);
            }
            void BuildChunk(TemplateChunk Chunk)
            {
                if (Chunk.ChunkType == PacketDataTypes.NONE || Chunk.Condition != null)
                {
                    Output.Add(255);
                    Output.Add((byte)Chunk.Condition.MainOperation);
                    Output.AddRange(BitConverter.GetBytes((int)Chunk.Condition.Right.Length));
                    Output.AddRange(BitConverter.GetBytes((int)Chunk.Condition.LeftADDR));
                    Output.Add((byte)(Chunk.Condition.LeftADDR_Rel ? 1 : 0));
                    Output.AddRange(Chunk.Condition.Right);
                    if (Chunk.Condition.MainOperation == DynamicSerializedPacketConditionOperators.IS_AND ||
                        Chunk.Condition.MainOperation == DynamicSerializedPacketConditionOperators.IS_NOT_AND)
                        BuildSubOperation(Chunk.Condition);
                    Output.AddRange(BitConverter.GetBytes((int)Chunk.Condition.InsertTemplate.Length));
                    foreach (TemplateChunk SubChunk in Chunk.Condition.InsertTemplate)
                        BuildChunk(SubChunk);
                    Output.AddRange(BitConverter.GetBytes((int)Chunk.Condition.ElseTemplate.Length));
                    foreach (TemplateChunk SubChunk in Chunk.Condition.ElseTemplate)
                        BuildChunk(SubChunk);
                }
                else
                    Output.Add((byte)Chunk.ChunkType);

            }
            foreach (TemplateChunk Chunk in Template)
                BuildChunk(Chunk);

            return Output.ToArray();
        }
    }
    public class DynamicSerializedPacketConditionBuilder
    {
        private TemplateChunk Chunk = new TemplateChunk() { Condition = new DynamicSerializedPacketCondition(), ChunkType = PacketDataTypes.NONE };

        public TemplateChunk Build()
        {
            if (this.Chunk.Condition.LeftADDR < 0)
                throw new MissingFieldException("LeftAddr is not set");
            if ((this.Chunk.Condition.MainOperation == DynamicSerializedPacketConditionOperators.IS_AND ||
               this.Chunk.Condition.MainOperation == DynamicSerializedPacketConditionOperators.IS_NOT_AND) &&
               this.Chunk.Condition.SubOperation == null)
                throw new MissingFieldException("Operation Contains an AND setting, but SubOperation Is Not Set");
            if (this.Chunk.Condition.Right == null)
                this.Chunk.Condition.Right = new byte [ ] { };
            return this.Chunk;
        }
        
        public void SetLeftAddr(int LeftAddr)
        {
            if (LeftAddr < 0)
                throw ArgumentOutOfRangeException();
            this.Chunk.Condition.LeftADDR = LeftAddr;
        }
        public void SetRightValue(byte [ ] RAW)
        {
            Chunk.Condition.Right = RAW;
        }
        public void SetRightValue(short number)
        {
            Chunk.Condition.Right = BitConverter.GetBytes(number);
        }
        public void SetRightValue(ushort number)
        {
            Chunk.Condition.Right = BitConverter.GetBytes(number);
        }
        public void SetRightValue(int number)
        {
            Chunk.Condition.Right = BitConverter.GetBytes(number);
        }
        public void SetRightValue(uint number)
        {
            Chunk.Condition.Right = BitConverter.GetBytes(number);
        }
        public void SetRightValue(long number)
        {
            Chunk.Condition.Right = BitConverter.GetBytes(number);
        }
        public void SetRightValue(ulong number)
        {
            Chunk.Condition.Right = BitConverter.GetBytes(number);
        }

        public void SetRightValue(string text, Encoding Coding = null)
        {
            if (Coding == null)
                Coding = Encoding.UTF8;
            Chunk.Condition.Right = Coding.GetBytes(text);
        }
        public void SetRightValue(MessageType type)
        {
            Chunk.Condition.Right = new byte [ ] { (byte)type };
        }
        public void SetRightValue(Version V)
        {
            Chunk.Condition.Right = new byte [ 8 ];
            BitConverter.GetBytes((ushort)V.Major).CopyTo(Chunk.Condition.Right, 0);
            BitConverter.GetBytes((ushort)V.Minor).CopyTo(Chunk.Condition.Right, 0);
            BitConverter.GetBytes((ushort)V.Build).CopyTo(Chunk.Condition.Right, 0);
            BitConverter.GetBytes((ushort)V.Revision).CopyTo(Chunk.Condition.Right, 0);
        }

        public void SetToEqwalsCondition()
        {
            Chunk.Condition.MainOperation = DynamicSerializedPacketConditionOperators.IS;
        }
        public void SetToNotEqwalsCondition()
        {
            Chunk.Condition.MainOperation = DynamicSerializedPacketConditionOperators.IS_NOT;
        }
        public void SetToEqwalsAndCondition()
        {
            Chunk.Condition.MainOperation = DynamicSerializedPacketConditionOperators.IS_AND;
        }
        public void SetToNotEqwalsAndCondition()
        {
            Chunk.Condition.MainOperation = DynamicSerializedPacketConditionOperators.IS_NOT_AND;
        }

        public void SetSubOperation(DynamicSerializedPacketCondition Condition)
        {
            Chunk.Condition.SubOperation = Condition;
        }
        public void SetSubOperation(DynamicSerializedPacketConditionBuilder Condition)
        {
            Chunk.Condition.SubOperation = Condition.Build().Condition;
        }
    }
}
