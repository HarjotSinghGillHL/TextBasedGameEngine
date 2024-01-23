using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Game;

namespace TextBasedGameEngine
{
    internal class HL_Entry
    {
        static void Main(string[] args)
        {
            HL_Engine Engine = new HL_Engine();

            HL_EngineInfo Info;
            Info.TickRate = 128;
            Info.MaxFrameRate = 170.0;
            Info.OnFrameStart = HL_GameClient.OnFrameStart;
            Info.OnTick = HL_GameClient.OnTick;
            Info.OnFrameEnd = HL_GameClient.OnFrameEnd;

            Engine.InitializeEngine(ref Info);
            Engine.RunEngine();
        }
    }
}
