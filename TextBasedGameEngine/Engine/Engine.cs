using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Client;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Game;
using TextBasedGameEngine.Tools;

public struct HL_EngineInfo
{
    public double TickRate;
    public double MaxFrameRate;
    public Action<HL_Engine, HL_Client> OnFrameStart;
    public Action<HL_Engine, HL_Client> OnTick;
    public Action<HL_Engine, HL_Client> OnFrameEnd;
}

namespace TextBasedGameEngine.Engine
{
    public class HL_Engine
    {
        public HL_GlobalVarsManager GlobalVarsMgr = null;
        public HL_Client Client = null;
        public HL_RenderSystem RenderInstance = null;

        public bool DestructEngine = false;
        double LastUpdateTime = 0;
        public void InitializeEngine(ref HL_EngineInfo InitInfo)
        {

            GlobalVarsMgr = new HL_GlobalVarsManager();
            RenderInstance = new HL_RenderSystem();
            GlobalVarsMgr.InitializeGlobalVarsMgr(InitInfo);
            RenderInstance.InitializeRenderSystem(this,Client);
            Client = new HL_Client();
        }

        public void FrameStart()
        {
            GlobalVarsMgr.OnFrameStart();
            Client.OnFrameStart();
            HL_GameClientManager.RunGameClientOnFrameStart(this, Client, GlobalVarsMgr.GVars.EngineInfo.OnFrameStart);

            if ((GlobalVarsMgr.GVars.CurrentTime - LastUpdateTime) >= GlobalVarsMgr.GVars.IntervalPerTick)
            {
                OnTick();
                LastUpdateTime = GlobalVarsMgr.GVars.CurrentTime;
            }

        }

        public void OnTick()
        {
            HL_GameClientManager.RunGameClientOnTick(this, Client, GlobalVarsMgr.GVars.EngineInfo.OnTick);
            GlobalVarsMgr.OnTick();
            Client.OnTick();
        }
        public void FrameEnd()
        {
            HL_GameClientManager.RunGameClientOnFrameEnd(this, Client, GlobalVarsMgr.GVars.EngineInfo.OnFrameEnd);
            Client.OnFrameEnd();
            GlobalVarsMgr.OnFrameEnd();
        }
        public void RunEngine()
        {
            using (new HL_DevTime(1))
            {
                while (!DestructEngine)
                {
                    FrameStart();
                    FrameEnd();
                }
            }
        }
    }
}
