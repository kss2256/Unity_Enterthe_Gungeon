using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{


    protected EnemyMove enemyMove;
    protected Animator mAnimator;


    protected virtual void Awake()
    {
        mAnimator = GetComponent<Animator>();
        enemyMove = gameObject.AddComponent<EnemyMove>();
        StartCoroutine(FindPlayer());

    }
    
    protected virtual void Update()
    {
    
        
        
    }


    IEnumerator FindPlayer()
    {

        enemyMove.FindPlayer(Engine.mInstant.player.transform.position);
        yield return new WaitForSeconds(2);


        StartCoroutine(FindPlayer());
        //enemyMove.FindPlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

}
