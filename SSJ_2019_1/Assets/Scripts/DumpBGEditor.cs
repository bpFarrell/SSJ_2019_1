using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DumpBGEditor : MonoBehaviour
{
    public Material mat;
    public bool randomize;
    public bool save;
    public int size = 256;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (save) {
            save = false;
            Save();
        }
        if (randomize) {
            randomize = false;
            GameManager.RandomizeBG(mat);
        }
    }
    void Save() {
        string path = Application.dataPath+"/"+ System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")+".png";
        Debug.Log("Saving to " + path);
        RenderTexture rt = new RenderTexture(size, size, 0);
        Graphics.Blit((Texture2D)null, rt, mat);
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(size, size);
        tex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
    }
}
