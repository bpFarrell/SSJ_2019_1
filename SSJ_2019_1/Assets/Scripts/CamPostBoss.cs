using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamPostBoss : MonoBehaviour
{
    public static RenderTexture rt;
    public Material mat;
    Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGBFloat);
        cam = GetComponent<Camera>();
        cam.targetTexture = rt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, mat);
    }
}
