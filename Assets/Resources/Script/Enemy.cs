using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    
    private AStart aStart;
    private bool bIsMove = false;

    public float moveSpeed = 5f; // 이동 속도
    public List<Vector2> moveVec; // 이동 경로

    private int currentWaypointIndex = 0;

    protected Animator mAnimator;


    protected virtual void Awake()
    {
        mAnimator = GetComponent<Animator>();
        aStart = gameObject.AddComponent<AStart>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindPlayer();
            bIsMove = true;
        }

       
        

    }

    protected virtual void FixedUpdate()
    {
        if(bIsMove)
        {
            MoveAStart();
        }
     
      
           
                   

    }

    private void MoveAStart()
    {

        if (moveVec == null || moveVec.Count == 0)
            return;

        Vector2 targetPosition = moveVec[currentWaypointIndex];

        // 현재 위치에서 목표 위치로의 방향 벡터 계산
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        // 캐릭터를 목표 위치로 이동
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 캐릭터가 목표 위치에 도착했는지 확인
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 다음 경로 지점으로 이동
            currentWaypointIndex++;

            // 모든 경로 지점을 다 돌았는지 확인
            if (currentWaypointIndex >= moveVec.Count)
            {
                // 이동 완료
                moveVec.Clear();
                bIsMove = false;
                currentWaypointIndex = 0;
            }
        }

    }

    private void FindPlayer()
    {
        GetComponent<AStart>().moveVec.Clear();
        moveVec.Clear();
        bIsMove = false;
        currentWaypointIndex = 0;

        GetComponent<AStart>().PathFind(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        moveVec = GetComponent<AStart>().moveVec;
    }

}
