using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.RenderSystem;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_GameManager
    {
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;

        }
    }
}
