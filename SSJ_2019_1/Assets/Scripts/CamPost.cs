using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamPost : MonoBehaviour
{
    public Material postMat;
    public Material edgeMat;
    RenderTexture tempRT;
    Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
        tempRT = new RenderTexture(Screen.width/2, Screen.height/2, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, tempRT, postMat);
        Graphics.Blit(tempRT, dest, edgeMat);
    }
}
