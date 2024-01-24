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
        public HL_MapManager MapManager = null;
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            MapManager = new HL_MapManager();
            OwningEngine = _OwningEngine;
            MapManager.Initialize(_OwningEngine);

        }
    }
}
