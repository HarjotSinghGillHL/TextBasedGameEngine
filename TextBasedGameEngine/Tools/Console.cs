using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Tools
{
    public class HL_Console
    {
        public static void SetCursorPos(Vector2 vecPosition)
        {
            Console.SetCursorPosition((int)vecPosition.x, (int)vecPosition.y);
        }
        public static string ChangeStringElement(string sValue, int iIndex, char charValue)
        {
            string strValueOut = "";

            for (int iCurrentElement = 0; iCurrentElement < sValue.Length; iCurrentElement++)
            {
                if (iCurrentElement == iIndex)
                    strValueOut += charValue;
                else
                    strValueOut += sValue[iCurrentElement];
            }

            return strValueOut;
        }

        public static void SetColorAndDrawConsole(char cInput, ConsoleColor Color, ConsoleColor ColorBkg = ConsoleColor.Black)
        {
            ConsoleColor colOriginal = Console.ForegroundColor;
            ConsoleColor colOriginalBkg = Console.BackgroundColor;
            Console.ForegroundColor = Color;
            Console.BackgroundColor = ColorBkg;
            Console.Write(cInput);
            Console.ForegroundColor = colOriginal;
            Console.BackgroundColor = colOriginalBkg;
        }

        public static void SetColorAndDrawConsole(string szInput, ConsoleColor Color, ConsoleColor ColorBkg = ConsoleColor.Black)
        {
            ConsoleColor colOriginal = Console.ForegroundColor;
            ConsoleColor colOriginalBkg = Console.BackgroundColor;
            Console.ForegroundColor = Color;
            Console.BackgroundColor = ColorBkg;
            Console.Write(szInput);
            Console.ForegroundColor = colOriginal;
            Console.BackgroundColor = colOriginalBkg;
        }

        public static void DrawError(string szString, [CallerMemberName] string szMemberName = "", [CallerLineNumber] int iLineNumber = 0)
        {
            SetColorAndDrawConsole("[ERROR MESSAGE from " + $"{szMemberName} at line " + $"{iLineNumber}" + "] " + szString + "\n", ConsoleColor.DarkRed);
        }
    }
}
