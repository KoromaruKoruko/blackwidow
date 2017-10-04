using System;
using System.Collections.Generic;
using BDLib.BDLibInfo;

namespace BDLib.Internal.Chat
{

    //TODO Make sure this works

    public static class InternalAlerts
    {
        private static Dictionary<string,EventHandler<InternalAlertEventArgs>> Events = new Dictionary<string,EventHandler<InternalAlertEventArgs>>();
        private static Random IDGEN = new Random();

        public static void Register(Action<object, InternalAlertEventArgs> AletFunction, string Name)
        {
            if (!Info.Moduls.Contains("Internal/Chat/InternalAlerts.cs"))
                Info.Moduls.Add("Internal/Chat/InternalAlerts.cs");
            Events.Add(Name,new EventHandler<InternalAlertEventArgs>(AletFunction));
        }
        public static void UnRegister(string User)
        {
            Events.Remove(User);
        }
        
        public static bool Exists(string name)
        {
            return Events.ContainsKey(name);
        }
        
        public static void Alert(string User, InternalAlertEventArgs AlertInfo, string YouUsername)
        {
            Events[User].Invoke(YouUsername, AlertInfo);
        }
    }
    public struct InternalAlertEventArgs
    {
        string AlertString;
        object[] Data;
    }
}
