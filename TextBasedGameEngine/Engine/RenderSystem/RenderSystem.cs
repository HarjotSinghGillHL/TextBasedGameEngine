using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Client;

namespace TextBasedGameEngine.Engine.RenderSystem
{
    public class HL_RenderSystem
    {
        HL_HudOverlay HudOverlay = null;
        HL_Engine OwningEngine = null;
        HL_Client OwningClient = null;
        public void InitializeRenderSystem(HL_Engine _OwningEngine, HL_Client _OwningClient)
        {
            HudOverlay = new HL_HudOverlay();
            OwningEngine = _OwningEngine;
            OwningClient = _OwningClient;
        }
    }
}
