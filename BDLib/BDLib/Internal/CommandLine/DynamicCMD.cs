using System;
using System.Collections.Generic;
using BDLib.BDLibInfo;

namespace BDLib.Internal.CommandLine
{
    internal struct DynamicCommand
    {
        public EventHandler<string[]> Command;
        public string[] CommandAlias;
    }

    public delegate void DynamicCommandFunction(object[] Args);


    //for external cmd IE all can access without ref of commandline
    public static class OpenDynamicCMD
    {
        private static void INIT()
        {
            if (!Info.Moduls.Contains("Internal/CommandLine/DynamicCMD.cs"))
                Info.Moduls.Add("Internal/CommandLine/DynamicCMD.cs");
        }

        private static Dictionary<string, DynamicCommandFunction> Commands = new Dictionary<string, DynamicCommandFunction>();

        public static Dictionary<string,DynamicCommandFunction>.KeyCollection CmdList()
        {
            return Commands.Keys;
        }
        public static bool ExecuteCommand(string x, object[] Args)
        {
            try
            {
                Commands[x].Invoke(Args);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool Exists(string command)
        {
            return Commands.ContainsKey(command);
        }


        public static bool CreateCommand(DynamicCommandFunction Func, string[] Aliases)
        {
            for (int x = 0; x < Aliases.Length; x++)
            {
                if (Commands.ContainsKey(Aliases[x])) return false;
            }

            for(int x = 0; x < Aliases.Length; x++)
            {
                Commands.Add(Aliases[x], Func);
            }

            return true;
        }
        public static bool CreateCommand(DynamicCommandFunction Func, string Alias)
        {
            try
            {
                Commands.Add(Alias, Func);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    //for internal cmd IE req access to ref of commandline
    public class ClosedDynamicCMD
    {
        private Dictionary<string, DynamicCommandFunction> Commands = new Dictionary<string, DynamicCommandFunction>();

        public ClosedDynamicCMD()
        {
            if (!Info.Moduls.Contains("Internal/CommandLine/DynamicCMD.cs"))
                Info.Moduls.Add("Internal/CommandLine/DynamicCMD.cs");
        }

        public Dictionary<string, DynamicCommandFunction>.KeyCollection CmdList()
        {
            return Commands.Keys;
        }
        public bool ExecuteCommand(string x, object[] Args)
        {
            try
            {
                Commands[x].Invoke(Args);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Exists(string command)
        {
            return Commands.ContainsKey(command);
        }

        public bool CreateCommand(DynamicCommandFunction Func, string[] Aliases)
        {
            for (int x = 0; x < Aliases.Length; x++)
            {
                if (Commands.ContainsKey(Aliases[x])) return false;
            }

            for (int x = 0; x < Aliases.Length; x++)
            {
                Commands.Add(Aliases[x], Func);
            }

            return true;
        }
        public bool CreateCommand(DynamicCommandFunction Func, string Alias)
        {
            try
            {
                Commands.Add(Alias, Func);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
