using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    public enum PartList
    {
        Hand,
        Weapon,
    }

    private Vector3 mMouseDirPos;
    private const int mDirtransform = -1;
    private bool mbIsRightDir = true;

    private SpriteRenderer mSpriteRenderer;
    private Transform mHandTr;
    private Transform mWeaponTr;

    public bool mouseIsRight
    {
        get { return mbIsRightDir; }
    }


    protected virtual void Start()
    {
        //mHandTr = transform.GetChild((int)PartList.Hand);
        //mWeaponTr = transform.GetChild((int)PartList.Weapon);
        mSpriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected virtual void Update()
    {
        //Player Left Right Cheak - mbIsRightDir;
        playerDirCheak();





    }

    protected virtual void FixedUpdate()
    {

        turnabout();
      


    }

    private void LateUpdate()
    {
        if(mSpriteRenderer)
        mSpriteRenderer.flipX = !mbIsRightDir;
    }

    private void playerDirCheak()
    {
       mMouseDirPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       Transform playerTr = transform.GetComponentInParent<Player>().transform;
       mMouseDirPos = mMouseDirPos - playerTr.position;

        mbIsRightDir = mMouseDirPos.x > 0;

    }

    private void turnabout()
    {

        if (mouseIsRight)
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


}
