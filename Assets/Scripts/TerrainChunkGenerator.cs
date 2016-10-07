using UnityEngine;
using System.Collections;

public class TerrainChunkGenerator : MonoBehaviour {

    public int width = 128;
    public int length = 128;
    public int seed = 1000;
    
    public GameObject grass;
    public GameObject sand;
    public GameObject snow;

    // Use this for initialization
    static int EMPTY_SPACE = 0;
    static int BLOCK_GENERATED = 1;
    static int BLOCK_INVISIBLE = 2;

    private int[,,] allBlocks;
    public int startX, startZ;

    void Start () {
        startX = (int) transform.position.x - width / 2;
        startZ = (int) transform.position.z - length / 2;

        allBlocks = new int[width, 50, length];

        for (int x = 0; x < width; x++ )
        {
            for (int z = 0; z < length; z++)
            {
                int adjustedX = seed + startX + x;
                int adjustedZ = seed + startZ + z;
                int y = 10 + (int) (Mathf.PerlinNoise(adjustedX/35f, adjustedZ/35f) * 20f);

                GenerateRow(new Vector3(x, y, z));
            }
        }
	}

    void GenerateRow(Vector3 position)
    {
        GenerateBlock(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;
        for (int lowerY = y - 1; lowerY >= 0; lowerY--)
            allBlocks[x, lowerY, z] = BLOCK_INVISIBLE;
    }

    public void GenerateBlock(Vector3 position)
    {
        if (position.y >= 50)
            return;

        GameObject prefab;
        if (position.y < 17)
            prefab = sand;
        else if (position.y > 26)
            prefab = snow;
        else
            prefab = grass;

        Vector3 adjustedPosition = new Vector3(startX + position.x,
                                               position.y,
                                               startZ + position.z);
        GameObject newBlock = (GameObject) Instantiate(prefab, adjustedPosition, Quaternion.identity);
        newBlock.transform.parent = gameObject.transform;

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;
        allBlocks[x, y, z] = BLOCK_GENERATED;
    }

    public void DestroyedBlock(Vector3 position)
    {
        int positionX = (int) position.x;
        int positionY = (int) position.y;
        int positionZ = (int) position.z;

        for (int x = positionX-1; x <= positionX +1; x++)
        {
            for (int y = positionY-1; y <= positionY + 1; y++)
            {
                for (int z = positionZ - 1; z <= positionZ + 1; z++)
                {
                    if (allBlocks[x, y, z] == BLOCK_INVISIBLE)
                        GenerateBlock(new Vector3(x, y, z));
                }
            }
        }

        allBlocks[positionX, positionY, positionZ] = EMPTY_SPACE;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
