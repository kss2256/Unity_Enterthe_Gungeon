using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Part;

public class Weapon : Part
{

    private const float mReverseLeft = -180;
    private const float mReverseRight = 0;
    
    
    private float mCurrentDir = 0;

    protected override void Update()
    {
        base.Update();



        
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


}
