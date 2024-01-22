using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct HL_BaseGlobalVars
{
    public void Construct(HL_EngineInfo _EngineInfo)
    {
        FrameTime = 0;
        CurrentTime = 0;
        TickCount = 0;
        FrameCount = 0;
        NextSleep = 0;
        EngineInfo = _EngineInfo;
        IntervalPerTick = (1000.0 / (double)EngineInfo.TickRate);
    }

    public double FrameTime;
    public double CurrentTime;
    public int FrameCount;
    public double IntervalPerTick;
    public int TickCount;
    public int NextSleep;
    public HL_EngineInfo EngineInfo;
}

namespace TextBasedGameEngine.Engine
{
    public class HL_GlobalVarsManager
    {
        public long DateTickBegin = 0;

        public HL_BaseGlobalVars GVars;
        public HL_BaseGlobalVars SecondOldVars; //1 second old
        public void InitializeGlobalVarsMgr(HL_EngineInfo InitInfo)
        {
            DateTickBegin = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; ;
            GVars.Construct(InitInfo);
        }
        public void OnFrameStart()
        {
            GVars.CurrentTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - DateTickBegin;
        }
        public void OnFrameEnd()
        {
            GVars.FrameTime = ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - DateTickBegin) - GVars.CurrentTime;
            Console.WriteLine(GVars.FrameTime);

            if (GVars.CurrentTime - SecondOldVars.CurrentTime > 1000.0)
            {
                //determine sleep here using frametime
                //SecondOldVars.NextSleep
                SecondOldVars.CurrentTime = GVars.CurrentTime;
            }

            GVars.FrameCount++;
        }
    }
}
