namespace BDLib.Text
{
    static class StringHelpers
    {
        public static bool IsWhiteSpaceOrNull(string x)
        {
            if (x == null || x == "")
                return true;
            else
                return false;
        }
    }
}