using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopographyParent : MonoBehaviour
{
    public Topography topographyPre;
    public int blockSizeX;
    public int blockSizeY;
    public int blockSizeZ;

    public int plusBlockY;

    public int topoSize;

    public float WaveLength;
    public float Amplitude;

    public Player[] players;
    private int playerX; //index
    private int playerZ; //index

    private Topography[,] topographies;

    private bool isStart;
    private void Awake()
    {
       
    }
    //∏ ¿∫ toposize¿« 2πË 
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

    public void CreateTopo()
    {
        for(int i = -topoSize; i < topoSize; i++)
        {
            for (int j = -topoSize; j < topoSize; j++)
            {
                topographies[i + topoSize, j + topoSize] = Instantiate(topographyPre);
                topographies[i + topoSize, j + topoSize].Init(blockSizeX, blockSizeY, blockSizeZ, WaveLength, Amplitude, i * blockSizeX, plusBlockY, j * blockSizeZ, this, topoSize * blockSizeX, topoSize * blockSizeZ);
                topographies[i + topoSize, j + topoSize].CreateTopography();
            }
        }
    }

    public void FindPlayerPosition()
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

    public int playerXF()
    {
        return DownNum(players[0].transform.position.x / blockSizeX);
    }
    public int playerZF()
    {
        return DownNum(players[0].transform.position.z / blockSizeZ);
    }

    public void InstallBlock(int x,int y,int z, Block item)
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
                topographies[i + playerX + topoSize, j + playerZ + topoSize].InstallBlock(x, y, z, item);
            }
        }
    }

    public void BrokenBlock(int x, int y, int z)
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
                topographies[i + playerX + topoSize, j + playerZ + topoSize].BrokenBlock(x, y, z);
            }
        }
        VisibleNearBlock(x, y, z);
    }

    public void VisibleNearBlock(int x,int y,int z)
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

    public void InvisibleNearTopo(int x, int z)
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

    public void VisibleNearTopo(int x,int z)
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
}
