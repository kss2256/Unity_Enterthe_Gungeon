using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    
    private AStart aStart;
    private bool bIsMove = false;

    public float moveSpeed = 5f; // �̵� �ӵ�
    public List<Vector2> moveVec; // �̵� ���

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

        // ���� ��ġ���� ��ǥ ��ġ���� ���� ���� ���
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        // ĳ���͸� ��ǥ ��ġ�� �̵�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // ĳ���Ͱ� ��ǥ ��ġ�� �����ߴ��� Ȯ��
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ���� ��� �������� �̵�
            currentWaypointIndex++;

            // ��� ��� ������ �� ���Ҵ��� Ȯ��
            if (currentWaypointIndex >= moveVec.Count)
            {
                // �̵� �Ϸ�
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
