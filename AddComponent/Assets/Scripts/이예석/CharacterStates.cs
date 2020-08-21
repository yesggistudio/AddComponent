using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates 
{

    public enum CharacterConditions
    {
        Normal,
        Paused,
        Dead
    }

    public enum MovementStates
    {
        Normal =0,
        Climb = 10,
        Dead =50,
        Walking,
        Running,
        Jumping,
        Pushing
    }

}
