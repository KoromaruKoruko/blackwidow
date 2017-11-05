using System;
using System.Collections.Generic;
using BDLib.BDLibInfo;

namespace BDLib.Internal.Chat
{
    public static class InternalAlerts
    {
        private static Dictionary<string,EventHandler<InternalAlertEventArgs>> Events = new Dictionary<string,EventHandler<InternalAlertEventArgs>>();
        private static Random IDGEN = new Random();

        /// <summary>
        /// creates an alert
        /// </summary>
        /// <param name="AletFunction">alert function</param>
        /// <param name="Name">alert name</param>
        public static void Register(Action<object, InternalAlertEventArgs> AletFunction, string Name)
        {
            if (!Info.Moduls.Contains("Internal/Chat/InternalAlerts.cs"))
                Info.Moduls.Add("Internal/Chat/InternalAlerts.cs");
            Events.Add(Name,new EventHandler<InternalAlertEventArgs>(AletFunction));
        }

        /// <summary>
        /// removes an alert
        /// </summary>
        /// <param name="Name">alert name</param>
        public static void UnRegister(string Name)
        {
            Events.Remove(Name);
        }
        
        /// <summary>
        /// checks if an alert exists
        /// </summary>
        /// <param name="name">Alert name</param>
        /// <returns>if exists</returns>
        public static bool Exists(string name)
        {
            return Events.ContainsKey(name);
        }
        
        /// <summary>
        /// triggers an alert
        /// </summary>
        /// <param name="Name">Alert Name</param>
        /// <param name="AlertInfo">Info about the alert</param>
        /// <param name="Sender">the Alert Trigger-er</param>
        public static void Alert(string Name, InternalAlertEventArgs AlertInfo, string Sender)
        {
            Events[Name].Invoke(Sender, AlertInfo);
        }
    }
    public struct InternalAlertEventArgs
    {
        string AlertString;
        object[] Data;
    }
}
