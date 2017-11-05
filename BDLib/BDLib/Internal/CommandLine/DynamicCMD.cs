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

    /// <summary>
    /// template for a DynamicCommand
    /// </summary>
    /// <param name="Args">the arguments send by Executer</param>
    public delegate void DynamicCommandFunction(object[] Args);


    /// <summary>
    /// for matching a function with a name or list of names
    /// </summary>
    public static class OpenDynamicCMD
    {
        /// <summary>
        /// initulizes for use
        /// </summary>
        public static void INIT()
        {
            if (!Info.Moduls.Contains("Internal/CommandLine/DynamicCMD.cs"))
                Info.Moduls.Add("Internal/CommandLine/DynamicCMD.cs");
        }

        private static Dictionary<string, DynamicCommandFunction> Commands = new Dictionary<string, DynamicCommandFunction>();

        /// <summary>
        /// returns the commands In form of a KeyCollection
        /// </summary>
        /// <returns>Commands</returns>
        public static Dictionary<string,DynamicCommandFunction>.KeyCollection CmdList()
        {
            return Commands.Keys;
        }

        /// <summary>
        /// runs a Function based off of a name
        /// </summary>
        /// <param name="x">command name</param>
        /// <param name="Args">data to send it</param>
        /// <returns>if it was executed or not</returns>
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

        /// <summary>
        /// checks if a command name exists
        /// </summary>
        /// <param name="command">command name</param>
        /// <returns>if it exists</returns>
        public static bool Exists(string command)
        {
            return Commands.ContainsKey(command);
        }

        /// <summary>
        /// adds a command with a load of names
        /// </summary>
        /// <param name="Func">delegate: Function</param>
        /// <param name="Aliases">the names to match to Function with</param>
        /// <returns>if it was added or not</returns>
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
        /// <summary>
        /// adds a command with one name
        /// </summary>
        /// <param name="Func">delegate: Function</param>
        /// <param name="Alias">the name to match to Function with</param>
        /// <returns>if it was added or not</returns>
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
    
    /// <summary>
    /// for matching a function with a name or list of names,
    /// without letting everyone access the function list
    /// </summary>
    public class ClosedDynamicCMD
    {
        private Dictionary<string, DynamicCommandFunction> Commands = new Dictionary<string, DynamicCommandFunction>();
        
        public ClosedDynamicCMD()
        {
            if (!Info.Moduls.Contains("Internal/CommandLine/DynamicCMD.cs"))
                Info.Moduls.Add("Internal/CommandLine/DynamicCMD.cs");
        }
        /// <summary>
        /// returns the commands In form of a KeyCollection
        /// </summary>
        /// <returns>Commands</returns>
        public Dictionary<string, DynamicCommandFunction>.KeyCollection CmdList()
        {
            return Commands.Keys;
        }

        /// <summary>
        /// runs a Function based off of a name
        /// </summary>
        /// <param name="x">command name</param>
        /// <param name="Args">data to send it</param>
        /// <returns>if it was executed or not</returns>
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

        /// <summary>
        /// checks if a command name exists
        /// </summary>
        /// <param name="command">command name</param>
        /// <returns>if it exists</returns>
        public bool Exists(string command)
        {
            return Commands.ContainsKey(command);
        }

        /// <summary>
        /// adds a command with a load of names
        /// </summary>
        /// <param name="Func">delegate: Function</param>
        /// <param name="Aliases">the names to match to Function with</param>
        /// <returns>if it was added or not</returns>
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
        /// <summary>
        /// adds a command with one name
        /// </summary>
        /// <param name="Func">delegate: Function</param>
        /// <param name="Alias">the name to match to Function with</param>
        /// <returns>if it was added or not</returns>
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
