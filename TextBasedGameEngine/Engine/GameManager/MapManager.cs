using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_MapManager
    {
        HL_Engine OwningEngine = null;
        public void Initialize(HL_Engine _OwningEngine)
        {
            OwningEngine = _OwningEngine;
        }
        public void Load()
        {

        }
    }
}
