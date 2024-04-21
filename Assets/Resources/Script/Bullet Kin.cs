using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletKin : Enemy
{

    [SerializeField]
    GameObject handObject;

    [SerializeField]
    GameObject gunObject;

    


    protected override void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {
            Shoot();
        }


        base.Update();
    }





    private void Shoot()
    {
        gunObject.GetComponent<Animator>().SetTrigger("Attack");

    }

}
