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
    public static float frustumHeight;
    public static float frustumWidth;
    public static float halfH;
    public static float halfW;
    public static Rect screenRect;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
        tempRT = new RenderTexture(Screen.width, Screen.height, 0);

    }

    // Update is called once per frame
    void Update()
    {
        CalcCameraCrossSection();
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, tempRT, postMat);
        Graphics.Blit(tempRT, dest, edgeMat);
    }
    void CalcCameraCrossSection() {
        //TODO You should Remove me!
        frustumHeight = 2.0f * -transform.position.z * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * cam.aspect;
        halfH = frustumHeight / 2;
        halfW = frustumWidth / 2;
        screenRect = new Rect();// transform.position, new Vector2(frustumWidth, frustumHeight));
        screenRect.center = transform.position;
        screenRect.size = new Vector2(frustumWidth, frustumHeight);
        screenRect.min = (Vector2)transform.position - new Vector2(halfW, halfH);
        screenRect.max = (Vector2)transform.position + new Vector2(halfW, halfH);
    }
    private void OnDrawGizmos() {
        CalcCameraCrossSection();
        Gizmos.color = Color.red;
        Gizmos.DrawCube(
            new Vector3(transform.position.x, transform.position.y + halfH, 0),
            new Vector3(frustumWidth, 0.5f, 1)
            );
        Gizmos.DrawCube(
            new Vector3(transform.position.x, transform.position.y - halfH, 0),
            new Vector3(frustumWidth, 0.5f, 1)
            );
        Gizmos.DrawCube(
            new Vector3(transform.position.x - halfW, transform.position.y, 0),
            new Vector3(0.5f, frustumHeight, 1)
            );
        Gizmos.DrawCube(
            new Vector3(transform.position.x + halfW, transform.position.y, 0),
            new Vector3(0.5f, frustumHeight, 1)
            );
    }
}
