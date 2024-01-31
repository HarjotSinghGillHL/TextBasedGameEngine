using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_MapManager
    {
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }

        public static void WriteMapCharacter(char MapChar, ConsoleColor BkgColor = ConsoleColor.Black)
        {

            switch (MapChar)
            {

                case 'E':  // Enemy player
                    {
                        HL_Console.SetColorAndDrawConsole('E', ConsoleColor.Gray, BkgColor);
                        break;
                    }
                case 'P':  // local player
                    {
                        HL_Console.SetColorAndDrawConsole('P', ConsoleColor.Gray, BkgColor);
                        break;
                    }
                case 'M':  // mountains
                    {
                        HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.DarkGray, ConsoleColor.DarkGray);
                        break;
                    }
                case 'W': // water
                    {
                        HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.Blue, ConsoleColor.Blue);
                        break;
                    }
                case 'G':   // grass
                    {
                        HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.DarkGreen, ConsoleColor.DarkGreen);
                        break;
                    }
                case 'T':  // trees
                    {
                        HL_Console.SetColorAndDrawConsole('\u00A5', ConsoleColor.Green, ConsoleColor.DarkGreen);
                        break;
                    }
                case 'L':   // lava
                    {
                        HL_Console.SetColorAndDrawConsole('\u2592', ConsoleColor.Red, ConsoleColor.DarkRed);
                        break;
                    }
                case 'C':   // coins
                    {
                        HL_Console.SetColorAndDrawConsole('O', ConsoleColor.Yellow, ConsoleColor.DarkGreen);
                        break;
                    }
                case 'H':   // health pickup
                    {
                        HL_Console.SetColorAndDrawConsole('\u2665', ConsoleColor.DarkRed, ConsoleColor.DarkGreen);
                        break;
                    }
                case 'S':   // shield pickup
                    {
                        HL_Console.SetColorAndDrawConsole('\u2665', ConsoleColor.Blue, ConsoleColor.DarkGreen);
                        break;
                    }
                default:
                    {
                        System.Console.Write(MapChar);
                        break;
                    }

            }

        }
        public bool Load(string Text)
        {
            List<string> Map = HL_FileSystem.ReadFileLineByLine("Map.txt");

            if (Map.Count == 0)
                return false;

            int iLongestRowLength = HL_FileSystem.GetLengthOfTheLongestRow(ref Map);

            System.Console.Write('\u250C');

            for (int iBorderChar = 0; iBorderChar < iLongestRowLength; iBorderChar++)
                System.Console.Write('\u2500');

            System.Console.Write('\u2510');

            System.Console.WriteLine();

            int iLegendPadding = 0;

            for (int iCurrent = 0; iCurrent < Map.Count; iCurrent++)
            {
                System.Console.Write('\u2502');

                for (int iIndex = 0; iIndex < Map[iCurrent].Length; iIndex++)
                {
                    if (Map[iCurrent][iIndex] == 'P' || Map[iCurrent][iIndex] == 'E')
                        WriteMapCharacter(Map[iCurrent][iIndex], ConsoleColor.DarkGreen);
                    else
                        WriteMapCharacter(Map[iCurrent][iIndex]);
                }

                System.Console.Write('\u2502');

                DrawLegend(ref iLegendPadding);

                System.Console.WriteLine();
            }

            System.Console.Write('\u2514');

            for (int iBorderChar = 0; iBorderChar < iLongestRowLength; iBorderChar++)
                System.Console.Write('\u2500');

            System.Console.Write('\u2518');

            System.Console.WriteLine();

            return true;
        }

        static void DrawLegend(ref int iLegendPadding)
        {
            if (iLegendPadding <= 9)
            {
                Console.Write(" ");

                switch (iLegendPadding)
                {
                    case 0:
                        {
                            Console.Write("=======LEGEND======");
                            break;
                        }
                    case 1:
                        {
                            Console.Write("Mountains = ");
                            WriteMapCharacter('M');
                            break;
                        }
                    case 2:
                        {
                            Console.Write("Water = ");
                            WriteMapCharacter('W');
                            break;
                        }
                    case 3:
                        {
                            Console.Write("Grass = ");
                            WriteMapCharacter('G');
                            break;
                        }
                    case 4:
                        {
                            Console.Write("Trees = ");
                            HL_Console.SetColorAndDrawConsole('\u00A5', ConsoleColor.Green);
                            break;
                        }
                    case 5:
                        {
                            Console.Write("Lava = ");
                            WriteMapCharacter('L');
                            break;
                        }
                    case 6:
                        {
                            Console.Write("Coins = ");
                            HL_Console.SetColorAndDrawConsole('O', ConsoleColor.Yellow);
                            break;
                        }
                    case 7:
                        {
                            Console.Write("Heal Pickup = ");
                            HL_Console.SetColorAndDrawConsole('\u2665', ConsoleColor.DarkRed);
                            break;
                        }
                    case 8:
                        {
                            Console.Write("Shield Pickup = ");
                            HL_Console.SetColorAndDrawConsole('\u2665', ConsoleColor.Blue);
                            break;
                        }
                    case 9:
                        {
                            Console.Write("===================");
                            break;
                        }
                    default:
                        {
                            break;
                        }


                }

                iLegendPadding += 1;
            }

        }
    }
}
