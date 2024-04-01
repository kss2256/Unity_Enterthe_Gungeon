using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hand : MonoBehaviour
{


    
    private Vector3 mMouseDirPos;
    private const int mDirtransform = -1;
    private bool mbIsRightDir = true;


    public bool mouseIsRight
    {
        get { return mbIsRightDir; }
    }

    private void Awake()
    {

        

    }


    private void Update()
    {

        

       



    }

    private void FixedUpdate()
    {

        playerDirCheak();      


        if (mbIsRightDir)
        {
            if (transform.localPosition.x < 0)
            {
                Vector2 pos = transform.localPosition;
                pos.x *= mDirtransform;
                transform.localPosition = pos;
            }
        }
        else
        {
            if (transform.localPosition.x > 0)
            {
                Vector2 pos = transform.localPosition;
                pos.x *= mDirtransform;
                transform.localPosition = pos;
            }
        }
    }




    private void playerDirCheak()
    {
        mMouseDirPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform playerTr = transform.GetComponentInParent<Player>().transform;
        mMouseDirPos = mMouseDirPos - playerTr.position;

        mbIsRightDir = mMouseDirPos.x > 0;      

    }


}
