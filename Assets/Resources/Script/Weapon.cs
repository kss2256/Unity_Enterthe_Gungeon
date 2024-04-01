using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Part;

public class Weapon : Part
{

    private const float mReverseLeft = -180;
    private const float mReverseRight = 0;
    
    
    private float mCurrentDir = 0;


    protected override void FixedUpdate()
    {

        base.FixedUpdate();
    
       

     


        mouseToTargetRot();

    }



    private void mouseToTargetRot()
    {
        if(mouseIsRight)
        {
            mCurrentDir = mReverseRight;
        }
        else
        {
            mCurrentDir = mReverseLeft;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angle + mCurrentDir, Vector3.forward);
    }


}
