using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellularAutomata : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    [SerializeField] private int smoothNum;

    private int[,] map;
    private const int ROAD = 0;
    private const int WALL = 1;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;
    [SerializeField] private Color[] colors;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        MapRandomFill();

        for (int i = 0; i < smoothNum; i++) //�ݺ��� �������� ������ ������ �Ų���������.
            SmoothMap();
    }

    private void MapRandomFill() //���� ������ ���� �� Ȥ�� �� �������� �����ϰ� ä��� �޼ҵ�
    {
        if (useRandomSeed) seed = Time.time.ToString(); //�õ�

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); //�õ�� ���� �ǻ� ���� ����

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) map[x, y] = WALL; //�����ڸ��� ������ ä��
                else map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; //������ ���� �� Ȥ�� �� ���� ����
                OnDrawTile(x, y); //Ÿ�� ����
                SetTileColor(x, y); //Ÿ�� ���� ����
            }
        }
    }

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4) map[x, y] = WALL; //�ֺ� ĭ �� ���� 4ĭ�� �ʰ��� ��� ���� Ÿ���� ������ �ٲ�
                else if (neighbourWallTiles < 4) map[x, y] = ROAD; //�ֺ� ĭ �� ���� 4ĭ �̸��� ��� ���� Ÿ���� �� �������� �ٲ�
                SetTileColor(x, y); //Ÿ�� ���� ����
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        { //���� ��ǥ�� �������� �ֺ� 8ĭ �˻�
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                { //�� ������ �ʰ����� �ʰ� ���ǹ����� �˻�
                    if (neighbourX != gridX || neighbourY != gridY) 
                    {
                        wallCount += map[neighbourX, neighbourY]; //���� 1�̰� �� ������ 0�̹Ƿ� ���� ��� wallCount ����
                    }                   
                }
                else 
                    wallCount++; //�ֺ� Ÿ���� �� ������ ��� ��� wallCount ����
            }
        }
        return wallCount;
    }

    private void SetTileColor(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0); //ȭ�� �߾� ����
        tilemap.SetTileFlags(pos, TileFlags.None); //Ÿ�� ������ �����ϱ� ���� TileFlags�� None���� ����
        switch (map[x, y])
        {
            case ROAD: tilemap.SetColor(pos, colors[0]); break;
            case WALL: tilemap.SetColor(pos, colors[1]); break;
        }
    }

    private void OnDrawTile(int x, int y)
    {
        Vector3Int pos = new Vector3Int(-width / 2 + x, -height / 2 + y, 0);
        tilemap.SetTile(pos, tile);
    }
}
