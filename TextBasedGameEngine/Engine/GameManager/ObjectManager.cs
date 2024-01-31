using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Tools;

namespace TextBasedGameEngine.Engine.GameManager
{
    public static class HL_EntityManager
    {
        public static List<HL_MapEntity> EnemyPlayers;
        public static List<Vector2> CoinsConsumed;
        public static bool RemoveEntityFromList(ref List<HL_MapEntity> EntityList, Vector2 vecPosition)
        {
            int iIndex = -1;

            for (int iCurrentIndex = 0; iCurrentIndex < EntityList.Count; iCurrentIndex++)
            {
                if (EntityList[iCurrentIndex].vecPositionOnMap == vecPosition)
                {
                    iIndex = iCurrentIndex;
                    break;
                }
            }

            if (iIndex != -1)
            {
                EntityList.RemoveAt(iIndex);
                return true;
            }

            return false;
        }
        public static List<HL_MapEntity> GenerateMapEntityList(List<string> Map)
        {
            List<HL_MapEntity> EntityList = new List<HL_MapEntity>();

            for (int iCurrentListIndex = 0; iCurrentListIndex < Map.Count; iCurrentListIndex++)
            {
                for (int iCurrentStringIndex = 0; iCurrentStringIndex < Map[iCurrentListIndex].Count(); iCurrentStringIndex++)
                {
                    if (Map[iCurrentListIndex][iCurrentStringIndex] == 'E')
                    {
                        EntityList.Add(new HL_MapEntity(new HL_BaseEntity("Enemy"), new Vector2(iCurrentStringIndex, iCurrentListIndex))); ;
                    }
                }
            }

            return EntityList;
        }
        public static Vector2 FindLocalPlayerInMap(List<string> Map)
        {
            for (int iCurrentListIndex = 0; iCurrentListIndex < Map.Count; iCurrentListIndex++)
            {
                for (int iCurrentStringIndex = 0; iCurrentStringIndex < Map[iCurrentListIndex].Count(); iCurrentStringIndex++)
                {
                    if (Map[iCurrentListIndex][iCurrentStringIndex] == HL_MapManager.LocalPlayerChar)
                    {
                        return new Vector2(iCurrentStringIndex, iCurrentListIndex);
                    }
                }
            }

            return new Vector2();
        }
    }
}
