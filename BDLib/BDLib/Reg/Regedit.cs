using Microsoft.Win32;

//WindowsOnly



namespace BDLib.Reg
{
    public static class Regedit
    {
        //key is directory to folder Valuename is the name of the value in folder that is wanted
        /// <summary>
        /// returns a value in the Registry based on Key and ValueName
        /// </summary>
        /// <typeparam name="T">how should i interprit the data</typeparam>
        /// <param name="Key">the path to the Value</param>
        /// <param name="ValueName">the name of the Value</param>
        /// <returns>the Value</returns>
        public static T GetValue<T>(string Key, string ValueName)
        {
            T x = (T)Registry.GetValue(Key, ValueName, "NOVALUE");
            return x;
        }
        /// <summary>
        /// sets the Value to 'NewValue'
        /// </summary>
        /// <param name="Key">path to Value</param>
        /// <param name="ValueName">the name of the Value</param>
        /// <param name="NewValue">the Value to give it</param>
        public static void SetValue(string Key, string ValueName, object NewValue)
        {
            Registry.SetValue(Key, ValueName, NewValue);
        }
        /// <summary>
        /// looks to see if the value exists
        /// </summary>
        /// <param name="Key">path to value</param>
        /// <param name="ValueName">name of value</param>
        /// <returns>if it exists</returns>
        public static bool Exists (string Key,string ValueName)
        {

            if (Registry.GetValue(Key, ValueName, "NOVALUE") == "NOVALUE")//its fine
                return false;
            else
                return true;
        }
    }
}
