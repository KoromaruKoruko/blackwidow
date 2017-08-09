using System.Collections.Generic;
using System;

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
        public static int Items { get { return Resorces.Count; } }
        public static Resorce EmptyResorceEntity;
 
        public static void INIT()
        {
            Resorces = new Dictionary<string, Resorce>();



        }

        public static Resorce GetResorce(string ResorceName)
        {
            if (Resorces.ContainsKey(ResorceName))
            {
                return Resorces[ResorceName];
            }
            else return EmptyResorceEntity;
        }
        public static void AddResorce(string ResorceName,Resorce Res)
        {
            if (Resorces.ContainsKey(ResorceName))
                throw new InvalidOperationException("Item Exists");
            else
            {
                Resorces.Add(ResorceName, Res);
            }
        }
        public static void RemoveResorce(string ResorceName)
        {
            if (!Resorces.ContainsKey(ResorceName))
                throw new IndexOutOfRangeException("Item doesn't existed");

            Resorces.Remove(ResorceName);
        }
    }
}
