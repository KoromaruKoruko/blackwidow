using System;
using System.Collections.Generic;
using BDLib.BDLibInfo;

namespace BDLib.Internal.Chat
{
    public static class InternalChat
    {
        private static List<EventHandler<InternalChatEventArgs>> Events = new List<EventHandler<InternalChatEventArgs>>();

        public static void Join(Action<object,InternalChatEventArgs> ReturnFunction)
        {
            if (!Info.Moduls.Contains("Internal/Chat/InternalChat.cs"))
                Info.Moduls.Add("Internal/Chat/InternalChat.cs");

            Events.Add(new EventHandler<InternalChatEventArgs>(ReturnFunction));
        }
        public static void Part(Action<object,InternalChatEventArgs> ReturnFunction)
        {
            EventHandler<InternalChatEventArgs> TMP = new EventHandler<InternalChatEventArgs>(ReturnFunction);

            if (Events.Contains(TMP))
                Events.Remove(new EventHandler<InternalChatEventArgs>(ReturnFunction));
        }

        private static int Send(string message, string sender)
        {
            InternalChatEventArgs e = new InternalChatEventArgs()
            {
                Message = message,
                Sender = sender,
            };

            foreach (EventHandler<InternalChatEventArgs> E in Events)
                E.Invoke(null, e);

            return Events.Count;
        }
    }
    public struct InternalChatEventArgs
    {
        public string Message;
        public string Sender;
    }
}
