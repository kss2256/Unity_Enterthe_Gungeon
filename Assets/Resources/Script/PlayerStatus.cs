using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum StateType
    {
        None,
        Idle,
        Walking,
        Roll,
    }


    [SerializeField] private StateType mState = StateType.Idle;


    public StateType state
    {
        get { return mState; }
        set { mState = value; }
    }

}
