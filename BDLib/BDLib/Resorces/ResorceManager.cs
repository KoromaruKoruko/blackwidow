using System.Collections.Generic;
using System;
using BDLib.BDLibInfo;

namespace BDLib.Resorces
{

    public struct Resorce
    {
        object Item;
        Type ItemType;
    }

    public static class ResorceManager
    {
        private static Dictionary<string, Resorce> Resorces;
        /// <summary>
        /// the total amount of resources i have
        /// </summary>
        public static int TotalResorce { get { return Resorces.Count; } }
        /// <summary>
        /// an empty resource
        /// </summary>
        public static Resorce EmptyResorceEntity;

        /// <summary>
        /// initulizes for use
        /// </summary>
        public static void INIT()
        {
            if (!Info.Moduls.Contains("Resorces/ResorceManager.cs"))
                Info.Moduls.Add("Resorces/ResorceManager.cs");

            Resorces = new Dictionary<string, Resorce>();
        }

        /// <summary>
        /// returns a resource based on name
        /// </summary>
        /// <param name="ResorceName">the resource name</param>
        /// <returns>resource</returns>
        public static Resorce GetResorce(string ResorceName)
        {
            if (Resorces.ContainsKey(ResorceName))
            {
                return Resorces[ResorceName];
            }
            else return EmptyResorceEntity;
        }
        /// <summary>
        /// adds a resource
        /// </summary>
        /// <param name="ResorceName">the name of the resource</param>
        /// <param name="Res">the resource</param>
        public static void AddResorce(string ResorceName, Resorce Res)
        {
            if (Resorces.ContainsKey(ResorceName))
                throw new InvalidOperationException("Item Exists");
            else
            {
                Resorces.Add(ResorceName, Res);
            }
        }
        /// <summary>
        /// removes a resource based on name
        /// </summary>
        /// <param name="ResorceName">name of resource</param>
        public static void RemoveResorce(string ResorceName)
        {
            if (!Resorces.ContainsKey(ResorceName))
                throw new IndexOutOfRangeException("Item doesn't existed");

            Resorces.Remove(ResorceName);
        }
    }
}
