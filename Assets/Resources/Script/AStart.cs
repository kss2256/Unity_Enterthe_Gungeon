using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;



//��ȸ�� ����ü
[System.Serializable]
public class Node
{
    public Node parentNode;
    public int x, y;
    public bool bIsWall;

    public int g, h, f;

    public Node(int x, int y, bool bIsWall)
    {
        this.x = x;
        this.y = y;
        this.bIsWall = bIsWall;
    }
}

public class AStart : MonoBehaviour
{
    public Vector2Int startPos, targetPos;
    public List<Node> finalNodeList;

    int fieldX, fieldY;
    bool moveX, moveY;      //������ ��� true�� ���ְ� fieldx,y�� ���밪�� �����
    Node[,] fieldArray;
    Node startNode, targetNode, curNode;
    List<Node> openList, closeList;


    private void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("���콺 ����");
            Vector3 targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 monpos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 pos = Camera.main.ScreenToWorldPoint(monpos);


            startPos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            targetPos = new Vector2Int(Mathf.RoundToInt(targetpos.x), Mathf.RoundToInt(targetpos.y));
            PathFind();
        }


    }


    public void PathFind()
    {
        //�ʵ� ������ = Ÿ����ġ - �����ġ �簢���� �׸��� �ε� +1�� ���������. 
        fieldX = targetPos.x - startPos.x + 1;
        fieldY = targetPos.y - startPos.y + 1;


        //�ʵ��� ũ�� 2���� �迭�� ������ x,y�������� ĭĭ���� node�� �ʱ�ȭ ����.
        fieldArray = new Node[fieldX, fieldY];

        for (int col = 0; col < fieldX; col++)
        {
            for (int row = 0; row < fieldY; row++)
            {
                bool wall = false;

                Collider2D[] colliders = Physics2D.OverlapCircleAll
                    (new Vector2(col + startPos.x, row + startPos.y), 0.4f);

                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider2D collider = colliders[i];
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        wall = true;
                }

                fieldArray[col, row] = new Node(col + startPos.x, row + startPos.y, wall);
            }
        }

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        startNode = fieldArray[startPos.x, startPos.y];
        targetNode = fieldArray[targetPos.x, targetPos.y];

        openList = new List<Node> { startNode };
        closeList = new List<Node>();
        finalNodeList = new List<Node>();


        while (openList.Count > 0)
        {
            //curNode ����
            curNode = openList[0];
            //f = g + h ���� h�� ���� �����ɷ�
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f <= curNode.f && openList[i].h < curNode.h)
                    curNode = openList[i];
            }
            openList.Remove(curNode);
            closeList.Add(curNode);


            // cur��尡 ������ ��忡 �����ߴٸ�
            if (curNode == targetNode)
            {
                //��带 �����ؼ� ���̳θ���Ʈ ��忡 �Ųٷ� �ö󰡸鼭 ä���ش�. parentNode
                Node TargetCurNode = targetNode;
                while (TargetCurNode != startNode)
                {
                    finalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.parentNode;
                }
                //���������� ���� ��带 �־��ְ� ������ ������ �ڹٲ��ش�.
                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                //Ȯ�ο�
                for (int i = 0; i < finalNodeList.Count; i++)
                {
                    Debug.Log(i + "��°�� " + finalNodeList[i].x + ", " + finalNodeList[i].y);
                    return;
                }
                
            }

            // �� �� �� ��
            OpenListAdd(curNode.x, curNode.y + 1);
            OpenListAdd(curNode.x + 1, curNode.y);
            OpenListAdd(curNode.x, curNode.y - 1);
            OpenListAdd(curNode.x - 1, curNode.y);

        }





    }



    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= startPos.x && checkX < targetPos.x + 1 && checkY >= startPos.y && checkY < targetPos.y + 1 && !fieldArray[checkX - startPos.x, checkY - startPos.y].bIsWall && !closeList.Contains(fieldArray[checkX - startPos.x, checkY - startPos.y]))
        {
           
            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = fieldArray[checkX - startPos.x, checkY - startPos.y];
            int MoveCost = curNode.g + (curNode.x - checkX == 0 || curNode.y - checkY == 0 ? 10 : 14);


            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
            if (MoveCost < NeighborNode.g || !openList.Contains(NeighborNode))
            {
                NeighborNode.g = MoveCost;
                NeighborNode.h = (Mathf.Abs(NeighborNode.x - targetNode.x) + Mathf.Abs(NeighborNode.y - targetNode.y)) * 10;
                NeighborNode.parentNode = curNode;

                openList.Add(NeighborNode);
            }
        }
    }


    void OnDrawGizmos()
    {
        if (finalNodeList.Count != 0) for (int i = 0; i < finalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(finalNodeList[i].x, finalNodeList[i].y), new Vector2(finalNodeList[i + 1].x, finalNodeList[i + 1].y));
    }

}
