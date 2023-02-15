using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topography : MonoBehaviour
{
    public AddImages addImages;
    public TopographyParent Tparent;
    public Block[,,] blocks;
    public float WaveLength;
    public float Amplitude;
    public int Width_X;
    public int Width_Z;
    public int Height_Y;
    public int plusX;
    public int plusY;
    public int plusZ;
    public int maxX;
    public int maxZ;


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
            blocks[x - plusX, y, z - plusZ].Destruction();
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

    public void InstallBlock(int x, int y, int z, Block item)
    {
        if (x - plusX < 0 ||x - plusX >= Width_X)
            return;
        if (z - plusZ < 0 ||z - plusZ >= Width_Z)
            return;
        if (y < 0 || y >= Height_Y)
            return;

        blocks[x, y, z] = item;
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

        StartCoroutine(CreateDownTo());
    }

    public IEnumerator CreateDownTo()
    {
        int topY = -1;

        for (int i = 0; i < Width_X; i++)
        {
            for (int j = 0; j < Width_Z; j++)
            {
                topY = -1;
                for (int k = Height_Y - 1; k >= 5; k--)        //최소높이를 정하는곳
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
                            //blocks[i, k, j].SetBlockTexturePath("Tex/s_k_i_n");   //블럭 텍스쳐 바꾸는 방법
                            //blocks[i, k, j].ApplyBlockTexture();                  //블럭 텍스쳐 바꾸는 방법
                            blocks[i, k, j].SetInvisible();
                        }
                    }

                    if (blocks[i, k, j] != null && topY == -1)
                    {
                        topY = k;
                    }
                    
                }
                yield return null;
            }
        }
        Debug.Log("생성완료");
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
    }

    public void SetSize(int x, int y, int z)
    {
        Width_X = x;
        Height_Y = y;
        Width_Z = z;
    }
    public void SetPosi(int x, int y, int z)
    {
        plusX = x;
        plusY = y;
        plusZ = z;
    }
    public void SetWave(int wave = 20,int amp = 10)
    {
        WaveLength = wave;
        Amplitude = amp;
    }
    public void SetTParent(TopographyParent tp)
    {
        Tparent = tp;
    }
}
