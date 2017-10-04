namespace BDLib.Text
{
    public static class StringHelpers
    {
        public static bool IsWhiteSpaceOrNull(string x)
        {
            if (x == null || x == "")
                return true;
            else
                return false;
        }
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