using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Client;

public struct HL_EngineInfo
{
    public int TickRate;
    public int MaxFrameRate;
}

namespace TextBasedGameEngine.Engine
{
    public class HL_Engine
    {
        HL_GlobalVarsManager GlobalVarsMgr = null;
        HL_Client Client = null;

        public bool DestructEngine = false;
        public void InitializeEngine(ref HL_EngineInfo InitInfo)
        {
            GlobalVarsMgr = new HL_GlobalVarsManager();
            GlobalVarsMgr.InitializeGlobalVarsMgr(InitInfo);

            Client = new HL_Client();
            Console.WriteLine(GlobalVarsMgr.GVars.IntervalPerTick);     
        }

        public void RunEngine()
        {
            while (!DestructEngine)
            {
                GlobalVarsMgr.OnFrameStart();
                Client.OnFrameStart();

                Client.OnFrameEnd();
                GlobalVarsMgr.OnFrameEnd();

                System.Threading.Thread.Sleep(GlobalVarsMgr.GVars.NextSleep);
            }
        }
    }
}
