using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine.Engine.RenderSystem
{


    public class HL_HudOverlay
    {
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }

        static Vector2 vecStoredHealthBarStart;
        static Vector2 vecStoredShieldBarStart;
        static Vector2 vecStoredLivesStart;
        static Vector2 vecStoredCoinsStart;
        static Vector2 vecStoredScoreStart;
        public static void InitHud(int iLives, int iCoinsOnMap)
        {
            Console.Write("Health : ");

            vecStoredHealthBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            for (int i = 0; i < 10; i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Red);

            Console.Write(" 100");

            Console.WriteLine();
            Console.Write("Shield : ");

            vecStoredShieldBarStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            for (int i = 0; i < 10; i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.DarkBlue);

            Console.Write(" 100");

            Console.WriteLine();
            Console.Write("Lives : ");

            vecStoredLivesStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(iLives);

            Console.WriteLine();

            Console.Write("Coins (" + iCoinsOnMap + " on map) : ");

            vecStoredCoinsStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(0);

            Console.WriteLine();

            Console.Write("Score : ");

            vecStoredScoreStart = new Vector2(Console.CursorLeft, Console.CursorTop);

            Console.Write(0);

        }

        public static void UpdateHud(int iHealth, int iShield, int iLives, int iCoinsCollected, int iScore)
        {
            Console.SetCursorPosition((int)vecStoredHealthBarStart.x, (int)vecStoredHealthBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecStoredHealthBarStart.x, (int)vecStoredHealthBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iHealth / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.Red);

            Console.Write(" " + iHealth);

            Console.WriteLine();

            Console.Write("Shield : ");

            Console.SetCursorPosition((int)vecStoredShieldBarStart.x, (int)vecStoredShieldBarStart.y);

            for (int i = 0; i < 10; i++)
                Console.Write(" ");

            Console.Write("    ");

            Console.SetCursorPosition((int)vecStoredShieldBarStart.x, (int)vecStoredShieldBarStart.y);

            for (int i = 0; i < Math.Ceiling((decimal)(iShield / 10.0)); i++)
                HL_Console.SetColorAndDrawConsole("\u25A0", ConsoleColor.DarkBlue);

            Console.Write(" " + iShield);

            Console.SetCursorPosition((int)vecStoredLivesStart.x, (int)vecStoredLivesStart.y);
            Console.Write("            ");
            Console.SetCursorPosition((int)vecStoredLivesStart.x, (int)vecStoredLivesStart.y);
            Console.Write(iLives);

            Console.SetCursorPosition((int)vecStoredCoinsStart.x, (int)vecStoredCoinsStart.y);
            Console.Write("            ");
            Console.SetCursorPosition((int)vecStoredCoinsStart.x, (int)vecStoredCoinsStart.y);
            Console.Write(iCoinsCollected);

            Console.SetCursorPosition((int)vecStoredScoreStart.x, (int)vecStoredScoreStart.y);
            Console.Write("            ");
            Console.SetCursorPosition((int)vecStoredScoreStart.x, (int)vecStoredScoreStart.y);
            Console.Write(iScore);
        }

    }
}
