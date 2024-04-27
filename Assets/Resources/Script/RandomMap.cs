using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
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

public enum eMapDirection
{



}



public class RandomMap : MonoBehaviour
{

    [SerializeField] private int mapWidth, mapHeight;       //맵의 크기
    [SerializeField] private int wallRatio;                 //벽의 비율
    [SerializeField] private int roomCount;                 //랜덤맵 개수


    [SerializeField] private Tilemap gridGround;            
    [SerializeField] private Tilemap gridWall;
    [SerializeField] private Tile tileGround;
    [SerializeField] private Tile tileWall;

    private Cgrid[,] mapArray;
    private int wallCount;

    private void Awake()
    {
        
        CreateMapWall();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            CreateMapWall();
            RefreshTile();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            CellularAutomata();
            ClearTile();
            RefreshTile();

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ClearTile();

        }




    }


    void RoomConnect(int count)
    {
        System.Random random = new System.Random();

        for (int i = 0; i < roomCount; ++i)
        {

        }




    }



    //맵의 크기 맨 가장자리 벽 만들기
    void CreateMapWall()
    {
        //+1 하는 이유는 가장자리 벽
        if(mapArray == null)
        mapArray = new Cgrid[mapWidth + 1, mapHeight + 1];

        //벽의 비율 만큼 벽의 개수 정해주기 
        int tile = mapWidth * mapHeight;


        System.Random random = new System.Random();


        for (int i = 0; i < mapWidth + 1; i++)
        {
            for (int j = 0; j < mapHeight + 1; j++)
            {
                bool wall = false;
                //맵 의 가장자리는 전부 벽으로
                if (i == 0 || i == mapWidth || j == 0 || j == mapHeight)
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

                mapArray[i, j] = new Cgrid(i, j, wall);
                
            }
        }

    }

    void ClearTile()
    {

        Vector3Int pos = Vector3Int.zero;
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                pos.x = i;
                pos.y = j;
                gridWall.SetTile(pos, null);
                gridGround.SetTile(pos, null);
            }
        }
    }

    void RefreshTile()
    {
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                CreateTile(mapArray[i, j]);
            }
        }
    }

    void CreateTile(Cgrid Cgrid)
    {
        Vector3Int pos = new Vector3Int(Cgrid.x, Cgrid.y, 0);
        if(Cgrid.bIsWall)
        {
            gridWall.SetTile(pos, tileWall);
        }
        else
        {
            gridGround.SetTile(pos, tileGround);
        }

    }


    void CellularAutomata(int count = 1)
    {
        if (mapArray == null)
        {
            Debug.Log("MapArray Null");
            return;
        }

        //셀룰러 작업 count 횟수만큼 ---
        for (int repeat = 0; repeat < count; repeat++)
        {
           
            for(int col = 0; col < mapArray.GetLength(0); col++)
            {
                for(int row = 0; row < mapArray.GetLength(1); row ++)
                {
                    wallCount = 0;
                    // ↗↖↙↘
                    TileTransformation(col + 1, row + 1);
                    TileTransformation(col - 1, row + 1);
                    TileTransformation(col - 1, row - 1);
                    TileTransformation(col + 1, row - 1);

                    // ↑ → ↓ ←
                    TileTransformation(col, row + 1);
                    TileTransformation(col + 1, row);
                    TileTransformation(col, row - 1);
                    TileTransformation(col - 1, row);

                    //이게 CellularAutomata 알고리즘 
                    //주위의 벽이 5개 이상이면 벽으로 그게 아니면 땅으로 변경 하는 작업
                    if (wallCount > 4)
                        mapArray[col, row].bIsWall = true;
                    else
                    {
                        mapArray[col, row].bIsWall = false;
                    }
                }
            }




        }




    }


    int TileTransformation(int x, int y)
    {

        //맵의 크기를 벗어나면 WallCount 증가
        if (x > mapWidth || x < 0 || y > mapHeight || y < 0)
            return ++wallCount;

        //지정한 지역이 벽이라면 WallCount 증가
        if (mapArray[x, y].bIsWall)
            ++wallCount;
        



        return wallCount;
    }

}
