using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CMove_Type
{
    public bool bIsMove = false;

    public int curIndex = 0;
    public float moveSpeed = 3f;
    public float naviRange = 6f;
    public List<Vector2> moveVec = new List<Vector2>(); // �̵� ��� ����
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

    private void Update()
    {
        Vector3 playerpos = Engine.mInstant.player.transform.position;
        Vector3 enemyPos = transform.position;
        checkRange(playerpos, enemyPos, cMoveType.naviRange);



    }

    void checkRange(Vector2 playerPos, Vector2 monsterPos, float radius)
    {
        // �÷��̾�� ���� ������ �Ÿ� ���� ���
        float distanceSquared = (playerPos.x - monsterPos.x) * (playerPos.x - monsterPos.x)
                              + (playerPos.y - monsterPos.y) * (playerPos.y - monsterPos.y);

        // �Ÿ� ������ ������ ���� �̳��� �ִ��� Ȯ��
        if (distanceSquared <= radius * radius)
        {
            //������ ���� ��
            Debug.Log("���� �ȿ� ����");
        }
        else
        {
            //������ ���� ��
        }
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

        // ���� ��ġ���� ��ǥ ��ġ���� ���� ���� ���
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        // ĳ���͸� ��ǥ ��ġ�� �̵�
        transform.Translate(moveDirection * cMoveType.moveSpeed * Time.deltaTime);

        // ĳ���Ͱ� ��ǥ ��ġ�� �����ߴ��� Ȯ��
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ���� ��� �������� �̵�
            cMoveType.curIndex++;

            // ��� ��� ������ �� ���Ҵ��� Ȯ��
            if (cMoveType.curIndex >= cMoveType.moveVec.Count)
            {
                // �̵� �Ϸ�
                cMoveType.moveVec.Clear();
                cMoveType.bIsMove = false;
                cMoveType.curIndex = 0;
            }
        }

    }




}