using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

    public GameObject terrain;
    public GameObject water;
    private TerrainGenrator generator;

    //The scene's default fog settings
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;

    // Use this for initialization
    void Start () {
        generator = terrain.GetComponent<TerrainGenrator>();

        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;

        Camera.main.backgroundColor = new Color(0, 0.4f, 0.7f, 1);
    }

    // Update is called once per frame
    void Update () {
	    if (Input.GetMouseButtonDown(0))
            onLeftMouseClick();
        else if (Input.GetMouseButtonDown(1))
            onRightMouseClicked();

        water.transform.position = new Vector3(transform.position.x, 
                                               water.transform.position.y, 
                                               transform.position.z);

        if (water.transform.position.y > gameObject.transform.position.y)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0, 0.6f, 0.9f, 0.5f);
            RenderSettings.fogDensity = 0.06f;
            RenderSettings.skybox = null;
            water.transform.rotation = Quaternion.Euler(180, 0, 0);
        }
        else
        {
            water.transform.rotation = Quaternion.identity;
            RenderSettings.fog = defaultFog;
            RenderSettings.fogColor = defaultFogColor;
            RenderSettings.fogDensity = defaultFogDensity;
            RenderSettings.skybox = defaultSkybox;
        }
    }

    private void onRightMouseClicked()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out hit, 7f))
        {
            float x = hit.transform.position.x;
            float y = hit.transform.position.y;
            float z = hit.transform.position.z;

            if (Mathf.Abs(hit.point.x - x) == 0.5f)
                x += Mathf.Sign(hit.point.x - x);
            else if (Mathf.Abs(hit.point.y - y) == 0.5f)
                y += Mathf.Sign(hit.point.y - y);
            else if (Mathf.Abs(hit.point.z - z) == 0.5f)
                z += Mathf.Sign(hit.point.z - z);

            generator.GenerateBlock(new Vector3(x, y, z));
        }
    }

    private void onLeftMouseClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out hit, 7f) && hit.transform.position.y > 0)
        {
            generator.BlockDestroyed(hit.transform.position);
            Destroy(hit.transform.gameObject);
        }
    }
}
