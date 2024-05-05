using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 mDirection;
    private float mSpeed = 20.0f;

    public float speed
    {
        get { return mSpeed; }
        set { mSpeed = value; }
    }



    private void LateUpdate()
    {

        transform.Translate(mDirection * mSpeed * Time.deltaTime);
       
    }


    public void Shoot(Vector3 _dir)
    {
        mDirection = _dir;        

    }

}
