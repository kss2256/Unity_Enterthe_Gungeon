using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class Engine : MonoBehaviour
{
    static public Engine mInstant;
    
    [SerializeField] Player mPlayer;
    [SerializeField] SceneMgr mSceneMgr;


    public Player player { get { return mPlayer; } }
    public SceneMgr SceneMgr { get { return mSceneMgr; } }

    private void Awake()
    {
        mInstant = this;
        DontDestroyOnLoad(this);
    }

}
