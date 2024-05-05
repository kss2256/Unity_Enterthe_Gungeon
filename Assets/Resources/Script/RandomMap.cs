using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;

public class Cgrid
{
    public int x, y;
    public bool bIsWall;

    
    public Cgrid(int x, int y, bool bIsWall)
    {
        this.x = x;
        this.y = y;
        this.bIsWall = bIsWall;
    }

}






public class RandomMap : MonoBehaviour
{

    [SerializeField] private int mapWidth, mapHeight;       //맵의 크기
    [SerializeField] private int wallRatio;                 //벽의 비율
    [SerializeField] private int roomCount;                 //랜덤맵 개수


    [SerializeField] private Tilemap gridGround;            
    [SerializeField] private Tilemap gridWall;
    [SerializeField] private Tile[] tileGround;
    [SerializeField] private Tile[] tileWall;               //0~3 까지 → ← ↓ ↑ 순으로 배열에 두기



    private List<Cgrid[,]> mapArrList = new List<Cgrid[,]>();
    private int wallCount;

    private AStart aStart;
    private Vector2Int[] centerPosVec;      //centerPosVec 0번째가 시작 count마지막 번째가 클리어
    private Vector2Int dirDistance;
    private List<Vector2> listmoveVec = new List<Vector2>();

    private void Awake()
    {
        aStart = gameObject.AddComponent<AStart>();
        aStart.bdiagonal = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CreateDunegeon();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            CellularAutomata();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadConstruction();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {           
            TileErase();
        }

        


    }

    void LoadConstruction()
    {
        Vector3Int pos = new Vector3Int();
        System.Random random = new System.Random();

        for (int Idx = 0; Idx < listmoveVec.Count; Idx++)
        {

            pos = new Vector3Int((int)listmoveVec[Idx].x, (int)listmoveVec[Idx].y, 0);
            //mapArray밖의 범위 -> 통로가 될 부분
            if(null == gridGround.GetTile(pos) && gridWall.GetTile(pos) == null)
            {
                //이전 무브벡이 x이동인지 y이동인지 확인하기

                //y이동 중
                if(listmoveVec[Idx].x == listmoveVec[Idx - 1].x)
                {
                    gridWall.SetTile(new Vector3Int(pos.x + 2, pos.y, 0), tileWall[1]);
                    gridWall.SetTile(new Vector3Int(pos.x - 2, pos.y, 0), tileWall[0]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x + 1, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x - 1, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                }
                //x이동 중
                else if (listmoveVec[Idx].y == listmoveVec[Idx - 1].y)
                {
                    gridWall.SetTile(new Vector3Int(pos.x, pos.y + 2, 0), tileWall[3]);
                    gridWall.SetTile(new Vector3Int(pos.x, pos.y - 2, 0), tileWall[2]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y + 1, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y - 1, 0), tileGround[random.Next(0, tileGround.Length)]);
                }
              
            }
            //둘중 하나
            else
            {
                if (Idx <= 0)
                    continue;


                //타일 지금있는녀석 지우고 그라운드로 교체
                gridWall.SetTile(pos, null);
                gridGround.SetTile(pos, null);
                if (listmoveVec[Idx].x == listmoveVec[Idx - 1].x)
                {
                    gridWall.SetTile(new Vector3Int(pos.x + 1, pos.y, 0), null);
                    gridWall.SetTile(new Vector3Int(pos.x - 1, pos.y, 0), null);
                    gridGround.SetTile(new Vector3Int(pos.x + 1, pos.y, 0), null);
                    gridGround.SetTile(new Vector3Int(pos.x - 1, pos.y, 0), null);

                    gridGround.SetTile(new Vector3Int(pos.x + 1, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x - 1, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                }

                else if (listmoveVec[Idx].y == listmoveVec[Idx - 1].y)
                {
                    gridWall.SetTile(new Vector3Int(pos.x, pos.y + 1, 0), null);
                    gridWall.SetTile(new Vector3Int(pos.x, pos.y - 1, 0), null);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y + 1, 0), null);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y - 1, 0), null);

                    gridGround.SetTile(new Vector3Int(pos.x, pos.y, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y + 1, 0), tileGround[random.Next(0, tileGround.Length)]);
                    gridGround.SetTile(new Vector3Int(pos.x, pos.y - 1, 0), tileGround[random.Next(0, tileGround.Length)]);
                }
            }

        }


    }

    void RoomConnect()
    {
        centerPosVec = new Vector2Int[roomCount];
        aStart.moveVec.Clear();
        dirDistance = Vector2Int.zero;
       

        System.Random random = new System.Random();
        int curDir = 0, prevDir = 0;
        

        for (int i = 0; i < roomCount; ++i)
        {
            if(i == 0)
            {
                centerPosVec[i] = new Vector2Int(dirDistance.x + (mapWidth / 2), dirDistance.y + (mapHeight / 2));
                CreateMapWall();
                continue;
            }
            bool dir = true;
            curDir = random.Next(1, 5);     //5는 세로 직각 9 는 대각선까지
            while (dir)
            {
                int cheak = 0;
                if (!IsOppositeDir(curDir, prevDir))
                {
                    // → ← ↑ ↓  ↗↖↙↘
                    switch (curDir)
                    {
                        case 1:
                            dirDistance.x += random.Next(5, 10) + mapWidth;
                            break;
                        case 2:
                            dirDistance.x -= random.Next(5, 10) + mapWidth;
                            break;
                        case 3:
                            dirDistance.y += random.Next(5, 10) + mapHeight;
                            break;
                        case 4:
                            dirDistance.y -= random.Next(5, 10) + mapHeight;
                            break;
                        //case 5:
                        //    dirDistance.x += random.Next(5, 10) + mapWidth;
                        //    dirDistance.y += random.Next(5, 10) + mapHeight;
                        //    break;
                        //case 6:
                        //    dirDistance.x -= random.Next(5, 10) + mapWidth;
                        //    dirDistance.y += random.Next(5, 10) + mapHeight;
                        //    break;
                        //case 7:
                        //    dirDistance.x -= random.Next(5, 10) + mapWidth;
                        //    dirDistance.y -= random.Next(5, 10) + mapHeight;
                        //    break;
                        //case 8:
                        //    dirDistance.x += random.Next(5, 10) + mapWidth;
                        //    dirDistance.y -= random.Next(5, 10) + mapHeight;
                        //    break;
                    }
                    for (int k = 0; k < centerPosVec.Length; k++)
                    {
                        if (centerPosVec[k].x > dirDistance.x && centerPosVec[k].x < dirDistance.x + mapWidth && centerPosVec[k].y > dirDistance.y && centerPosVec[k].y < dirDistance.y + mapHeight)
                        {
                            //중복 된거
                            ++cheak;
                        }
                    }
                    if (cheak <= 0)
                        dir = false;
                }
                if(dir)
                curDir = random.Next(1, 5);               
            }

            
            centerPosVec[i] = new Vector2Int(dirDistance.x + (mapWidth / 2), dirDistance.y + (mapHeight / 2));
            aStart.PathFind(centerPosVec[i - 1], centerPosVec[i]);
            CreateMapWall(dirDistance.x, dirDistance.y);
            prevDir = curDir;
        }

        listmoveVec.AddRange(aStart.moveVec);

    }

    bool IsOppositeDir(int curdir, int prevdir)
    {

        switch(curdir)
        {
            case 1:
                if (prevdir == 2)
                    return true;
                break;
            case 2:
                if (prevdir == 1)
                    return true;
                break;
            case 3:
                if (prevdir == 4)
                    return true;
                break;
            case 4:
                if (prevdir == 3)
                    return true;
                break;
        }
        
        return false;
    }


    //맵의 크기 맨 가장자리 벽 만들기
    void CreateMapWall(int width = 0, int height = 0)
    {
        //+1 하는 이유는 가장자리 벽
        Cgrid[,] mapArray = new Cgrid[mapWidth + 1, mapHeight + 1];

        //벽의 비율 만큼 벽의 개수 정해주기 
        int tile = mapWidth * mapHeight;


        System.Random random = new System.Random();


        for (int i = 0; i < mapWidth + 1; i++)
        {
            for (int j = 0; j < mapHeight + 1; j++)
            {
                bool wall = false;
                //맵 의 가장자리는 전부 벽으로
                if (i == 0 || i == mapWidth || j == 0 || j == mapHeight )
                {
                    wall = true;
                }
                else
                {
                    //퍼센트로 벽 생성 (랜덤)
                    if (random.Next(100) < wallRatio)
                    {
                        wall = true;
                    }
                }

                mapArray[i, j] = new Cgrid(i + width, j + height, wall);
                
            }
        }
        mapArrList.Add(mapArray);
    }

    void CellularAutomata(int count = 1)
    {
        if (mapArrList == null)
        {
            Debug.Log("MapArray Null");
            return;
        }

        //셀룰러 작업 count 횟수만큼 ---
        for (int repeat = 0; repeat < count; repeat++)
        {

            for (int list = 0; list < mapArrList.Count; list++)
            {

                for (int col = 0; col < mapArrList[list].GetLength(0); col++)
                {
                    for (int row = 0; row < mapArrList[list].GetLength(1); row++)
                    {
                        wallCount = 0;
                        // ↗↖↙↘
                        TileTransformation(list, col + 1, row + 1);
                        TileTransformation(list, col - 1, row + 1);
                        TileTransformation(list, col - 1, row - 1);
                        TileTransformation(list, col + 1, row - 1);

                        // ↑ → ↓ ←
                        TileTransformation(list, col, row + 1);
                        TileTransformation(list, col + 1, row);
                        TileTransformation(list, col, row - 1);
                        TileTransformation(list, col - 1, row);

                        //이게 CellularAutomata 알고리즘 
                        //주위의 벽이 5개 이상이면 벽으로 그게 아니면 땅으로 변경 하는 작업
                        if (wallCount > 4)
                            mapArrList[list][col, row].bIsWall = true;
                        else
                        {
                            mapArrList[list][col, row].bIsWall = false;
                        }
                    }
                }

            }


        }

        RefreshTile();
    }

    void CreatTile()
    {
        for (int list = 0; list < mapArrList.Count; list++)
        {
            for (int i = 0; i < mapArrList[list].GetLength(0); i++)
            {
                for (int j = 0; j < mapArrList[list].GetLength(1); j++)
                {
                    //가장자리는 아웃라인 벽
                    if (i == 0 || i == mapWidth || j == 0 || j == mapHeight)
                    {
                        if(i == 0)
                            InitTile(mapArrList[list][i, j], tileWall[0]);
                        else if (i == mapWidth)
                            InitTile(mapArrList[list][i, j], tileWall[1]);
                        else if (j == 0)
                            InitTile(mapArrList[list][i, j], tileWall[2]);
                        else if (j == mapHeight)
                            InitTile(mapArrList[list][i, j], tileWall[3]);
                    }
                    else
                    {
                        InitTile(mapArrList[list][i, j]);
                    }                       
                }
            }
        }
    }

    void ReCreateMapWall(int width = 0, int height = 0)
    {
        mapArrList.Clear();
        CreateMapWall();
    }

    void RefreshTile()
    {
        ClearTile();
        CreatTile();
    }

    void ClearTile()
    {
        gridWall.ClearAllTiles();
        gridGround.ClearAllTiles();


        //Vector3Int pos = Vector3Int.zero;
        //for (int list = 0; list < mapArrList.Count; list++)
        //{
        //    for (int i = 0; i < mapArrList[list].GetLength(0); i++)
        //    {
        //        for (int j = 0; j < mapArrList[list].GetLength(1); j++)
        //        {
        //            pos.x = mapArrList[list][i, j].x;
        //            pos.y = mapArrList[list][i, j].y;
        //            gridWall.SetTile(pos, null);
        //            gridGround.SetTile(pos, null);
        //        }
        //    }
        //}
    }

    void InitTile(Cgrid Cgrid, TileBase _tile = null)
    {
        Vector3Int pos = new Vector3Int(Cgrid.x, Cgrid.y, 0);
        System.Random random = new System.Random();

        if (Cgrid.bIsWall)
        {
            if(_tile)
            {
                gridWall.SetTile(pos, _tile);
            }
            else
            gridWall.SetTile(pos, tileWall[random.Next(4, tileWall.Length)]);
        }
        else
        {
            gridGround.SetTile(pos, tileGround[random.Next(0, tileGround.Length)]);
        }

        

    }


    void TileErase()
    {
        ClearTile();
        mapArrList.Clear();
        listmoveVec.Clear();
        centerPosVec = null;
    }

    int TileTransformation(int list, int x, int y)
    {

        //맵의 크기를 벗어나면 WallCount 증가
        if (x > mapWidth || x < 0 || y > mapHeight || y < 0)
            return ++wallCount;

        //지정한 지역이 벽이라면 WallCount 증가
        if (mapArrList[list][x, y].bIsWall)
            ++wallCount;
        



        return wallCount;
    }


    //test
    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;

        if (listmoveVec != null && listmoveVec.Count != 0)
        {
            for (int i = 0; i < listmoveVec.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector2(listmoveVec[i].x, listmoveVec[i].y), new Vector2(listmoveVec[i + 1].x, listmoveVec[i + 1].y));
            }
        }
    }


    void CreateDunegeon()
    {
        RoomConnect();
      
        CreatTile();
    }

}
