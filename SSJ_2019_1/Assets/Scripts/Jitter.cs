using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    Vector3 startPos;
    Vector3 startRot;
    public float freqPos;
    public float freqRot;
    public float magPos;
    public float magRot;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        transform.position = startPos + GetRand(0, freqPos, magPos);
        transform.eulerAngles = startRot + GetRand(0, freqRot, magRot);
    }
    Vector3 GetRand(float offset,float freq,float mag) {
        Vector3 r =
            new Vector3(
                Mathf.PerlinNoise(GameManager.time * freq, offset  + 0),
                Mathf.PerlinNoise(GameManager.time * freq, offset  + 1),
                Mathf.PerlinNoise(GameManager.time * freq, offset  + 2)) * mag;


            return r;
    }
}
