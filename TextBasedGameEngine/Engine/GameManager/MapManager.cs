using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_MapManager
    {
        HL_Engine OwningEngine = null;

        static public HL_BaseEntity LocalPlayer;
        static public List<string> LoadedMap;
        static public List<string> OriginalMapStored;

        static private Vector2 vecLocalPlayerPositionStart;
        static private Vector2 vecLocalPlayerPosition;
        static private Vector2 vecMapRenderPositionStart;
        static HL_PlayerHurt PlayerHurtState;
        static private int iLives = 1;
        static private int iCoinsCollected = 0;
        static private int iScore = 0;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }

        public static char LocalPlayerChar = 'P';
        public static bool IsBlockedRegionChar(char Value)
        {
            return Value == 'M' || Value == 'T';
        }
        public static bool IsTireHarmful(char Value)
        {
            return Value == 'L';
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
        static void RestoreMap()
        {
            LoadedMap.Clear();
            LoadedMap.AddRange(OriginalMapStored);
        }

        static private bool bDiedScreenStart = false;
        static private int iDeathScreenTickCount = 0;
        static private int iOldDeathScreenTickCount = 0;
        static private bool bEnteredLava = false;

        static void YouDiedScreen()
        {
            if (!bDiedScreenStart)
            {
                iDeathScreenTickCount = 0;
                HL_Console.SetCursorPos(new Vector2(vecMapRenderPositionStart.x, vecMapRenderPositionStart.y));

                decimal MiddleCount = LoadedMap.Count;
                MiddleCount /= (decimal)2.0;
                MiddleCount = Math.Ceiling(MiddleCount);

                string szDiedScreenText = "You Died! (";
                szDiedScreenText += "Respawning)";

                if (iLives == -1)
                    szDiedScreenText = "Game over (esc to quit)";

                decimal MiddleCountOfText = szDiedScreenText.Length;
                MiddleCountOfText /= (decimal)2.0;
                MiddleCountOfText = Math.Ceiling(MiddleCountOfText);

                for (int iCurrent = 0; iCurrent < LoadedMap.Count; iCurrent++)
                {
                    int iLengthToSubstract = 0;

                    if (iCurrent == MiddleCount)
                    {
                        iLengthToSubstract = szDiedScreenText.Length - 1;
                    }

                    decimal MiddleCountOfIndex = LoadedMap[iCurrent].Length;
                    MiddleCountOfIndex /= (decimal)2.0;
                    MiddleCountOfIndex = Math.Ceiling(MiddleCountOfIndex);
                    MiddleCountOfIndex -= Math.Ceiling(MiddleCountOfText);

                    for (int iIndex = 0; iIndex < LoadedMap[iCurrent].Length - iLengthToSubstract; iIndex++)
                    {
                        if (iLengthToSubstract != 0 && MiddleCountOfIndex == iIndex)
                            Console.Write(szDiedScreenText);
                        else
                            Console.Write(" ");
                    }

                    Console.WriteLine();
                    HL_Console.SetCursorPos(new Vector2(Console.CursorLeft + vecMapRenderPositionStart.x, Console.CursorTop));
                }

                bDiedScreenStart = true;
            }

            if (iOldDeathScreenTickCount != Environment.TickCount)
            {
                ++iDeathScreenTickCount;
                iOldDeathScreenTickCount = Environment.TickCount;
            }

            if (iLives == -1)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                    if (KeyInfo.Key == ConsoleKey.Escape)
                        Environment.Exit(0);

                }

                iDeathScreenTickCount = 0;
            }

            if (iDeathScreenTickCount >= 100)
            {
                RestoreMap();
                HL_Console.SetCursorPos(new Vector2(0, 1));

                for (int iCurrent = 0; iCurrent < LoadedMap.Count; iCurrent++)
                {
                    Console.Write('\u2502');

                    for (int iIndex = 0; iIndex < LoadedMap[iCurrent].Length; iIndex++)
                    {
                        HL_MapManager.WriteMapCharacter(LoadedMap[iCurrent][iIndex]);
                    }

                    Console.Write('\u2502');

                    Console.WriteLine();
                }

                bDiedScreenStart = false;
                LocalPlayer.Spawn();

                HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);
                vecLocalPlayerPosition = vecLocalPlayerPositionStart;
            }
        }

        static bool HandleLocalPlayerMovement()
        {
            bool bReturn = false;

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                switch (KeyInfo.Key)
                {
                    case ConsoleKey.W:
                        {
                            if (vecLocalPlayerPosition.y != 0)
                            {
                                if (!IsBlockedRegionChar(LoadedMap[(int)vecLocalPlayerPosition.y - 1][(int)vecLocalPlayerPosition.x]))
                                {
                                    SetPositionAndDrawChar(LoadedMap, vecMapRenderPositionStart, vecLocalPlayerPosition);

                                    vecLocalPlayerPosition.y -= 1;

                                    SetPositionAndDrawChar(LocalPlayerChar, vecMapRenderPositionStart + vecLocalPlayerPosition, GetBkgColorBasedOfTile(OriginalMapStored[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x]));

                                    bReturn = true;
                                }
                            }
                            break;
                        }
                    case ConsoleKey.A:
                        {
                            if (vecLocalPlayerPosition.x != 0)
                            {
                                if (!IsBlockedRegionChar(LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x - 1]))
                                {
                                    SetPositionAndDrawChar(LoadedMap, vecMapRenderPositionStart, vecLocalPlayerPosition);

                                    vecLocalPlayerPosition.x -= 1;

                                    SetPositionAndDrawChar(LocalPlayerChar, vecMapRenderPositionStart + vecLocalPlayerPosition, GetBkgColorBasedOfTile(OriginalMapStored[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x]));

                                    bReturn = true;
                                }
                            }
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            if (vecLocalPlayerPosition.y < LoadedMap.Count - 1)
                            {
                                if (!IsBlockedRegionChar(LoadedMap[(int)vecLocalPlayerPosition.y + 1][(int)vecLocalPlayerPosition.x]))
                                {
                                    SetPositionAndDrawChar(LoadedMap, vecMapRenderPositionStart, vecLocalPlayerPosition);

                                    vecLocalPlayerPosition.y += 1;

                                    SetPositionAndDrawChar(LocalPlayerChar, vecMapRenderPositionStart + vecLocalPlayerPosition, GetBkgColorBasedOfTile(OriginalMapStored[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x]));

                                    bReturn = true;
                                }

                            }

                            break;
                        }
                    case ConsoleKey.D:
                        {

                            if (vecLocalPlayerPosition.x <LoadedMap[(int)vecLocalPlayerPosition.y].Length - 1)
                            {
                                if (!IsBlockedRegionChar(LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x + 1]))
                                {
                                    SetPositionAndDrawChar(LoadedMap, vecMapRenderPositionStart, vecLocalPlayerPosition);

                                    vecLocalPlayerPosition.x += 1;

                                    SetPositionAndDrawChar(LocalPlayerChar, vecMapRenderPositionStart + vecLocalPlayerPosition, GetBkgColorBasedOfTile(OriginalMapStored[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x]));

                                    bReturn = true;
                                }
                            }

                            break;
                        }
                }
            }

            return bReturn;
        }

        public static void HandleEntities()
        {
            if (!LocalPlayer.IsAlive())
                return;

            if (HL_EntityManager.EnemyPlayers.Count == 0)
                return;

            for (int iCurrent = 0; iCurrent < HL_EntityManager.EnemyPlayers.Count; iCurrent++)
            {
                if (HL_EntityManager.EnemyPlayers[iCurrent].ShouldMove())
                {
                    char ReplacementChar = OriginalMapStored[(int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y][(int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.x];

                    if (ReplacementChar == 'E' || (ReplacementChar == 'C' && IsCoinConsumed(HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap)))
                        ReplacementChar = 'G';


                    LoadedMap[(int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y] = HL_Console.ChangeStringElement(LoadedMap[(int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y], (int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.x, ReplacementChar);

                    SetPositionAndDrawChar(LoadedMap, vecMapRenderPositionStart, HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap);


                    HL_EntityManager.EnemyPlayers[iCurrent].UpdatePosition(LoadedMap[(int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y][HL_Math.Clamp(HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.x - 1, 0, LoadedMap[HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y].Length - 1)], LoadedMap[HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y][HL_Math.Clamp(HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.x + 1, 0, LoadedMap[HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y].Length - 1)]
                        , LoadedMap[(int)HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y].Length);


                    LoadedMap[HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y] = HL_Console.ChangeStringElement(LoadedMap[HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y], HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.x, 'E');
                    SetPositionAndDrawChar(LoadedMap, vecMapRenderPositionStart, HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap, GetBkgColorBasedOfTile(OriginalMapStored[HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.y][HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap.x]));

                    if (HL_EntityManager.EnemyPlayers[iCurrent].vecPositionOnMap == vecLocalPlayerPosition)
                    {
                        iLives -= 1;
                        HL_DamageHandler.HurtEntity(200, LocalPlayer);

                        if (iLives != -1)
                        {
                            vecLocalPlayerPosition = vecLocalPlayerPositionStart;
                            LocalPlayer.Spawn();
                            SetPositionAndDrawChar(LocalPlayerChar, vecMapRenderPositionStart + vecLocalPlayerPosition, ConsoleColor.DarkGreen);
                            iScore -= 300;
                            HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);
                        }
                    }

                }
            }
        }
        public static void HandleLocalPlayer()
        {
        LABEL_HANDLE_LOCAL_START:

            if (!LocalPlayer.IsAlive())
            {
                YouDiedScreen();
                return;
            }

            bDiedScreenStart = false;

            char OldMapChar =LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x];

            bool bMoved = HandleLocalPlayerMovement();

            if (bMoved)
            {
                if (LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'E')
                {
                   LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                    iScore += 500;
                    HL_EntityManager.RemoveEntityFromList(ref HL_EntityManager.EnemyPlayers, vecLocalPlayerPosition);
                    HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);
                }
            }

            if (LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'C')
            {
               LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                iCoinsCollected++;
                iScore += 100;
                HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);
                HL_EntityManager.CoinsConsumed.Add(vecLocalPlayerPosition);
            }

            if (LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'H')
            {
                if (HL_DamageHandler.HealEntity(50, LocalPlayer))
                {
                   LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                    HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);
                }
            }

            if (LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'S')
            {
                if (HL_DamageHandler.GainShield(50, LocalPlayer))
                {
                   LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
                    HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);
                }
            }

            if (LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x] == 'L')
            {
                if (!bEnteredLava)
                {
                    PlayerHurtState = new HL_PlayerHurt(40);
                    bEnteredLava = true;
                }
                else
                {
                    if (PlayerHurtState.ShouldHurtPlayer())
                    {
                        iScore -= 50;
                        HL_DamageHandler.HurtEntity(25, LocalPlayer);
                        HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);

                        if (!LocalPlayer.IsAlive())
                        {
                            iLives -= 1;
                            if (iLives == -1)
                                goto LABEL_HANDLE_LOCAL_START;
                            else
                            {
                                SetPositionAndDrawChar(LoadedMap[(int)vecLocalPlayerPosition.y][(int)vecLocalPlayerPosition.x], vecMapRenderPositionStart + vecLocalPlayerPosition);
                                vecLocalPlayerPosition = vecLocalPlayerPositionStart;
                                LocalPlayer.Spawn();
                                SetPositionAndDrawChar(LocalPlayerChar, vecMapRenderPositionStart + vecLocalPlayerPosition, ConsoleColor.DarkGreen);
                                HL_HudOverlay.UpdateHud(LocalPlayer.GetHealth(), LocalPlayer.GetShield(), iLives, iCoinsCollected, iScore);

                            }
                        }
                    }
                }
            }
            else
            {
                bEnteredLava = false;
            }
        }
        public bool Load(string Text)
        {
            LoadedMap = HL_FileSystem.ReadFileLineByLine(Text);

            if (LoadedMap.Count == 0)
                return false;

            int iLongestRowLength = HL_FileSystem.GetLengthOfTheLongestRow(ref LoadedMap);

            System.Console.Write('\u250C');

            for (int iBorderChar = 0; iBorderChar < iLongestRowLength; iBorderChar++)
                System.Console.Write('\u2500');

            System.Console.Write('\u2510');

            System.Console.WriteLine();

            int iLegendPadding = 0;

            for (int iCurrent = 0; iCurrent < LoadedMap.Count; iCurrent++)
            {
                System.Console.Write('\u2502');

                for (int iIndex = 0; iIndex < LoadedMap[iCurrent].Length; iIndex++)
                {
                    if (LoadedMap[iCurrent][iIndex] == 'P' || LoadedMap[iCurrent][iIndex] == 'E')
                        WriteMapCharacter(LoadedMap[iCurrent][iIndex], ConsoleColor.DarkGreen);
                    else
                        WriteMapCharacter(LoadedMap[iCurrent][iIndex]);
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

            if (OriginalMapStored == null)
                OriginalMapStored = new List<string>();

            OriginalMapStored.Clear();
            OriginalMapStored.AddRange(LoadedMap);

            vecLocalPlayerPosition = HL_EntityManager.FindLocalPlayerInMap(OriginalMapStored);
            LoadedMap[(int)vecLocalPlayerPosition.y] = HL_Console.ChangeStringElement(LoadedMap[(int)vecLocalPlayerPosition.y], (int)vecLocalPlayerPosition.x, 'G');
            vecLocalPlayerPositionStart = vecLocalPlayerPosition;
            vecMapRenderPositionStart = new Vector2(1, 1);

            iLives = 2;
            iCoinsCollected = 0;

            HL_EntityManager.CoinsConsumed = new List<Vector2>();
            HL_EntityManager.CoinsConsumed.Clear();

            LocalPlayer = new HL_BaseEntity("LocalPlayer");
            HL_EntityManager.EnemyPlayers = HL_EntityManager.GenerateMapEntityList(OriginalMapStored);

            HL_HudOverlay.InitHud(iLives, GetCountOfCoinsOnMap(LoadedMap));

            return true;
        }

        public static int GetCountOfCoinsOnMap(List<string> Map)
        {
            int iCount = 0;
            for (int iCurrentListIndex = 0; iCurrentListIndex < Map.Count; iCurrentListIndex++)
            {
                for (int iCurrentStringIndex = 0; iCurrentStringIndex <LoadedMap[iCurrentListIndex].Count(); iCurrentStringIndex++)
                {
                    if (LoadedMap[iCurrentListIndex][iCurrentStringIndex] == 'C')
                        ++iCount;
                }
            }

            return iCount;
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

        public static ConsoleColor GetBkgColorBasedOfTile(char Tile)
        {
            switch (Tile)
            {

                case 'M':
                    {
                        return ConsoleColor.DarkGray;
                    }
                case 'W':
                    {
                        return ConsoleColor.Blue;
                    }

                case 'L':
                    {
                        return ConsoleColor.DarkRed;
                    }
                case 'P':
                case 'G':
                case 'C':
                case 'E':
                case 'H':
                case 'S':
                    {
                        return ConsoleColor.DarkGreen;
                    }
                default:
                    {
                        return Console.BackgroundColor;
                    }

            }
        }
        static void SetPositionAndDrawChar(List<string> Map, Vector2 vecRenderPositionStart, Vector2 vecElementPosition, ConsoleColor BkgColor = ConsoleColor.Black)
        {
            HL_Console.SetCursorPos(vecRenderPositionStart + vecElementPosition);

            if (Map[(int)vecElementPosition.y][(int)vecElementPosition.x] == HL_MapManager.LocalPlayerChar)
                HL_Console.SetColorAndDrawConsole(' ', ConsoleColor.White, BkgColor);
            else
                HL_MapManager.WriteMapCharacter(Map[(int)vecElementPosition.y][(int)vecElementPosition.x], BkgColor);
        }

        static void SetPositionAndDrawChar(char Char, Vector2 vecPosition, ConsoleColor BkgColor = ConsoleColor.Black)
        {
            HL_Console.SetCursorPos(vecPosition);
            HL_MapManager.WriteMapCharacter(Char, BkgColor);
        }

        static bool IsCoinConsumed(Vector2 vecPosition)
        {
            if (HL_EntityManager.CoinsConsumed.Count == 0)
                return false;

            for (int iCurrent = 0; iCurrent < HL_EntityManager.CoinsConsumed.Count; iCurrent++)
            {
                if (HL_EntityManager.CoinsConsumed[iCurrent] == vecPosition)
                    return true;
            }

            return false;
        }
    }
}
