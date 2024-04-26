using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;



//��ȸ�� ����ü
[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}



public class AStart : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<Node> FinalNodeList = new List<Node>();
    public bool allowDiagonal, dontCrossCorner;

    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public List<Vector2> moveVec = new List<Vector2>();


    public void PathFind(Vector2 start, Vector2 target)
    {

        TargetFindPos(start, target);


        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����
        sizeX = Mathf.Abs(topRight.x - bottomLeft.x) + 1;
        sizeY = Mathf.Abs(topRight.y - bottomLeft.y) + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = bottomLeft.x; i <= topRight.x; i++)
        {
            for (int j = bottomLeft.y; j <= topRight.y; j++)
            {
                bool isWall = false;


                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(i, j), 0.4f);

                for (int k = 0; k < colliders.Length; k++)
                {
                    Collider2D col = colliders[k];
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        isWall = true;
                        break;
                    }
                }

                NodeArray[i - bottomLeft.x, j - bottomLeft.y] = new Node(isWall, i, j);
            }
        }


        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // ��������忡 �����ϸ� ������
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count; i++) 
                {
                    //����� ��¿�
                    //Debug.Log(i + "��°�� " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                    //�̵� �Ÿ� �����
                    moveVec.Add(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y));
                }
                return;
            }

            // �֢آע�
            OpenListAdd(CurNode.x + 1, CurNode.y + 1);
            OpenListAdd(CurNode.x - 1, CurNode.y + 1);
            OpenListAdd(CurNode.x - 1, CurNode.y - 1);
            OpenListAdd(CurNode.x + 1, CurNode.y - 1);

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {

            // �밢�� �̵���, �� ���̷� ��� �ȵ�
            if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // �ڳʸ� �������� ���� ������, �̵� �߿� �������� ��ֹ��� ������ �ȵ�
            if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    void TargetFindPos(Vector2 start, Vector2 target)
    {
        
        startPos = new Vector2Int(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y));
        targetPos = new Vector2Int(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.y));

        bottomLeft.x = Mathf.Min(startPos.x, targetPos.x);
        bottomLeft.y = Mathf.Min(startPos.y, targetPos.y);

        bottomLeft.x -= 15;
        bottomLeft.y -= 15;

        topRight.x = Mathf.Max(startPos.x, targetPos.x);
        topRight.y = Mathf.Max(startPos.y, targetPos.y);

        topRight.x += 15;
        topRight.y += 15;
    }


    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;

        if (FinalNodeList != null && FinalNodeList.Count != 0)
        {
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
            }
        }
    }

}
