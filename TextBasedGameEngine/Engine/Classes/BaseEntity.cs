using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.GameManager;
using TextBasedGameEngine.Tools;

public enum ELifeState
{
    STATE_NULL,
    STATE_ALIVE,
    STATE_DEAD,
    STATE_SPAWNING,
    STATE_DYING,
    STATE_SPECTATOR,
    STATE_MAX,
}
public enum EEntityClass
{
    ENTITY_PLAYER,
    ENTITY_WEAPON,
    ENTITY_PROP,
    ENTITY_NPC,
    ENTITY_MAX
}

namespace TextBasedGameEngine.Engine.Classes
{
    public class HL_BaseEntityHeader
    {
        public EEntityClass eEntityClass;
        public int iEntityIndex = 0;

    };

    public class HL_BaseEntity : HL_BaseEntityHeader
    {
        public HL_BaseEntity(string _szSanitizedName)
        {
            szSanitizedName = _szSanitizedName;
            eEntityClass = EEntityClass.ENTITY_PLAYER;
        }
        ~HL_BaseEntity()
        {

        }
        public void Spawn()
        {
            iHealth = iMaxHealth;
            iShield = iMaxShield;
            eLifeState = ELifeState.STATE_ALIVE;
        }
        public void KillPlayer()
        {
            eLifeState = ELifeState.STATE_DEAD;
        }
        public bool IsAlive()
        {
            return eLifeState == ELifeState.STATE_ALIVE;
        }
        public int GetHealth()
        {
            return iHealth;
        }

        public void SetHealth(int iNewHealth)
        {
            iHealth = iNewHealth;
        }
        public int GetMaxHealth()
        {
            return iMaxHealth;
        }
        public int GetMinHealth()
        {
            return iMinHealth;
        }

        public int GetShield()
        {
            return iShield;
        }
        public void SetShield(int iNewShield)
        {
            iShield = iNewShield;
        }
        public int GetMaxShield()
        {
            return iMaxShield;
        }
        public int GetMinShield()
        {
            return iMinShield;
        }

        internal void SetHealth(object value)
        {
            throw new NotImplementedException();
        }

        const int iMaxHealth = 100;
        const int iMinHealth = 0;
        int iHealth = iMaxHealth;
        const int iMaxShield = 100;
        const int iMinShield = 0;
        int iShield = iMaxHealth;
        ELifeState eLifeState = ELifeState.STATE_ALIVE;
        public string szSanitizedName;
    }

    public class HL_MapEntity
    {
        public HL_MapEntity(HL_BaseEntity _Entity, Vector2 _vecPositionOnMap)
        {
            Entity = _Entity;
            vecPositionOnMap = _vecPositionOnMap;
            iLastTickCount = 0;
            Random random = new Random();

            iPlayerMoveTickWait = random.Next(10, 30);
            iLastPlayerMoveTick = 0;
        }
        public bool ShouldMove()
        {
            if (iLastPlayerMoveTick == 0)
            {
                iLastPlayerMoveTick = iPlayerMoveTickWait;
                return true;
            }

            if (iLastTickCount != Environment.TickCount)
            {
                --iLastPlayerMoveTick;
                iLastTickCount = Environment.TickCount;
            }

            return false;
        }

        public void UpdatePosition(char LeftTile, char RightTile, int iLengthOfCurrentLine)
        {
            int iDelay = 5;

            if (iCurrentPosition == iDelay)
            {
                iMoveSide = 0;
            }
            else if (iCurrentPosition == 0)
                iMoveSide = 1;

            if (iMoveSide == 1)
            {
                ++iCurrentPosition;
                if (!HL_MapManager.IsBlockedRegionChar(RightTile) && !HL_MapManager.IsTireHarmful(RightTile) && vecPositionOnMap.x < iLengthOfCurrentLine - 1)
                    vecPositionOnMap.x += 1;
                else
                    iCurrentPosition = iDelay;
            }
            else
            {
                --iCurrentPosition;
                if (!HL_MapManager.IsBlockedRegionChar(LeftTile) && !HL_MapManager.IsTireHarmful(LeftTile) && vecPositionOnMap.x > 0)
                    vecPositionOnMap.x -= 1;
                else
                    iCurrentPosition = 0;
            }

        }

        public HL_BaseEntity Entity;
        public Vector2 vecPositionOnMap;

        private int iCurrentPosition = 0;
        private bool bMovePositionChanged = false;
        private int iMoveSide = 1;
        private int iLastTickCount;
        private int iLastPlayerMoveTick;
        private int iPlayerMoveTickWait;
    }

    public struct HL_PlayerHurt
    {
        public HL_PlayerHurt(int _iPlayerHurtTickWait)
        {
            iLastTickCount = 0;
            iLastPlayerHurtTick = 0;
            iPlayerHurtTickWait = _iPlayerHurtTickWait;
        }

        public bool ShouldHurtPlayer()
        {
            if (iLastPlayerHurtTick == 0)
            {
                iLastPlayerHurtTick = iPlayerHurtTickWait;
                return true;
            }

            if (Environment.TickCount != iLastTickCount)
            {
                --iLastPlayerHurtTick;
                iLastTickCount = Environment.TickCount;
            }

            return false;
        }

        private int iLastTickCount;
        public int iLastPlayerHurtTick;
        public int iPlayerHurtTickWait;
    }

}
