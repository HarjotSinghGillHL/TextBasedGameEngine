using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Engine.RenderSystem
{
    public class HL_HudOverlay
    {
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }
    }
}
