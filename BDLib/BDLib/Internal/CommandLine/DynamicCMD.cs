using System;
using System.Collections.Generic;

namespace BDLib.Internal.CommandLine
{
    internal struct DynamicCommand
    {
        public EventHandler<string[]> Command;
        public string[] CommandAlias;
    }

    public delegate void DynamicCommandFunction(object s,string[] Args);


    //for external cmd IE all can access without ref of commandline
    public static class OpenDynamicCMD
    {
        private static List<DynamicCommand> Commands = new List<DynamicCommand>();

        public static string CmdList()
        {
            string output = "";

            for (int x = 0; x < Commands.Count; x++)
            {
                output += "{ " + Commands[x].CommandAlias[0];

                for (int y = 1; y < Commands[x].CommandAlias.Length; y++)
                    output += ", " + Commands[x].CommandAlias[y];

                output += "},";
            }

            return output;
        }
        public static int ExecuteCommand(string x, string[] Args)
        {
            bool FindByString(DynamicCommand input)
            {
                for (int y = 0; y < input.CommandAlias.Length; y++)
                    if (input.CommandAlias[y] == x)
                        return true;

                return false;
            }

            int index = Commands.FindIndex(new Predicate<DynamicCommand>(FindByString));
            if (index > 0)
            {
                try {
                    Commands[index].Command.Invoke(null, Args);
                    return 0;
                }
                catch {
                    return 2;
                }
            }
            else
                return 1;
        }

        public static bool CreateCommand(DynamicCommandFunction Func, string[] Aliases)
        {
            DynamicCommand newCom = new DynamicCommand();
            try {
                newCom.Command = new EventHandler<string[]>(Func);
                newCom.CommandAlias = Aliases;
            }
            catch {
                return false;
            }

            for (int x = 0; x < Aliases.Length; x++)
                for (int y = 0; y < Commands.Count; y++)
                    for (int z = 0; z < Commands[y].CommandAlias.Length; z++)
                    {
                        if (Commands[y].CommandAlias[z] == Aliases[x])
                            return false;
                    }
            //if your at this point there inst an Alies that all-ready exists

            Commands.Add(newCom);

            return true;
        }
        public static bool CreateCommand(DynamicCommandFunction Func, string Alias)
        {
            DynamicCommand newCom = new DynamicCommand();
            try
            {
                newCom.Command = new EventHandler<string[]>(Func);
                newCom.CommandAlias = new string[] { Alias };
            }
            catch
            {
                return false;
            }

            for (int y = 0; y < Commands.Count; y++)
                for (int z = 0; z < Commands[y].CommandAlias.Length; z++)
                {
                    if (Commands[y].CommandAlias[z] == Alias)
                        return false;
                }
            //if your at this point there inst an Alies that all-ready exists

            Commands.Add(newCom);

            return true;
        }
    }

    //for internal cmd IE req access to ref of commandline
    public class ClosedDynamicCMD
    {
        private List<DynamicCommand> Commands = new List<DynamicCommand>();

        public string CmdList()
        {
            string output = "";

            for (int x = 0; x < Commands.Count; x++)
            {
                output += "{ " + Commands[x].CommandAlias[0];

                for (int y = 1; y < Commands[x].CommandAlias.Length; y++)
                    output += ", " + Commands[x].CommandAlias[y];

                output += "},";
            }

            return output;
        }
        public int ExecuteCommand(string x, string[] Args)
        {
            bool FindByString(DynamicCommand input)
            {
                for (int y = 0; y < input.CommandAlias.Length; y++)
                    if (input.CommandAlias[y] == x)
                        return true;

                return false;
            }

            int index = Commands.FindIndex(new Predicate<DynamicCommand>(FindByString));
            if (index > 0)
            {
                try
                {
                    Commands[index].Command.Invoke(null, Args);
                    return 0;
                }
                catch
                {
                    return 2;
                }
            }
            else
                return 1;
        }

        public bool CreateCommand(DynamicCommandFunction Func, string[] Aliases)
        {
            DynamicCommand newCom = new DynamicCommand();
            try
            {
                newCom.Command = new EventHandler<string[]>(Func);
                newCom.CommandAlias = Aliases;
            }
            catch
            {
                return false;
            }

            for (int x = 0; x < Aliases.Length; x++)
                for (int y = 0; y < Commands.Count; y++)
                    for (int z = 0; z < Commands[y].CommandAlias.Length; z++)
                    {
                        if (Commands[y].CommandAlias[z] == Aliases[x])
                            return false;
                    }
            //if your at this point there inst an Alies that all-ready exists

            Commands.Add(newCom);

            return true;
        }
        public bool CreateCommand(DynamicCommandFunction Func, string Alias)
        {
            DynamicCommand newCom = new DynamicCommand();
            try
            {
                newCom.Command = new EventHandler<string[]>(Func);
                newCom.CommandAlias = new string[] { Alias };
            }
            catch
            {
                return false;
            }

            for (int y = 0; y < Commands.Count; y++)
                for (int z = 0; z < Commands[y].CommandAlias.Length; z++)
                {
                    if (Commands[y].CommandAlias[z] == Alias)
                        return false;
                }
            //if your at this point there inst an Alies that all-ready exists

            Commands.Add(newCom);

            return true;
        }
    }
}
