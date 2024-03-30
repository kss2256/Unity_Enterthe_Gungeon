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

    public enum WeaponType
    {
        None,
        One_Handed,
        Two_Handed,
    }

    [SerializeField] private StateType mState = StateType.Idle;
    [SerializeField] private WeaponType mWeapon = WeaponType.None;


    public StateType state
    {
        get { return mState; }
        set { mState = value; }
    }
    public WeaponType weapon
    {
        get { return mWeapon; }
        set { mWeapon = value; }
    }

}
