﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.GameManager;

namespace TextBasedGameEngine.Game
{
    public static class HL_GameClient
    {
        public static void OnEngineInitialize(HL_Engine Engine)
        {
            Engine.GameManager.MapManager.Load("Map.txt");
        }
        public static void OnFrameStart(HL_Engine Engine)
        {
            HL_MapManager.HandleLocalPlayer();

        }
        public static void OnTick(HL_Engine Engine)
        {
            HL_MapManager.HandleEntities();
        }

        public static void OnFrameEnd(HL_Engine Engine)
        {
         
        }
    }

 
}
