using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum CUBETYPE
    {
        NONE,
        EDIBLE,
        TRAP,
        BEDROCK,
        TNT
    }
    
    public enum PlayerState
    {
        Aiming,
        Shooting,
        Dashing,
        WallJumping,
        Moving,
        Falling,
        KNOCKBACKED,
        STUNNED,
    }

    public enum SwitchSizeSkin
    {
        Little,
        Medium,
        Big
    }  
    public enum LEVEL_STATE
    {
        NONE,
        INITIALISING,
        LOADING,
        LOADED
    }
    public enum GAME_STATE
    {
        NONE,
        MENU,
        LOADING,
        PLAYING,
        END
    }
}
