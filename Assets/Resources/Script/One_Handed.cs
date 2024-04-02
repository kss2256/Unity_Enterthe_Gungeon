using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Part;

public class One_Handed : Part
{

    private const float mReverseLeft = -180;
    private const float mReverseRight = 0;
        
    private float mCurrentDir = 0;
    private float mReloadTime = 1.5f;
    private bool mbIsReload = false;

    private Animator mAnimator;


    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }


    protected override void Update()
    {
        base.Update();

        if(Engine.mInstant.player.GetComponent<PlayerStatus>().attack)
        {
            mAnimator.SetBool("Shoot", true);
        }

        if (Engine.mInstant.player.GetComponent<PlayerStatus>().reload)
        {
            if(!mbIsReload)
            StartCoroutine(Reload());            
        }
     

    }


    protected override void FixedUpdate()
    {

        base.FixedUpdate();


             


    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        mouseToTargetRot();


    }



    private void mouseToTargetRot()
    {
        if(mouseIsRight)
            mCurrentDir = mReverseRight;
        else
            mCurrentDir = mReverseLeft;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        
        if (mouseIsRight)
        {
            if (angle < -90 || angle > 90)
            {
                if (angle < -90)
                    angle = -90;
                else if (angle > 90)
                    angle = 90;
            }
        }
        else
        {
            if(angle < 0)
                angle += 360;

            if (angle < 90 || angle > 270)
            {
                if (angle < 90)
                    angle = 90;
                else if (angle > 270)
                    angle = 270;
            }
        }

        transform.localRotation = Quaternion.AngleAxis(angle + mCurrentDir, Vector3.forward);
             
    }

    public void ShootEnd()
    {
        Engine.mInstant.player.GetComponent<PlayerStatus>().attack = false;
        mAnimator.SetBool("Shoot", false);
    }

    public IEnumerator Reload()
    {
        mAnimator.SetBool("Reload", true);
        mbIsReload = true;

        yield return new WaitForSeconds(mReloadTime);

        mAnimator.SetBool("Reload", false);
        Engine.mInstant.player.GetComponent<PlayerStatus>().reload = false;
        mbIsReload = false;

    }


}
