using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackPlane : MonoBehaviour
{
    public bool go = false;
    [ExecuteInEditMode]
    private void Update()
    {
        if (go == false) return;
        go = false;
        float frustumHeight =   -Camera.main.transform.position.z * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * Camera.main.aspect;
        transform.localScale =  new Vector3(frustumWidth, transform.localScale.y, frustumHeight);
    }

}
