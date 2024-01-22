using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;

namespace TextBasedGameEngine
{
    internal class HL_Entry
    {
        static void Main(string[] args)
        {
            HL_Engine Engine = new HL_Engine();

            HL_EngineInfo Info;
            Info.TickRate = 128;
            Info.MaxFrameRate = 170;

            Engine.InitializeEngine(ref Info);
            Engine.RunEngine();
        }
    }
}
