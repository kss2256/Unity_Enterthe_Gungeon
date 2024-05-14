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
    private const float attackTime = 3.0f;
    private float attackLimtiTime = attackTime;


    private void Start()
    {
        shoot = GetComponent<Shoot>();


    }

    protected override void Update()
    {
        base.Update();

      
        if(IsAttack)
        {
            attackLimtiTime += Time.deltaTime;
            if(attackLimtiTime >= attackTime)
            {
                shoot.BulletFire(Engine.mInstant.player.transform.position, transform.position, 10.0f);


                attackLimtiTime = 0.0f;
            }
        }
        else
        {
            attackLimtiTime = 0.0f;
        }

        
      

    }





    private void ShootAnimaiton()
    {
        gunObject.GetComponent<Animator>().SetTrigger("Attack");
    }

}
