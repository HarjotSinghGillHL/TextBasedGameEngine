using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine;
using TextBasedGameEngine.Game;

namespace TextBasedGameEngine.Client
{
    public class HL_GameClientManager
    {
        public static void RunGameClientOnEngineInitialize(HL_Engine Engine, HL_Client Client, Action<HL_Engine, HL_Client> OnEngineInitialize)
        {
            OnEngineInitialize(Engine, Client);
        }

        public static void RunGameClientOnFrameStart(HL_Engine Engine, HL_Client Client, Action<HL_Engine, HL_Client> OnFrameStart)
        {
            OnFrameStart(Engine, Client);
        }

        public static void RunGameClientOnTick(HL_Engine Engine , HL_Client Client, Action<HL_Engine, HL_Client> OnTick)
        {
            OnTick(Engine, Client);
        }

        public static void RunGameClientOnFrameEnd(HL_Engine Engine, HL_Client Client, Action<HL_Engine, HL_Client> OnFrameEnd)
        {
            OnFrameEnd(Engine, Client);
        }
    }
    public class HL_Client
    {
        public void OnFrameStart()
        {
            
        }

        public void OnTick()
        {

        }
        public void OnFrameEnd()
        {

        }
    }
}
