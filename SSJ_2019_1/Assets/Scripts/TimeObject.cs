using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour, ITimeObject {
    public Vector3 dir;

    public float t { get; set; }
    public float spawnTime { get; set; }
    public float deathTime { get; set; }
    public Vector3 startpos { get; set; }
    public IEvaluable evaluable { get; set; }

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
