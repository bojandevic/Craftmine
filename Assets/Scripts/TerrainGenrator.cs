using UnityEngine;
using System.Collections;

public class TerrainGenrator : MonoBehaviour {
    public GameObject chunkPrefab;
    public GameObject player;

    private int width;
    private int length;

    private GameObject currentChunk;

    private GameObject[,] chunks = new GameObject[100, 100];

	// Use this for initialization
	void Start () {
        TerrainChunkGenerator chunkGenerator = chunkPrefab.GetComponent<TerrainChunkGenerator>();
        width = chunkGenerator.width;
        length = chunkGenerator.length;

        updateChunks();
    }
	
	// Update is called once per frame
	void Update () {
        if (isOutsideOfCurrentChunk())
            updateChunks();
    }

    bool isOutsideOfCurrentChunk()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 chunkPosition = currentChunk.transform.position;

        bool outsideX = Mathf.Abs(playerPosition.x - chunkPosition.x) > width / 2;
        bool outsideZ = Mathf.Abs(playerPosition.z - chunkPosition.z) > length / 2;

        return outsideX || outsideZ;
    }

    void updateChunks()
    {
        Vector3 playerPosition = player.transform.position;

        int x = (int)(width * Mathf.Floor((playerPosition.x + width / 2) / width));
        int z = (int)(length * Mathf.Floor((playerPosition.z + length / 2) / length));

        int idX = x / width;
        int idZ = z / length;
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                // Generate or show close chunks
                float distance = Vector2.Distance(new Vector2(idX, idZ), new Vector2(i, j));
                if (distance < 2)
                {
                    if (chunks[i, j] != null)
                        chunks[i, j].SetActive(true);
                    else
                        generateChunk(i, j);
                }
                // Hide distant chunks
                else if (chunks[i, j] != null)
                    chunks[i, j].SetActive(false);
            }
        }

        currentChunk = chunks[idX, idZ];
    }

    GameObject generateChunk(int x, int z)
    {
        Vector3 position = new Vector3(x * width, 0, z * length);
        chunks[x, z] = (GameObject)Instantiate(chunkPrefab, position, Quaternion.identity);
        chunks[x, z].transform.parent = gameObject.transform;
        return chunks[x, z];
    }

    public void BlockDestroyed(Vector3 position)
    {
        TerrainChunkGenerator currentGenerator = currentChunk.GetComponent<TerrainChunkGenerator>();
        Vector3 adjustedPosition = new Vector3(position.x - currentGenerator.startX, 
                                               position.y, 
                                               position.z - currentGenerator.startZ);
        currentGenerator.DestroyedBlock(adjustedPosition);
    }

    public void GenerateBlock(Vector3 position)
    {
        TerrainChunkGenerator currentGenerator = currentChunk.GetComponent<TerrainChunkGenerator>();
        Vector3 adjustedPosition = new Vector3(position.x - currentGenerator.startX, 
                                               position.y, 
                                               position.z - currentGenerator.startZ);
        currentGenerator.GenerateBlock(adjustedPosition);
    }
}
