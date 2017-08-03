using System;
using System.IO;
using System.Collections.Generic;

namespace BDLib.Internal.DataStream
{
    public static class DataStream
    {
        private static List<UInt32> IDs;
        private static List<DataStreamInfo> Streams;
        private static Random IDGEN = new Random();


        public static UInt32 RegisterStream()
        {
            byte[] ID = new byte[4];
            IDGEN.NextBytes(ID);
            UInt32 uint_ID = BitConverter.ToUInt32(ID, 0);
            IDs.Add(uint_ID);
            Streams.Add(new DataStreamInfo()
            {
                Data = new Queue<byte>(),
            });
            return uint_ID;
        }
        public static void UnRegisterStream(UInt32 ID)
        {
            SearchID = ID;
            int x = IDs.FindIndex(new Predicate<uint>(FindByID));
            IDs.RemoveAt(x);
            Streams.RemoveAt(x);
        }
        
        public static bool Exists(UInt32 ID)
        {
            SearchID = ID;
            return (IDs.FindIndex(new Predicate<UInt32>(FindByID)) > 0) ? true : false;
        }


        public static byte ReadFrom(UInt32 StreamID)
        {
            SearchID = StreamID;
            return Streams[IDs.FindIndex(new Predicate<uint>(FindByID))].Data.Dequeue();
        }
        public static void WriteTo(UInt32 StreamID,byte Data)
        {
            SearchID = StreamID;
            Streams[IDs.FindIndex(new Predicate<uint>(FindByID))].Data.Enqueue(Data);
        }
        public static byte[] ReadFrom(UInt32 StreamID,int Amount)
        {
            SearchID = StreamID;
            byte[] output = new byte[Amount];
            for(int x = 0; x < Amount; x++)
                output[x] = Streams[IDs.FindIndex(new Predicate<uint>(FindByID))].Data.Dequeue();
            return output;
        }
        public static void WriteTo(UInt32 StreamID, byte[] Data)
        {
            SearchID = StreamID;
            for(int x = 0; x < Data.Length; x++)
                Streams[IDs.FindIndex(new Predicate<uint>(FindByID))].Data.Enqueue(Data[x]);
        }



        private static UInt32 SearchID;
        private static bool FindByID(UInt32 ID)
        {
            return (ID == SearchID) ? true : false;
        }
    }

    internal struct DataStreamInfo
    {
        internal Queue<byte> Data;
        public int Available { get { return Data.Count; } }
    }
}
