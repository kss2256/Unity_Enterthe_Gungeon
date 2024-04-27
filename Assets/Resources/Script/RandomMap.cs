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

    [SerializeField] private int mapWidth, mapHeight;       //���� ũ��
    [SerializeField] private int wallRatio;                 //���� ����
    [SerializeField] private int roomCount;                 //������ ����


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



    //���� ũ�� �� �����ڸ� �� �����
    void CreateMapWall()
    {
        //+1 �ϴ� ������ �����ڸ� ��
        if(mapArray == null)
        mapArray = new Cgrid[mapWidth + 1, mapHeight + 1];

        //���� ���� ��ŭ ���� ���� �����ֱ� 
        int tile = mapWidth * mapHeight;


        System.Random random = new System.Random();


        for (int i = 0; i < mapWidth + 1; i++)
        {
            for (int j = 0; j < mapHeight + 1; j++)
            {
                bool wall = false;
                //�� �� �����ڸ��� ���� ������
                if (i == 0 || i == mapWidth || j == 0 || j == mapHeight)
                {
                    wall = true;
                }
                else
                {
                    //�ۼ�Ʈ�� �� ���� (����)
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

        //���귯 �۾� count Ƚ����ŭ ---
        for (int repeat = 0; repeat < count; repeat++)
        {
           
            for(int col = 0; col < mapArray.GetLength(0); col++)
            {
                for(int row = 0; row < mapArray.GetLength(1); row ++)
                {
                    wallCount = 0;
                    // �֢آע�
                    TileTransformation(col + 1, row + 1);
                    TileTransformation(col - 1, row + 1);
                    TileTransformation(col - 1, row - 1);
                    TileTransformation(col + 1, row - 1);

                    // �� �� �� ��
                    TileTransformation(col, row + 1);
                    TileTransformation(col + 1, row);
                    TileTransformation(col, row - 1);
                    TileTransformation(col - 1, row);

                    //�̰� CellularAutomata �˰��� 
                    //������ ���� 5�� �̻��̸� ������ �װ� �ƴϸ� ������ ���� �ϴ� �۾�
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

        //���� ũ�⸦ ����� WallCount ����
        if (x > mapWidth || x < 0 || y > mapHeight || y < 0)
            return ++wallCount;

        //������ ������ ���̶�� WallCount ����
        if (mapArray[x, y].bIsWall)
            ++wallCount;
        



        return wallCount;
    }

}
