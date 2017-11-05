using System;
using System.Collections.Generic;

namespace BDLib.Internal.DataStream
{
    public static class TwoWayDataStream
    {
        private static Dictionary<UInt32, TwoWayStream> Streams = new Dictionary<uint, TwoWayStream>();
        private static Random IDGEN = new Random();

        /// <summary>
        /// Creates a TwoWayStream for internal cons
        /// </summary>
        /// <returns>the TwoWayStream Info</returns>
        public static TwoWayStreamInfo RegisterStream()
        {
            if (!BDLibInfo.Info.Moduls.Contains("Internal/DataStream/TwoWayDataStream.cs"))
                BDLibInfo.Info.Moduls.Add("Internal/DataStream/TwoWayDataStream.cs");

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
            UInt32 uint_ID = 0;
            while (true)
            {
                byte[] ID = new byte[4];
                IDGEN.NextBytes(ID);
                uint_ID = BitConverter.ToUInt32(ID, 0);
                try
                {
                    var x = Streams[uint_ID].PT1;
                    break;
                }
                catch
                {

                }

            }
            return uint_ID;
        }
        /// <summary>
        /// removes the TwoWayStream
        /// </summary>
        /// <param name="ID">the StreamID</param>
        public static void UnRegisterStream(UInt32 ID)
        {
            Streams.Remove(ID);
        }

        /// <summary>
        /// look to see if the Stream is still there
        /// </summary>
        /// <param name="ID">StreamID</param>
        /// <returns>if it Exists</returns>
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

        /// <summary>
        /// returns 1byte of data from the stream 
        /// </summary>
        /// <param name="StreamID">StreamID</param>
        /// <param name="PointID">PT1/PT2</param>
        /// <returns>the data</returns>
        public static byte ReadFrom(UInt32 StreamID, UInt32 PointID)
        {
            if (PointID == Streams[StreamID].PT1)
            {
                return Streams[StreamID].STMToPT1.Data.Dequeue();
            }
            else if (PointID == Streams[StreamID].PT2)
            {
                return Streams[StreamID].STMToPT2.Data.Dequeue();
            }
            else return 0;
        }
        /// <summary>
        /// writes to the stream
        /// </summary>
        /// <param name="StreamID">Stream ID</param>
        /// <param name="PointID">PT1/PT2</param>
        /// <param name="Data">the data to write</param>
        public static void WriteTo(UInt32 StreamID, UInt32 PointID,  byte Data)
        {
            if (PointID == Streams[StreamID].PT1)
            {
                Streams[StreamID].STMToPT2.Data.Enqueue(Data);
            }
            else if (PointID == Streams[StreamID].PT2)
            {
                Streams[StreamID].STMToPT1.Data.Enqueue(Data);
            }
        }

        /// <summary>
        /// Reads x amount from stream if x goes over it just returns the data read
        /// </summary>
        /// <param name="StreamID">StreamID</param>
        /// <param name="PointID">PT1/PT2</param>
        /// <param name="Amount">the amount to read</param>
        /// <returns>the data</returns>
        public static byte[] ReadFrom(UInt32 StreamID, UInt32 PointID, int Amount)
        {
            if (PointID == Streams[StreamID].PT1)
            {
                byte[] buffer = new byte[(Streams[StreamID].STMToPT1.Available >= Amount) ? Amount : Streams[StreamID].STMToPT1.Available];
                for (int x = 0; x < Amount; x++)
                    buffer[x] = Streams[StreamID].STMToPT1.Data.Dequeue();
                return buffer;
            }
            else if (PointID == Streams[StreamID].PT2)
            {
                byte[] buffer = new byte[(Streams[StreamID].STMToPT1.Available >= Amount) ? Amount : Streams[StreamID].STMToPT1.Available];
                for (int x = 0; x < Amount; x++)
                    buffer[x] = Streams[StreamID].STMToPT2.Data.Dequeue();
                return buffer;
            }
            else return null;
        }
        /// <summary>
        /// writes data to the stream
        /// </summary>
        /// <param name="StreamID">Stream ID</param>
        /// <param name="PointID">PT1/PT2</param>
        /// <param name="Data">Data to Write</param>
        public static void WriteTo(UInt32 StreamID, UInt32 PointID, byte[] Data)
        {
            if (PointID == Streams[StreamID].PT1)
            {
                for(int y = 0; y < Data.Length; y++)
                    Streams[StreamID].STMToPT2.Data.Enqueue(Data[y]);
            }
            else if (PointID == Streams[StreamID].PT2)
            {
                for (int y = 0; y < Data.Length; y++)
                    Streams[StreamID].STMToPT1.Data.Enqueue(Data[y]);
            }
        }
    }

    internal struct TwoWayStream
    {
        public UInt32 PT1, PT2;
        public DataStreamInfo STMToPT1, STMToPT2;
    }
    public struct TwoWayStreamInfo
    {
        public UInt32 StreamID;
        public UInt32 Point1ID, Point2ID;
    }
}
