namespace BDLib.Text
{
    public static class StringHelpers
    {
        /// <summary>
        /// checks if a string is empty or null
        /// </summary>
        /// <param name="x">input string</param>
        /// <returns>if string is empty or null</returns>
        public static bool IsWhiteSpaceOrNull(string x)
        {
            if (x == null || x == "")
                return true;
            else
                return false;
        }
        /// <summary>
        /// looks if all items in string array are you same
        /// </summary>
        /// <param name="a">string array A</param>
        /// <param name="b">string array B</param>
        /// <returns>if all items in string array are you same</returns>
        public static bool AreTheSame(string[] a, string[] b)
        {
            bool output = true;

            if (a.Length != b.Length)
                output = false;
            else
                for (int x = 0; x < a.Length; x++)
                {
                    if (a[x] != b[x])
                    {

                        output = false;
                        break;
                    }
                }

            return output;
        }
    }
}