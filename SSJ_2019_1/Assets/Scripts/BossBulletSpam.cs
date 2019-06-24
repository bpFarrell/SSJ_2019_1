using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpam : TimeObject
{
    float lastSpawn;
    float spawnRate = 2;
    public Vector3 startPos;
    public float mag=1;
    public float freq=1;
    bool alternate;
    void Start()
    {
        startPos = transform.position;
        evaluable = new IEvaluable();
        scheduledDeathTime = float.MaxValue;

        evaluable.eval = (t) => { return new Vector3(0,
            Mathf.PerlinNoise(0, t*freq) * mag+ Mathf.PerlinNoise(2, t*freq*0.5f) * mag*2,0)+
            startPos; };
    }

    // Update is called once per frame
    void Update()
    {
        TimeUpdate();
        if (GameManager.time < lastSpawn) {
            lastSpawn -= spawnRate;
        }
        if (GameManager.time > lastSpawn + spawnRate) {
            lastSpawn += spawnRate;

            alternate = !alternate;
            Simple();
            //Volley();
            //WaveSpawn();
        }
    }
    public void Simple() {
        BulletPool obj = BulletPool.GetObject();
        obj.spawnTime = lastSpawn;
        obj.parentAgeAtBirth = lastSpawn;
        obj.splitTime = 1000f;
        obj.scheduledDeathTime = 15;
        obj.dir = new Vector3(
            -4,
            0,
            0) * 4;
        obj.curve = Vector3.zero;
        obj.Init(evaluable);
    }
    public void WaveSpawn() {
        int count = alternate ? 10 : 9;
        for (int x = 0; x < count; x++) {

            float angle = ((float)x) / count;
            BulletPool obj = BulletPool.GetObject();
            obj.spawnTime = lastSpawn;
            obj.parentAgeAtBirth = lastSpawn;
            obj.splitTime = 1000f;
            obj.scheduledDeathTime = 100;
            obj.dir = new Vector3(
                -Mathf.Sin(angle * 3.141529f),
                -Mathf.Cos(angle * 3.141529f),
                0) * 2;
            obj.Init(evaluable);
        }
    }
    public void Volley() {

        for (int x = 0; x < 4; x++) {

            BulletPool obj = BulletPool.GetObject();
            obj.spawnTime = lastSpawn;
            obj.parentAgeAtBirth = lastSpawn;
            obj.splitTime = 1000f;
            obj.scheduledDeathTime = 20;
            obj.dir = new Vector3(
                -1+((float)4-x)*0.1f,
                (alternate ? 2 : -2),
                0) * 4;
            obj.curve = new Vector3(0, -1+(((float)x)/10), 0)* (alternate ? 1 : -1);
            obj.Init(evaluable);
        }
    }
}
