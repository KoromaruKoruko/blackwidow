using System;
using System.Collections.Generic;

namespace BDLib.Internal.DataStream
{
    public static class DataStream
    {
        private static List<DataStreamInfo> Streams;
        private static Random IDGEN = new Random();


        public static UInt32 RegisterStream()
        {
            UInt32 uint_ID = GenID();
            Streams.Add(new DataStreamInfo()
            {
                ID = uint_ID,
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
                    SearchID = uint_ID;
                    if (Streams.FindIndex(new Predicate<DataStreamInfo>(FindByID)) > 0) break;
                }
                return uint_ID;
        }

        public static void UnRegisterStream(UInt32 ID)
        {
            SearchID = ID;
            int x = Streams.FindIndex(new Predicate<DataStreamInfo>(FindByID));
            Streams.RemoveAt(x);
        }
        
        public static bool Exists(UInt32 ID)
        {
            SearchID = ID;
            return (Streams.FindIndex(new Predicate<DataStreamInfo>(FindByID)) > 0) ? true : false;
        }


        public static byte ReadFrom(UInt32 StreamID)
        {
            SearchID = StreamID;
            return Streams.Find(new Predicate<DataStreamInfo>(FindByID)).Data.Dequeue();
        }
        public static void WriteTo(UInt32 StreamID,byte Data)
        {
            SearchID = StreamID;
            Streams.Find(new Predicate<DataStreamInfo>(FindByID)).Data.Enqueue(Data);
        }
        public static byte[] ReadFrom(UInt32 StreamID,int Amount)
        {
            SearchID = StreamID;
            byte[] output = new byte[Amount];
            for(int x = 0; x < Amount; x++)
                output[x] = Streams.Find(new Predicate<DataStreamInfo>(FindByID)).Data.Dequeue();
            return output;
        }
        public static void WriteTo(UInt32 StreamID, byte[] Data)
        {
            SearchID = StreamID;
            for(int x = 0; x < Data.Length; x++)
                Streams.Find(new Predicate<DataStreamInfo>(FindByID)).Data.Enqueue(Data[x]);
        }



        private static UInt32 SearchID;
        private static bool FindByID(DataStreamInfo STM)
        {
            return (STM.ID == SearchID) ? true : false;
        }
    }

    internal struct DataStreamInfo
    {
        public UInt32 ID;
        internal Queue<byte> Data;
        public int Available { get { return Data.Count; } }
    }
}
