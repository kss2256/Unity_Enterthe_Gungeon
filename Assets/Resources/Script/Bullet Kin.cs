using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletKin : Enemy
{

    [SerializeField]
    private GameObject handObject;

    [SerializeField]
    private GameObject gunObject;

    private Shoot shoot;

    private void Start()
    {
        shoot = GetComponent<Shoot>();


    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.N))
        {
            shoot.BulletFire(Engine.mInstant.player.transform.position, transform.position);
        }


        if(IsAttack)
        {

        }
        
    }





    private void ShootAnimaiton()
    {
        gunObject.GetComponent<Animator>().SetTrigger("Attack");
    }

}
