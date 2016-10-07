using UnityEngine;
using System.Collections;

public class DaylightController : MonoBehaviour {
    public float rotatePerSecond = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotatePerSecond * Time.deltaTime, 0f, 0f);

        Vector3 rotation = transform.rotation.eulerAngles;
        if (rotation.x >= 180f)
            transform.rotation = Quaternion.Euler(new Vector3(180f, rotation.y, rotation.z));
    }
}
