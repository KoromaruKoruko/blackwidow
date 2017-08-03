using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BDLib.Reg
{
    public static class Regedit
    {
        //key is directory to folder Valuename is the name of the value in folder that is wanted
        public static T GetValue<T>(string Key, string ValueName)
        {
            T x = (T)Registry.GetValue(Key, ValueName, "NOVALUE");
            return x;
        }
        public static void SetValue(string Key, string ValueName, object NewValue)
        {
            Registry.SetValue(Key, ValueName, NewValue);
        }
        public static bool Exists (string Key,string ValueName)
        {

            if (Registry.GetValue(Key, ValueName, "NOVALUE") == "NOVALUE")
                return false;
            else
                return true;
        }
    }
}
