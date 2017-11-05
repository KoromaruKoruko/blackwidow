using System;


namespace BDLib.Text
{
    public static class ConsoleHelpers
    {
        /// <summary>
        /// all ConsoleColors in string format
        /// incase you need to list them somewhere
        /// </summary>
        public static readonly string[] ConsoleColors = new string[]
        {
            "Black", "Blue", "Cyan", "DarkBlue", "DarkCyan",
            "DarkGray", "DarkGreen", "DarkMagenta", "DarkRed",
            "DarkYellow", "Gray", "Green", "Magenta", "Red",
            "White", "Yellow"
        };

        /// <summary>
        /// converts color name 'string' to ConsoleColor
        /// for user input IDK
        /// </summary>
        /// <param name="color">color name</param>
        /// <returns>ConsoleColor based on color</returns>
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
        /// <summary>
        /// converts ConsoleColor to string
        /// for configfiles IDK
        /// </summary>
        /// <param name="x">ConsoleColor</param>
        /// <returns>string name based on x</returns>
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
