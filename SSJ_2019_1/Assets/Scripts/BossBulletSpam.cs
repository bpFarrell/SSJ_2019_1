﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpam : TimeObject
{
    float lastSpawn;
    float spawnRate = 2;
    float lastHitAt=-1;
    public int maxHP = 100;
    public int hp = 20;
    public Vector3 startPos;
    public float mag=1;
    public float freq=1;
    bool alternate;
    public GameObject renderObjects;
    Stack<KeyValuePair<float, float>> damageStack = new Stack<KeyValuePair<float, float>>();
    public MeshRenderer[] tentColor;
    public MeshRenderer[] tentDepth;
    public Material[] headMats;
    int nextDeadTent;
    public bool isdying;
    public float timeOfDeath;
    float percentHP {
        get { return ((float)hp) / ((float)maxHP); }
    }
    int currentLivingTentavles {
        get {
            return Mathf.FloorToInt(percentHP * (9 - Mathf.Epsilon));
        }
    }
    private void OnEnable() {
        GameManager.instance.OnNewTurn += OnNewTurn;
    }
    private void OnDisable() {
        GameManager.instance.OnNewTurn -= OnNewTurn;
    }
    void Start()
    {
        startPos = transform.position;
        evaluable = new IEvaluable();
        scheduledDeathTime = float.MaxValue;
        evaluable.eval = (t) => { return new Vector3(0,
            Mathf.PerlinNoise(0, t*freq) * mag+ Mathf.PerlinNoise(2, t*freq*0.5f) * mag*2,0)+
            startPos; };
        hp = maxHP;
        for (int x = 0; x < tentColor.Length; x++) {
            tentColor[x].material = new Material(tentColor[x].material);
            tentDepth[x].material = new Material(tentDepth[x].material);
        }

        for (int x = 0; x < headMats.Length; x++) {
            headMats[x].SetFloat("_Decay", 9999);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isdying) {
            IsDying();
            return;
        }
        TimeUpdate();
        CheckDamageRewind();
        if (dead) return;
        if (GameManager.time < lastSpawn) {
            lastSpawn -= spawnRate;
        }
        if (GameManager.time > lastSpawn + spawnRate) {
            lastSpawn += spawnRate;

            alternate = !alternate;
            //Simple();
            Volley();
            WaveSpawn();
        }
    }
    void IsDying() {
        transform.position -= new Vector3(1, 2, 1f)*Time.deltaTime;
        transform.Rotate(new Vector3(20, -17, 8) * Time.deltaTime);
        if (GameManager.time > timeOfDeath + 8) {
            GameManager.GoToTitle();
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
        int count = (alternate ? 4 : 3)+nextDeadTent;
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
            obj.curve = Vector3.zero;
            obj.Init(evaluable);
        }
    }
    public void Volley() {

        for (int x = 0; x < Mathf.Max(0, nextDeadTent*2); x++) {

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
            obj.SetMat(1);
            obj.Init(evaluable);
        }
    }
    public override void Resurrect() {
        renderObjects.SetActive(true);
    }
    private void CheckDamageRewind() {
        Shader.SetGlobalFloat("_BossHurt", 1-Mathf.Clamp01(((GameManager.time - lastHitAt) * 8)));
        if (damageStack.Count == 0) return;
        if (GameManager.time < damageStack.Peek().Key) {

            int preHitCount = currentLivingTentavles;
            hp += (int)damageStack.Pop().Value;
            if (damageStack.Count == 0)
                lastHitAt = -1;
            else
                lastHitAt = damageStack.Peek().Key;
            if (currentLivingTentavles != preHitCount) {
                nextDeadTent--;
                Debug.Log("Adding back tentacle " + nextDeadTent);
                nextDeadTent = Mathf.Clamp(nextDeadTent, 0, tentColor.Length - 1);

                tentColor[nextDeadTent].material.SetFloat("_Decay", 9999);
                tentDepth[nextDeadTent].material.SetFloat("_Decay", 9999);
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        //if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")) return;
        TimeObject to = other.GetComponent<TimeObject>();
        to.Kill(GameManager.time);
        int preHitCount = currentLivingTentavles;
        hp--;
        lastHitAt = GameManager.time;
        if (currentLivingTentavles != preHitCount) {

            Debug.Log("killing tentacle " + nextDeadTent);
            nextDeadTent = Mathf.Clamp(nextDeadTent, 0, tentColor.Length - 1);
            tentColor[nextDeadTent].material.SetFloat("_Decay", lastHitAt);
            tentDepth[nextDeadTent].material.SetFloat("_Decay", lastHitAt);
            nextDeadTent++;
        }
        damageStack.Push(new KeyValuePair<float, float>(GameManager.time, 1));
        if (hp <= 0) {
            Debug.Log("Killed the boss!");
            if (GameManager.instance.state == GameState.SIMULATE_PLAY) {
                GameManager.ChangeState(GameState.END);
                isdying = true;
                for (int x = 0; x < headMats.Length; x++) {
                    headMats[x].SetFloat("_Decay", GameManager.time+1);
                }
                timeOfDeath = GameManager.time;
            }
            Kill(GameManager.time);
            //renderObjects.SetActive(false);
        }
    }
    void OnNewTurn() {
        //if(GameManager.instance.turnNumber%3>0)
            //SpawnDangerZone(Rand.GetRange(0,5));
    }
    public void SpawnDangerZone(int type) {
        GameObject go = Instantiate(Resources.Load("DangerZone"))as GameObject;
        go.name = "DangeZone " + type;
        switch (type) {
            case (0): //Bottom
                go.transform.position = new Vector3(CamPost.screenRect.center.x, CamPost.screenRect.center.y - CamPost.frustumHeight / 4, 0);
                go.transform.localScale = new Vector3(CamPost.screenRect.size.x, CamPost.screenRect.size.y / 2, 0);
                break;
            case (1): //Top
                go.transform.position = new Vector3(CamPost.screenRect.center.x, CamPost.screenRect.center.y + CamPost.frustumHeight / 4, 0);
                go.transform.localScale = new Vector3(CamPost.screenRect.size.x, CamPost.screenRect.size.y / 2, 0);
                break;
            case (2):
                go.transform.position = new Vector3(CamPost.screenRect.center.x- CamPost.screenRect.size.x/8*1, CamPost.screenRect.center.y, 0);
                go.transform.localScale = new Vector3(CamPost.screenRect.size.x/4, CamPost.screenRect.size.y, 0);
                break;
            case (3):
                go.transform.position = new Vector3(CamPost.screenRect.center.x - CamPost.screenRect.size.x / 8 * 3, CamPost.screenRect.center.y, 0);
                go.transform.localScale = new Vector3(CamPost.screenRect.size.x / 4, CamPost.screenRect.size.y, 0);
                break;
            case (4):
                go.transform.position = new Vector3(CamPost.screenRect.center.x + CamPost.screenRect.size.x / 8 * 1, CamPost.screenRect.center.y, 0);
                go.transform.localScale = new Vector3(CamPost.screenRect.size.x / 4, CamPost.screenRect.size.y, 0);
                break;
            default:
                break;
        }

    }
}