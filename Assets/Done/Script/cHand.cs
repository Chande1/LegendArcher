using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandState
{
    Non,
    TakeArrow,
    TakeString,
}


public class cHand : MonoBehaviour
{
    public HandState hstate;

    cHand()
    {
        hstate = HandState.Non;
    }
}


