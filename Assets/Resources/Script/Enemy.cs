using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    private EnemyMove enemyMove;    public EnemyMove EnemyMove { get { return enemyMove; } }
    private Animator animator;     public Animator Animator { get { return animator; } }

    private float moveRadius = 10.0f;
    private float attackRadius = 15.0f;
    private bool bIsMove = false;   public bool IsMove { get { return bIsMove; } }
    private bool bIsAttack = false; public bool IsAttack { get { return bIsAttack; } }
    public float MoveRadius 
    {
        get { return moveRadius; } set { moveRadius = value; }
    }
    public float AttackRadius 
    {
        get { return attackRadius; } set { attackRadius = value; }
    }



    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        enemyMove = gameObject.AddComponent<EnemyMove>();
        StartCoroutine(FindPlayer());

    }
    
    protected virtual void Update()
    {
        checkRange();



    }


    IEnumerator FindPlayer()
    {
        if (bIsMove)
            enemyMove.FindPlayer(Engine.mInstant.player.transform.position);
        else
            enemyMove.MoveStop();

        yield return new WaitForSeconds(1);
        


        StartCoroutine(FindPlayer());
    }

    void checkRange()
    {
        Vector3 playerpos = Engine.mInstant.player.transform.position;
        Vector3 enemyPos = transform.position;

        //유클리드 거리, 제곱근x안씌운 값
        float distanceSquared = (playerpos.x - enemyPos.x) * (playerpos.x - enemyPos.x)
                              + (playerpos.y - enemyPos.y) * (playerpos.y - enemyPos.y);


        //이동 반경인지 확인
        if (distanceSquared <= moveRadius * moveRadius)
        {
            bIsMove = false;
        }
        else
        {
            bIsMove = true;
        }

        //공격 거리 확인
        if (distanceSquared <= attackRadius * attackRadius)
        {
            Debug.Log("Attack");
            bIsAttack = true;
        }
        else
        {
            bIsAttack = false;
        }

    }

}
