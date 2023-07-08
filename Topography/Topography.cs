using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topography : MonoBehaviour
{
    private AddImages addImages;
    private TopographyParent Tparent;
    public Block[,,] blocks;
    private float WaveLength;
    private float Amplitude;
    private int Width_X;
    private int Width_Z;
    private int Height_Y;
    //���� ��ǥ blocks�� �ε����� �̸�ŭ ���̰� ����
    public int plusX;
    //���� ��ǥ�� blocks�� �ε����� ���� plusY�� �����ϴ� ������ �ּ� ���̸� �����ֱ⶧�� plusY = 8�̶�� 8 �Ʒ����ʹ� ����� �������� ����
    public int plusY;
    //���� ��ǥ blocks�� �ε����� �̸�ŭ ���̰� ����
    public int plusZ;
    private int maxX;
    private int maxZ;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void TopoVisible()
    {
        StartCoroutine(visible());
    }
    private IEnumerator visible()
    {
        for (int i = 0; i < Width_X; i++)
        {
            for (int k = 0; k < Height_Y; k++)
            {
                for (int j = 0; j < Width_Z; j++)
                {
                    if (blocks[i, k, j] != null)
                    {
                        blocks[i, k, j].SetVisible();
                    }
                }
            }
            yield return null;  
        }
    }
    public void TopoInvisible()
    {
        StartCoroutine(Invisible());
    }
    public IEnumerator Invisible()
    {
        for (int i = 0; i < Width_X; i++)
        {
            for (int k = 0; k < Height_Y; k++)
            {
                for (int j = 0; j < Width_Z; j++)
                {
                    if (blocks[i, k, j] != null)
                    {
                        blocks[i, k, j].SetInvisible();
                    }
                }
            }
            yield return null;
        }
    }

    public void BrokenBlock(int x, int y, int z)
    {
        if(x - plusX < 0 || x - plusX >= Width_X)
        {
            return;
        }
        if (z - plusZ < 0 || z - plusZ >= Width_Z)
        {
            return;
        }


        if (blocks[x - plusX, y, z - plusZ] != null)
        {
            blocks[x - plusX, y, z - plusZ] = null;
        }
    }

    public void VisibleNearBlock(int x,int y,int z)
    {
        for(int i = -1; i < 2; i++)
        {
            if (i + x - plusX < 0 || i + x - plusX >= Width_X)
                continue;

            for(int j = -1; j < 2; j++)
            {
                if (j + z - plusZ < 0 || j + z - plusZ >= Width_Z)
                    continue;

                for(int k = -1; k < 2; k++)
                {
                    if (k + y < 0 || k + y >= Height_Y)
                        continue;

                    if(blocks[i + x - plusX, k + y, j + z - plusZ] != null)
                    {
                        blocks[i + x - plusX, k + y, j + z - plusZ].isVisible = true;
                        blocks[i + x - plusX, k + y, j + z - plusZ].SetVisible();
                    }

                    
                }
            }
        }
    }

    public bool InstallBlock(int x, int y, int z, Block block, bool visible = false)
    {
        if (x - plusX < 0 ||x - plusX >= Width_X)
            return false;
        if (z - plusZ < 0 ||z - plusZ >= Width_Z)
            return false;
        if (y < 0 || y >= Height_Y)
            return false;

        if(blocks[x - plusX, y, z - plusZ] == null)
        {
            blocks[x - plusX, y, z - plusZ] = addImages.CreateBlock(block);
            blocks[x - plusX, y, z - plusZ].isVisible = true;
            if(!visible)
                blocks[x - plusX, y, z - plusZ].SetInvisible();
            else
                blocks[x - plusX, y, z - plusZ].SetVisible();
            blocks[x - plusX, y, z - plusZ].transform.position = new Vector3(x, y, z);
            blocks[x - plusX, y, z - plusZ].transform.SetParent(transform, false);
            return true;
        }
        return false;
    }

    public bool InstallBlock(int x, int y, int z, _ITEMCODE code, bool visible = false)
    {
        if (x - plusX < 0 || x - plusX >= Width_X)
            return false;
        if (z - plusZ < 0 || z - plusZ >= Width_Z)
            return false;
        if (y < 0 || y >= Height_Y)
            return false;

        if (blocks[x - plusX, y, z - plusZ] == null)
        {
            Block block = addImages.CreateBlock(code);
            blocks[x - plusX, y, z - plusZ] = block;
            block.isVisible = true;
            if (!visible)
                block.SetInvisible();
            else
                block.SetVisible();
            block.transform.position = new Vector3(x, y, z);
            block.transform.SetParent(transform, false);
            return true;
        }
        return false;
    }

    public void CreateTopography()
    {
        blocks = new Block[Width_X, Height_Y, Width_Z];


        for (int i = 0; i < Width_X; i++)
        {
            for(int j = 0; j < Width_Z; j++)
            {
                Block b = addImages.CreateBlock(_ITEMCODE.GRASS);

                float xcoord = (i + plusX + maxX) / WaveLength;
                float zcoord = (j + plusZ + maxZ) / WaveLength;

                int y = (int)(Mathf.PerlinNoise(xcoord, zcoord) * Amplitude);

                b.transform.position = new Vector3(i + plusX, y + plusY, j + plusZ);
                b.transform.rotation = Quaternion.identity;
                b.transform.SetParent(transform, false);
                b.isVisible = true;
                b.SetInvisible();

                blocks[i, y + plusY, j] = b;
            }
        }

        //StartCoroutine(CreateDownTo());
    }

    private IEnumerator CreateDownTo()
    {
        int topY = -1;

        for (int i = 0; i < Width_X; i++)
        {
            for (int j = 0; j < Width_Z; j++)
            {
                topY = -1;
                for (int k = Height_Y - 1; k >= 5; k--)        //�ּҳ��̸� ���ϴ°�
                {
                    if (topY != -1)
                    {
                        if (topY - k <= 2)
                        {
                            Block b = addImages.CreateBlock(_ITEMCODE.SOIL);
                            b.transform.position = new Vector3(i + plusX, k, j + plusZ);
                            b.transform.rotation = Quaternion.identity;
                            b.transform.SetParent(transform, false);
                            blocks[i, k, j] = b;

                            blocks[i, k, j].isVisible = false;
                            blocks[i, k, j].SetInvisible();
                        }
                        else
                        {
                            Block b = addImages.CreateBlock(_ITEMCODE.STONE);
                            b.transform.position = new Vector3(i + plusX, k, j + plusZ);
                            b.transform.rotation = Quaternion.identity;
                            b.transform.SetParent(transform, false);
                            blocks[i, k, j] = b;

                            blocks[i, k, j].isVisible = false;
                            //blocks[i, k, j].SetBlockTexturePath("Tex/s_k_i_n");   //�� �ؽ��� �ٲٴ� ���
                            //blocks[i, k, j].ApplyBlockTexture();                  //�� �ؽ��� �ٲٴ� ���
                            blocks[i, k, j].SetInvisible();
                        }
                    }

                    if (blocks[i, k, j] != null && topY == -1)      //���ΰ�
                    {
                        topY = k;
                    }
                    
                }
                yield return null;
            }
        }
        Debug.Log("�����Ϸ�");
    }

    public void Init(int sizeX,int sizeY, int sizeZ, float wave, float amp, int plusX, int plusY, int plusZ, TopographyParent tp, int maxplusX,int maxPlusZ)
    {
        addImages = FindObjectOfType<AddImages>();
        Width_X = sizeX;
        Height_Y = sizeY;
        Width_Z = sizeZ;
        WaveLength = wave;
        Amplitude = amp;
        this.plusX = plusX;
        this.plusY = plusY;
        this.plusZ = plusZ;
        Tparent = tp;
        maxX = maxplusX;
        maxZ = maxPlusZ;
        transform.SetParent(tp.transform, false);
    }

    /// <summary>
    /// ������ǥ�� �ް� �ش��ϴ� ����� ����
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Block GetBlock(Vector3Int vector3)
    {
        return blocks[vector3.x - plusX, vector3.y, vector3.z - plusZ];
    }

    /// <summary>
    /// ��ǥ�� ���� �� ������� y�� ����
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int MaximumY(int x, int z)
    {
        if (x - plusX < 0 || x - plusX >= Width_X)
            return -1;
        if (z - plusZ < 0 || z - plusZ >= Width_Z)
            return -1;

        bool b = false;

        for (int i = 0; i < Height_Y; i++)
        {
            if(blocks[x - plusX, i, z - plusZ] != null)
            {
                b = true;
            }

            if(blocks[x - plusX, i, z - plusZ] == null)
            {
                if (b)
                    return i;
            }
        }

        return -1;
    }


    /// <summary>
    /// blocks[x,y,z]�� �����δ� ��� ��ġ���ֳ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector3Int GetIndexToPosition(int x,int y, int z)
    {
        return new Vector3Int(x + plusX,y + plusY,z + plusZ);
    }

}
