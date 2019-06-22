using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    public float t;
    public float spawnTime;
    public Vector3 dir;
    public Vector3 startpos;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Random.Range(0, 10);
        startpos = Random.insideUnitSphere;
        dir = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        t = GameManager.time - spawnTime;
        transform.position = startpos + dir * t;
    }
}
