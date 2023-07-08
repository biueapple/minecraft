using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Progress;

public class TopographyParent : MonoBehaviour
{
    public Topography topographyPre;
    [Header("한개의 지형이 가지는 크기")]
    public int blockSizeX;
    public int blockSizeY;
    public int blockSizeZ;

    [Header("맵의 최소 높이")]
    public int plusBlockY;

    [Header("맵의 크기는 size의 2배")]
    public int topoSize;

    [Header("맵의 폭과 높낮이")]
    public float WaveLength;
    public float Amplitude;

    [Header("플레이어들")]
    public Player[] players;
    private int playerX; //index
    private int playerZ; //index

    private Topography[,] topographies;

    private bool isStart;

    [Header("나무의 스폰 확률 x분의 y")]
    public Vector2Int treeProb;

    private void Awake()
    {
       
    }
    //맵은 toposize의 2배 
    void Start()
    {
        Init();
    }

    void Update()
    {
        if(isStart)
        {
            FindPlayerPosition();
        }
    }

    public void Init()
    {
        players = FindObjectsOfType<Player>();
        topographies = new Topography[topoSize * 2, topoSize * 2];
        CreateTopo();
        VisibleNearTopo(playerX, playerZ);

        isStart = true;
    }

    private void CreateTopo()
    {
        for(int i = -topoSize; i < topoSize; i++)
        {
            for (int j = -topoSize; j < topoSize; j++)
            {
                topographies[i + topoSize, j + topoSize] = Instantiate(topographyPre);
                //초기화
                topographies[i + topoSize, j + topoSize].Init(blockSizeX, blockSizeY, blockSizeZ, WaveLength, Amplitude, i * blockSizeX, plusBlockY, j * blockSizeZ, this, topoSize * blockSizeX, topoSize * blockSizeZ);
                //지형 만들기
                topographies[i + topoSize, j + topoSize].CreateTopography();
            }
        }
        //나무심기
            //심을 위치 정하기
            //나무 만들기
            //잎 만들기
        TreePositionFind(topographies, treeProb);
        //동굴 만들기
    }

    public void TreePositionFind(Topography[,] topos, Vector2Int prob)
    {
        int minX = topos[0, 0].plusX, maxX = 0;
        int minZ = topos[0,0].plusZ, maxZ = 0;
        int y = -1;

        for(int i = 0; i < topos.GetLength(0); i++)
        {
            for (int j = 0; j < topos.GetLength(1); j++)
            {
                if (topos[i, j].plusX < minX)
                    minX = topos[i, j].plusX;
                if(topos[i, j].plusX + blockSizeX > maxX)
                    maxX = topos[i, j].plusX + blockSizeX;

                if (topos[i, j].plusZ < minZ)
                    minZ = topos[i, j].plusZ;
                if (topos[i, j].plusZ + blockSizeZ > maxZ)
                    maxZ = topos[i, j].plusZ + blockSizeZ;

            }
        }

        for(int i = minX; i < maxX; i++)
        {
            for(int j = minZ; j < maxZ; j++)
            {
                if(Random.Range(0, prob.x) < prob.y)
                {
                    y = GetTopoY(i, j);
                    CreateTree(i, y, j);
                }
            }
        }
    }

    public void CreateTree(int x, int y, int z)
    {
        int i = 0;
        for (; i < 5; i++)
        {
            if (!InstallBlock(x, y + i, z, _ITEMCODE.WOOD))
                break;
        }
        CreateLeaf(x, y + i, z);
    }

    public void CreateLeaf(int x, int y, int z)
    {
        for (int k = -1; k <= 1; k++)    // -1 0 1
        {
            for (int i = -1 + k; i <= 1 - k; i++)    // -2 -1 0 // 2 1 0
            {
                for (int j = -1 + k; j <= 1 - k; j++)    // -2 -1 0 // 2 1 0
                {
                    InstallBlock(x + i, y + k, z + j, _ITEMCODE.LEAF);
                }
            }
        }
    }

    private void FindPlayerPosition()
    {
        if (players[0] != null)
        {
            if (playerX != playerXF())
            {
                InvisibleNearTopo(playerX, playerZ);
                playerX = playerXF();
                VisibleNearTopo(playerX, playerZ);
            }
            if (playerZ != playerZF())
            {
                InvisibleNearTopo(playerX, playerZ);
                playerZ = playerZF();
                VisibleNearTopo(playerX, playerZ);
            }
        }
    }

    private int playerXF()
    {
        return DownNum(players[0].transform.position.x / blockSizeX);
    }
    private int playerZF()
    {
        return DownNum(players[0].transform.position.z / blockSizeZ);
    }

    public bool InstallBlock(int x,int y,int z, Block block, bool visible = false)
    {
        if (y < 0 || y >= blockSizeY)
        {
            return false;
        }

        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (x < topographies[i, j].plusX || x >= topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (z < topographies[i, j].plusZ || z >= topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                return topographies[i, j].InstallBlock(x, y, z, block, visible);
            }
        }
        return false;
    }

    public bool InstallBlock(int x, int y, int z, _ITEMCODE code, bool visible = false)
    {
        if (y < 0 || y >= blockSizeY)
        {
            return false;
        }

        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (x < topographies[i, j].plusX || x >= topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (z < topographies[i, j].plusZ || z >= topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                return topographies[i, j].InstallBlock(x, y, z, code, visible);
            }
        }
        return false;
    }

    public bool InstallBlock(Vector3Int vector, _ITEMCODE code, bool visible = false)
    {
        if (vector.y < 0 || vector.y >= blockSizeY)
        {
            return false;
        }

        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (vector.x < topographies[i, j].plusX || vector.x > topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (vector.z < topographies[i, j].plusZ || vector.z > topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                return topographies[i, j].InstallBlock(vector.x, vector.y, vector.z, code, visible);
            }
        }
        return false;
    }

    public bool InstallBlock(Vector3Int vector, Block item, bool visible = false)
    {
        if (vector.y < 0 || vector.y >= blockSizeY)
        {
            return false;
        }

        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (vector.x < topographies[i, j].plusX || vector.x > topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (vector.z < topographies[i, j].plusZ || vector.z > topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                return topographies[i, j].InstallBlock(vector.x, vector.y, vector.z, item, visible);
            }
        }
        return false;
    }

    public Block GetBlock(Vector3Int vector3)
    {
        if (vector3.y < 0 || vector3.y >= blockSizeY)
        {
            return null;
        }

        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (vector3.x < topographies[i, j].plusX || vector3.x >= topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (vector3.z < topographies[i, j].plusZ || vector3.z >= topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                return topographies[i, j].GetBlock(vector3);
            }
        }
        return null;
    }
    public void BrokenBlock(int x, int y, int z)
    {
        VisibleNearBlock(x, y, z);
        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (x < topographies[i, j].plusX || x >= topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (z < topographies[i, j].plusZ || z >= topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                topographies[i, j].BrokenBlock(x, y, z);
            }
        }
    }

    public void BrokenBlock(Vector3Int vector3)
    {
        VisibleNearBlock(vector3.x, vector3.y, vector3.z);
        for (int i = 0; i < topographies.GetLength(0); i++)
        {
            for (int j = 0; j < topographies.GetLength(1); j++)
            {
                if (vector3.x < topographies[i, j].plusX || vector3.x >= topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (vector3.z < topographies[i, j].plusZ || vector3.z >= topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }

                topographies[i, j].BrokenBlock(vector3.x, vector3.y, vector3.z);
            }
        }
    }

    private void VisibleNearBlock(int x,int y,int z)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i + playerX + topoSize < 0 || i + playerX + topoSize >= topoSize * 2)
                {
                    continue;
                }
                if (j + playerZ + topoSize < 0 || j + playerZ + topoSize >= topoSize * 2)
                {
                    continue;
                }
                topographies[i + playerX + topoSize, j + playerZ + topoSize].VisibleNearBlock(x, y, z);
            }
        }
        //
    }

    public int DownNum(float x)
    {
        if( x < 0)
        {
            return (int)(x - 1);
        }
        else
        {
            return (int)x;
        }
    }

    private void InvisibleNearTopo(int x, int z)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if(i + x + topoSize < 0 || i + x + topoSize >= topoSize * 2)
                {
                    continue;
                }
                if(j + z + topoSize < 0 || j + z + topoSize >= topoSize * 2)
                {
                    continue;
                }
                topographies[i + x + topoSize, z + j + topoSize].TopoInvisible();
            }
        }
    }

    private void VisibleNearTopo(int x,int z)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i + x + topoSize < 0 || i + x + topoSize >= topoSize * 2)
                {
                    continue;
                }
                if (j + z + topoSize < 0 || j + z + topoSize >= topoSize * 2)
                {
                    continue;
                }
                topographies[i + x + topoSize, z + j + topoSize].TopoVisible();
            }
        }
    }

    private int GetTopoY(int x,int z)
    {
        for(int i = 0; i < topographies.GetLength(0); i++)
        {
            for(int j = 0; j < topographies.GetLength(1); j++)
            {
                if (x < topographies[i, j].plusX || x > topographies[i, j].plusX + blockSizeX)
                {
                    continue;
                }
                if (z < topographies[i, j].plusZ || z > topographies[i, j].plusZ + blockSizeZ)
                {
                    continue;
                }
                return topographies[i, j].MaximumY(x,z);
            }
        }

        return -1;
    }
}
