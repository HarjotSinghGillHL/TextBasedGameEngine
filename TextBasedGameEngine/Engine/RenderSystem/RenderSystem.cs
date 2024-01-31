using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Engine.RenderSystem
{
    public class HL_RenderSystem
    {
        HL_HudOverlay HudOverlay = null;
        HL_UserInterface UserInterface = null;
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            HudOverlay = new HL_HudOverlay();
            OwningEngine = _OwningEngine;
            HudOverlay.Initialize(_OwningEngine);
        }
        public void OnFrameStart()
        {

        }

        public void OnFrameEnd()
        {

        }
    }
}
