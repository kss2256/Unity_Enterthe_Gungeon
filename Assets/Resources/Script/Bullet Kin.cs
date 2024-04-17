using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletKin : Enemy
{

    [SerializeField]
    GameObject mHandObject;

    [SerializeField]
    GameObject mGunObject;



    protected override void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {
            Shoot();
        }

    }


    


    private void Shoot()
    {
        mGunObject.GetComponent<Animator>().SetTrigger("Attack");

    }

}
