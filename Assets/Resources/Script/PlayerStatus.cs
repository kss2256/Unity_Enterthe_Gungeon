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


    private bool mbAttack = false;
    private bool mbReload = false;

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
    public bool attack
    {
        get { return mbAttack; }
        set { mbAttack = value; }
    }
    public bool reload
    {
        get { return mbReload; }
        set { mbReload = value; }
    }

}
