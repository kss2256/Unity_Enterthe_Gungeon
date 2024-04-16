using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    protected Animator mAnimator;

    protected virtual void Awake()
    {
        mAnimator = GetComponent<Animator>();

    }

    protected virtual void Update()
    {

        

    }




}
