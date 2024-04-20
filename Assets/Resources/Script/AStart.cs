using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;



//순회자 구조체
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
    bool moveX, moveY;      //음수일 경우 true로 해주고 fieldx,y에 절대값을 씌운다
    Node[,] fieldArray;
    Node startNode, targetNode, curNode;
    List<Node> openList, closeList;


    private void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("마우스 딸깍");
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
        //필드 사이즈 = 타겟위치 - 출발위치 사각형을 그릴것 인데 +1을 더해줘야함. 
        fieldX = targetPos.x - startPos.x + 1;
        fieldY = targetPos.y - startPos.y + 1;


        //필드의 크기 2차원 배열로 생성후 x,y방향으로 칸칸별로 node를 초기화 해줌.
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

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        startNode = fieldArray[startPos.x, startPos.y];
        targetNode = fieldArray[targetPos.x, targetPos.y];

        openList = new List<Node> { startNode };
        closeList = new List<Node>();
        finalNodeList = new List<Node>();


        while (openList.Count > 0)
        {
            //curNode 지정
            curNode = openList[0];
            //f = g + h 에서 h의 값이 작은걸로
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f <= curNode.f && openList[i].h < curNode.h)
                    curNode = openList[i];
            }
            openList.Remove(curNode);
            closeList.Add(curNode);


            // cur노드가 마지막 노드에 도달했다면
            if (curNode == targetNode)
            {
                //노드를 생성해서 파이널리스트 노드에 거꾸로 올라가면서 채워준다. parentNode
                Node TargetCurNode = targetNode;
                while (TargetCurNode != startNode)
                {
                    finalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.parentNode;
                }
                //마지막으로 시작 노드를 넣어주고 데이터 순서를 뒤바꿔준다.
                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                //확인용
                for (int i = 0; i < finalNodeList.Count; i++)
                {
                    Debug.Log(i + "번째는 " + finalNodeList[i].x + ", " + finalNodeList[i].y);
                    return;
                }
                
            }

            // ↑ → ↓ ←
            OpenListAdd(curNode.x, curNode.y + 1);
            OpenListAdd(curNode.x + 1, curNode.y);
            OpenListAdd(curNode.x, curNode.y - 1);
            OpenListAdd(curNode.x - 1, curNode.y);

        }





    }



    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= startPos.x && checkX < targetPos.x + 1 && checkY >= startPos.y && checkY < targetPos.y + 1 && !fieldArray[checkX - startPos.x, checkY - startPos.y].bIsWall && !closeList.Contains(fieldArray[checkX - startPos.x, checkY - startPos.y]))
        {
           
            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = fieldArray[checkX - startPos.x, checkY - startPos.y];
            int MoveCost = curNode.g + (curNode.x - checkX == 0 || curNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
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
