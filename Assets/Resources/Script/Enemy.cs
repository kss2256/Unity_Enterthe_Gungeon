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

    }
    
    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�÷��̾� ��ġ�� ���� �Ұ���
            enemyMove.FindPlayer(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

       
        

    }




}
