using System;


namespace BDLib.Text
{
    public static class ConsoleHelpers
    {
        public static readonly string[] ConsoleColors = new string[]
        {
            "Black", "Blue", "Cyan", "DarkBlue", "DarkCyan",
            "DarkGray", "DarkGreen", "DarkMagenta", "DarkRed",
            "DarkYellow", "Gray", "Green", "Magenta", "Red",
            "White", "Yellow"
        };

        public static ConsoleColor Str2CC(string color)
        {
            switch (color.ToLower())
            {
                case ("black"):
                    return ConsoleColor.Black;

                case ("blue"):
                    return ConsoleColor.Blue;

                case ("cyan"):
                    return ConsoleColor.Cyan;

                case ("darkblue"):
                    return ConsoleColor.DarkBlue;

                case ("darkcyan"):
                    return ConsoleColor.DarkCyan;

                case ("darkgray"):
                    return ConsoleColor.DarkGray;

                case ("darkgreen"):
                    return ConsoleColor.DarkGreen;

                case ("darkmagenta"):
                    return ConsoleColor.DarkMagenta;

                case ("darkred"):
                    return ConsoleColor.DarkRed;

                case ("darkyellow"):
                    return ConsoleColor.DarkYellow;

                case ("green"):
                    return ConsoleColor.Green;

                case ("magenta"):
                    return ConsoleColor.Magenta;

                case ("red"):
                    return ConsoleColor.Red;

                case ("white"):
                    return ConsoleColor.White;

                case ("yellow"):
                    return ConsoleColor.Yellow;

                default:
                    throw new ArgumentException("no translation");
            }
        }
        public static string CC2Str(ConsoleColor x)
        {
            switch (x)
            {
                case (ConsoleColor.Black):
                    return "Black";

                case (ConsoleColor.Blue):
                    return "Blue";

                case (ConsoleColor.Cyan):
                    return "Cyan";

                case (ConsoleColor.DarkBlue):
                    return "DarkBlue";

                case (ConsoleColor.DarkCyan):
                    return "DarkCyan";

                case (ConsoleColor.DarkGray):
                    return "DarkGray";

                case (ConsoleColor.DarkGreen):
                    return "DarkGreen";

                case (ConsoleColor.DarkMagenta):
                    return "DarkMagenta";

                case (ConsoleColor.DarkRed):
                    return "DarkRed";

                case (ConsoleColor.DarkYellow):
                    return "DarkYellow";

                case (ConsoleColor.Green):
                    return "Green";

                case (ConsoleColor.Magenta):
                    return "Magenta";

                case (ConsoleColor.Red):
                    return "Red";

                case (ConsoleColor.White):
                    return "White";

                case (ConsoleColor.Yellow):
                    return "Yellow";

                default:
                    throw new ArgumentException("no translation");
            }
        }
    }
}
