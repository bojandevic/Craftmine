using UnityEngine;
using System.Collections;

public class CloudsGenerator : MonoBehaviour {
    public int amount = 20;
    public int size = 20;

    public GameObject cloud;

	// Use this for initialization
	void Start () {
        int generatorX = (int) transform.position.x;
        int generatorZ = (int)transform.position.z;
        for (int a = 0; a < amount; a++)
        {
            int x = Random.Range(0, 128);
            int z = Random.Range(0, 128);
            for (int s = 0; s < size; s++)
            {
                Vector3 cloudPosition = new Vector3(x + generatorX, 50, z + generatorZ);
                GameObject cloud = (GameObject) Instantiate(this.cloud, cloudPosition, Quaternion.identity);
                cloud.transform.parent = gameObject.transform;

                x += Random.Range(-1, 2);
                z += Random.Range(-1, 2);
                x = (x < 128 && x > 0) ? x : 64;
                z = (z < 128 && z > 0) ? z : 64;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
