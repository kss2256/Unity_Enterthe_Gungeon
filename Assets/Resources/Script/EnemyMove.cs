using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CMove_Type
{
    public bool bIsMove = false;

    public int curIndex = 0;
    public float moveSpeed = 5f; 
    public List<Vector2> moveVec = new List<Vector2>(); // 이동 경로 저장
}


public class EnemyMove : MonoBehaviour
{

    private AStart aStart;
    private CMove_Type cMoveType;

    private void Awake()
    {
        aStart = gameObject.AddComponent<AStart>();
        cMoveType = new CMove_Type();
    }

    private void FixedUpdate()
    {
        MoveAStar();
    }



    public void FindPlayer(Vector3 target)
    {
        GetComponent<AStart>().moveVec.Clear();
        cMoveType.moveVec.Clear();
        cMoveType.bIsMove = true;
        cMoveType.curIndex = 0;

        GetComponent<AStart>().PathFind(transform.position, target);

        cMoveType.moveVec = GetComponent<AStart>().moveVec;
    }


    private void MoveAStar()
    {

        if (cMoveType.moveVec == null || cMoveType.moveVec.Count == 0)
            return;

        Vector2 targetPosition = cMoveType.moveVec[cMoveType.curIndex];

        // 현재 위치에서 목표 위치로의 방향 벡터 계산
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        // 캐릭터를 목표 위치로 이동
        transform.Translate(moveDirection * cMoveType.moveSpeed * Time.deltaTime);

        // 캐릭터가 목표 위치에 도착했는지 확인
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 다음 경로 지점으로 이동
            cMoveType.curIndex++;

            // 모든 경로 지점을 다 돌았는지 확인
            if (cMoveType.curIndex >= cMoveType.moveVec.Count)
            {
                // 이동 완료
                cMoveType.moveVec.Clear();
                cMoveType.bIsMove = false;
                cMoveType.curIndex = 0;
            }
        }

    }




}
