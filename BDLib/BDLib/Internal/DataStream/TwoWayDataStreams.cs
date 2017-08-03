using System;
using System.Collections.Generic;

namespace BDLib.Internal.DataStream
{
    public static class TwoWayDataStream
    {
        private static List<TwoWayStream> Streams;
        private static Random IDGEN = new Random();


        public static TwoWayStreamInfo RegisterStream()
        {
            UInt32 StreamID = GenID();

            TwoWayStreamInfo Info = new TwoWayStreamInfo()
            {
                StreamID = GenID(),
                Point1ID = GenID(),
                Point2ID = GenID(),
            };

            return Info;
        }

        private static uint GenID()
        {
            byte[] ID = new byte[4];
            IDGEN.NextBytes(ID);
            UInt32 uint_ID = BitConverter.ToUInt32(ID, 0);

            //TODO :: make sure id doesnt Exist (only have to do StreamID not PointID)
            return uint_ID;
        }

        public static void UnRegisterStream(UInt32 ID)
        {
            SearchID = ID;
            int x = Streams.FindIndex(new Predicate<TwoWayStream>(FindByID));
            Streams.RemoveAt(x);
        }

        public static bool Exists(UInt32 ID)
        {
            SearchID = ID;
            return (Streams.FindIndex(new Predicate<TwoWayStream>(FindByID)) > 0) ? true : false;
        }


        public static byte ReadFrom(UInt32 StreamID, UInt32 PointID)
        {
            SearchID = StreamID;
            int Index = Streams.FindIndex(new Predicate<TwoWayStream>(FindByID));
            if (PointID == Streams[Index].PT1)
            {
                return Streams[Index].STMToPT1.Data.Dequeue();
            }
            else if (PointID == Streams[Index].PT2)
            {
                return Streams[Index].STMToPT2.Data.Dequeue();
            }
            else return 0;
        }
        public static void WriteTo(UInt32 StreamID, UInt32 PointID,  byte Data)
        {
            SearchID = StreamID;
            int Index = Streams.FindIndex(new Predicate<TwoWayStream>(FindByID));
            if (PointID == Streams[Index].PT1)
            {
                Streams[Index].STMToPT2.Data.Enqueue(Data);
            }
            else if (PointID == Streams[Index].PT2)
            {
                Streams[Index].STMToPT1.Data.Enqueue(Data);
            }
        }

        public static byte[] ReadFrom(UInt32 StreamID, UInt32 PointID, int Amount)
        {
            SearchID = StreamID;
            int Index = Streams.FindIndex(new Predicate<TwoWayStream>(FindByID));
            if (PointID == Streams[Index].PT1)
            {
                byte[] buffer = new byte[(Streams[Index].STMToPT1.Available >= Amount) ? Amount : Streams[Index].STMToPT1.Available];
                for (int x = 0; x < Amount; x++)
                    buffer[x] = Streams[Index].STMToPT1.Data.Dequeue();
                return buffer;
            }
            else if (PointID == Streams[Index].PT2)
            {
                byte[] buffer = new byte[(Streams[Index].STMToPT1.Available >= Amount) ? Amount : Streams[Index].STMToPT1.Available];
                for (int x = 0; x < Amount; x++)
                    buffer[x] = Streams[Index].STMToPT2.Data.Dequeue();
                return buffer;
            }
            else return null;
        }
        public static void WriteTo(UInt32 StreamID, UInt32 PointID, byte[] Data)
        {
            SearchID = StreamID;
            int Index = Streams.FindIndex(new Predicate<TwoWayStream>(FindByID));
            if (PointID == Streams[Index].PT1)
            {
                for(int y = 0; y < Data.Length; y++)
                    Streams[Index].STMToPT2.Data.Enqueue(Data[y]);
            }
            else if (PointID == Streams[Index].PT2)
            {
                for (int y = 0; y < Data.Length; y++)
                    Streams[Index].STMToPT1.Data.Enqueue(Data[y]);
            }
        }



        private static UInt32 SearchID;
        private static bool FindByID(TwoWayStream ID)
        {
            return (ID.ID == SearchID) ? true : false;
        }
    }

    internal struct TwoWayStream
    {
        public UInt32 ID;
        public UInt32 PT1, PT2;
        public DataStreamInfo STMToPT1, STMToPT2;
    }

    public struct TwoWayStreamInfo
    {
        public UInt32 StreamID;
        public UInt32 Point1ID, Point2ID;
    }
}
