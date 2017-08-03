using System;
using System.Collections.Generic;

namespace BDLib.Internal.Chat
{
    public static class InternalAlerts
    {
        private static List<EventHandler<InternalAlertEventArgs>> Events = new List<EventHandler<InternalAlertEventArgs>>();
        private static List<UInt32> IDs;
        private static List<string> Names;
        private static Random IDGEN = new Random();
            

        public static UInt32 Register(Action<object, InternalAlertEventArgs> AletFunction, string Name)
        {
            Events.Add(new EventHandler<InternalAlertEventArgs>(AletFunction));

            byte[] ID = new byte[4];
            IDGEN.NextBytes(ID);
            UInt32 uint_ID = BitConverter.ToUInt32(ID,0);
            IDs.Add(uint_ID);
            Names.Add(Name);
            return uint_ID;
        }
        public static void UnRegister(UInt32 ID)
        {
            SearchID = ID;
            Events.RemoveAt((Int32.Parse(IDs.FindIndex(new Predicate<uint>(FindByID)).ToString())));
        }


        private static UInt32 SearchID;
        private static string SearchName;
        private static bool FindByID(UInt32 ID)
        {
            return (ID == SearchID) ? true : false;
        }
        private static bool FindByName(string name)
        {
            return (name == SearchName) ? true : false;
        }


        private static bool Exists(UInt32 ID)
        {
            SearchID = ID;
            return (IDs.FindIndex(new Predicate<UInt32>(FindByID)) > 0) ? true : false;
        }
        public static bool Exists(string name)
        {
            SearchName = name;
            return (Names.FindIndex(new Predicate<string>(FindByName)) > 0) ? true : false;
        }


        public static UInt32 GetUserID(string name)
        {
            SearchName = name;
            return IDs[Names.FindIndex(new Predicate<string>(FindByName))];
        }


        public static void Alert(UInt32 ID, InternalAlertEventArgs AlertInfo, UInt32 YOUR_ID)
        {
            SearchID = ID;
            Events[(Int32.Parse(IDs.FindIndex(new Predicate<uint>(FindByID)).ToString()))].Invoke(YOUR_ID, AlertInfo);
        }
    }
    public struct InternalAlertEventArgs
    {
        string AlertString;
        object[] Data;
    }
}
