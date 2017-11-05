using System;
using System.Collections.Generic;
using BDLib.BDLibInfo;

namespace BDLib.Internal.Chat
{
    public static class InternalChat
    {
        private static List<EventHandler<InternalChatEventArgs>> Events = new List<EventHandler<InternalChatEventArgs>>();

        /// <summary>
        /// adds event function to event list
        /// </summary>
        /// <param name="ReturnFunction">event function</param>
        public static void Join(Action<object,InternalChatEventArgs> ReturnFunction)
        {
            if (!Info.Moduls.Contains("Internal/Chat/InternalChat.cs"))
                Info.Moduls.Add("Internal/Chat/InternalChat.cs");

            Events.Add(new EventHandler<InternalChatEventArgs>(ReturnFunction));
        }
        /// <summary>
        /// removes event function from event list
        /// </summary>
        /// <param name="ReturnFunction">event function</param>
        public static void Part(Action<object,InternalChatEventArgs> ReturnFunction)
        {
            EventHandler<InternalChatEventArgs> TMP = new EventHandler<InternalChatEventArgs>(ReturnFunction);

            if (Events.Contains(TMP))
                Events.Remove(new EventHandler<InternalChatEventArgs>(ReturnFunction));
        }

        /// <summary>
        /// sends a message to the chat
        /// </summary>
        /// <param name="message">the message that needs to be sent</param>
        /// <param name="sender">the sender name</param>
        /// <returns>the amount of ppl that heard</returns>
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
