using System;
using System.Collections.Generic;
using BDLib.BDLibInfo;

namespace BDLib.Internal.DataStream
{
    public static class DataStream
    {
        private static Dictionary<UInt32,DataStreamInfo> Streams;
        private static Random IDGEN = new Random();


        public static UInt32 RegisterStream()
        {
            if (!Info.Moduls.Contains("Internal/DataStream/DataStream.cs"))
                Info.Moduls.Add("Internal/DataStream/DataStream.cs");

            UInt32 uint_ID = GenID();
            Streams.Add(uint_ID, new DataStreamInfo()
            {
                Data = new Queue<byte>(),
            });
            return uint_ID;
        }
        private static UInt32 GenID()
        {
            UInt32 uint_ID = 0;
            while (true)
            {
                byte[] ID = new byte[4];
                IDGEN.NextBytes(ID);
                uint_ID = BitConverter.ToUInt32(ID, 0);
                try
                {
                    var x = Streams[uint_ID];
                    break;
                }
                catch
                {

                }
            }
            return uint_ID;
        }
        public static void UnRegisterStream(UInt32 ID)
        {
            Streams.Remove(ID);
        }

        public static bool Exists(UInt32 ID)
        {
            try
            {
                var x = Streams[ID];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte ReadFrom(UInt32 StreamID)
        {
            return Streams[StreamID].Data.Dequeue();
        }
        public static void WriteTo(UInt32 StreamID,byte Data)
        {
            Streams[StreamID].Data.Enqueue(Data);
        }
        public static byte[] ReadFrom(UInt32 StreamID,int Amount)
        {
            byte[] output = new byte[Amount];
            for(int x = 0; x < Amount; x++)
                output[x] = Streams[StreamID].Data.Dequeue();
            return output;
        }
        public static void WriteTo(UInt32 StreamID, byte[] Data)
        {
            for(int x = 0; x < Data.Length; x++)
                Streams[StreamID].Data.Enqueue(Data[x]);
        }
    }

    internal struct DataStreamInfo
    {
        internal Queue<byte> Data;
        public int Available { get { return Data.Count; } }
    }
}
