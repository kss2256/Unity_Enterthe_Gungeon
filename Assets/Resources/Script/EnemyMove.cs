using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CMove_Type
{
    public int curIndex = 0;
    public float moveSpeed = 3f;
    public List<Vector2> moveVec = new List<Vector2>(); // �̵� ��� ����
}


public class EnemyMove : MonoBehaviour
{

    private AStart aStart;
    private CMove_Type cMoveType;

    //�̵� �������� Image ȸ��
    private SpriteRenderer  spriteRenderer;
    private bool bflip;

    private void Awake()
    {
        aStart = gameObject.AddComponent<AStart>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        cMoveType = new CMove_Type();
    }



    private void FixedUpdate()
    {
        MoveAStar();
    }

    private void LateUpdate()
    {
        spriteRenderer.flipX = bflip;
    }

    public void FindPlayer(Vector3 target)
    {
        GetComponent<AStart>().moveVec.Clear();
        cMoveType.moveVec.Clear();
        cMoveType.curIndex = 0;

        GetComponent<AStart>().PathFind(transform.position, target);

        cMoveType.moveVec = GetComponent<AStart>().moveVec;
    }

    public void MoveStop()
    {
        cMoveType.moveVec.Clear();
        cMoveType.curIndex = 0;
    }


    private void MoveAStar()
    {

        if (cMoveType.moveVec == null || cMoveType.moveVec.Count == 0)
            return;

        Vector2 targetPosition = cMoveType.moveVec[cMoveType.curIndex];

        // ���� ��ġ���� ��ǥ ��ġ���� ���� ���� ���
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        bflip = moveDirection.x < 0;

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
                cMoveType.curIndex = 0;
            }
        }

    }




}
