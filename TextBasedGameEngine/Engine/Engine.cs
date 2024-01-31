using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngine.Game;
using TextBasedGameEngine.Tools;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using TextBasedGameEngine.Engine.GameManager;

public struct HL_EngineInfo
{
    public double TickRate;
    public double MaxFrameRate;
    public Vector2 ScreenSize;
    public float DpiScaleFactor;
    public Action<HL_Engine> OnEngineInitialize;
    public Action<HL_Engine> OnFrameStart;
    public Action<HL_Engine> OnTick;
    public Action<HL_Engine> OnFrameEnd;
}

namespace TextBasedGameEngine.Engine
{
    public class HL_Engine
    {
        public HL_GlobalVarsManager GlobalVarsMgr = null;
        public HL_GameManager GameManager = null;
        public HL_RenderSystem RenderInstance = null;
        public HL_UserInterface UserInterface = null;

        public bool DestructEngine = false;
        double LastUpdateTime = 0;
        public void InitializeEngine(ref HL_EngineInfo InitInfo)
        {

            GlobalVarsMgr = new HL_GlobalVarsManager();
            GameManager = new HL_GameManager();
            RenderInstance = new HL_RenderSystem();
            UserInterface = new HL_UserInterface();

            GlobalVarsMgr.Initialize(InitInfo);
            GameManager.Initialize(this);
            RenderInstance.Initialize(this);
            UserInterface.Initialize(this);

            HL_System.SetConsoleWindowToFullScreen();
            GlobalVarsMgr.GVars.EngineInfo.OnEngineInitialize(this);
        }

        public void FrameStart()
        {
            GlobalVarsMgr.OnFrameStart();
            RenderInstance.OnFrameStart();
            GlobalVarsMgr.GVars.EngineInfo.OnFrameStart(this);

            if ((GlobalVarsMgr.GVars.CurrentTime - LastUpdateTime) >= GlobalVarsMgr.GVars.IntervalPerTick)
            {
                OnTick();
                LastUpdateTime = GlobalVarsMgr.GVars.CurrentTime;
            }

        }

        public void OnTick()
        {
            GlobalVarsMgr.GVars.EngineInfo.OnTick(this);
            GlobalVarsMgr.OnTick();
        }
        public void FrameEnd()
        {
            GlobalVarsMgr.GVars.EngineInfo.OnFrameEnd(this);
            RenderInstance.OnFrameEnd();
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
