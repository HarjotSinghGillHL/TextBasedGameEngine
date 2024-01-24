using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TextBasedGameEngine.Client;
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

            Client = new HL_Client();
            Console.Write(InitInfo.DpiScaleFactor);
            if (InitInfo.ScreenSize.x > 0 && InitInfo.ScreenSize.y > 0)
            {
             //   Console.SetWindowSize((int)Console.LargestWindowWidth, (int)Console.LargestWindowHeight);
                
            } 
            HL_System.SetConsoleWindowToFullScreen();
            Load("Assets/Menu/Menu.txt");
        }

        public void FrameStart()
        {
            GlobalVarsMgr.OnFrameStart();
            Client.OnFrameStart();
            RenderInstance.OnFrameStart();
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
            RenderInstance.OnFrameEnd();
            GlobalVarsMgr.OnFrameEnd();
        }
        public void Load(string MapName)
        {

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
