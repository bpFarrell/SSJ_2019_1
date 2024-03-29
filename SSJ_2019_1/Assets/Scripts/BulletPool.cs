﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletPool : TimeObject {
    private static List<BulletPool> activeItem = new List<BulletPool>();
    private static List<BulletPool> pendingItems = new List<BulletPool>();
    private static List<BulletPool> pooledItems = new List<BulletPool>();
    private static Material _mat;
    public static Material mat {
        get {
            return _mat ?? (_mat = Resources.Load("Unlit_Projectile") as Material);
        }
    }
    private static Material _mat1;
    public static Material mat1 {
        get {
            return _mat1 ?? (_mat1 = Resources.Load("Unlit_Projectile 1") as Material);
        }
    }
    private static GameObject holder;
    MeshRenderer mr;
    Rigidbody rb;
    SphereCollider sc;
    static int spawnCount;
    public float splitTime = 2;
    public bool didSplit = false;
    public Vector3 curve;
    void Awake() {
        mr = GetComponentInChildren<MeshRenderer>();
        evaluable = new IEvaluable();
        scheduledDeathTime = 10;
        sc = gameObject.GetComponent<SphereCollider>();
        sc.radius = 0.2f;
        sc.center = Vector3.up * 0.1f;
        sc.isTrigger = true;
        activeItem = new List<BulletPool>();
        pendingItems = new List<BulletPool>();
        pooledItems = new List<BulletPool>();
        spawnCount = 0;
    }
    public static BulletPool GetObject() {
        if (pooledItems.Count == 0) {
            PopulatePool(100);
        }
        BulletPool obj = pooledItems[0];
        obj.MarkPolledToActive();
        obj.SetMat(0);
        return obj;
    }
    private void OnDisable() {
    }
    public void SetMat(int type) {
        switch (type) {
            case (0):
                mr.material = mat;
                break;
            case (1):
                mr.material = mat1;
                break;
            default:
                break;
        }
    }
    public static void PopulatePool(int count) {
        if (holder == null)
            holder = new GameObject("poolHolder");
        for (int x = 0; x < count; x++) {
            GameObject go = Instantiate(Resources.Load("EnemyShot"))as GameObject;
            go.name = "pooled " + spawnCount;
            go.layer = LayerMask.NameToLayer("EnemyBullet");
            spawnCount++;
            go.transform.parent = holder.transform;
            go.SetActive(false);
            go.GetComponentInChildren<MeshRenderer>().material = mat;
            BulletPool obj = go.GetComponent<BulletPool>();

            go.transform.localScale = Vector3.one * 2;
            pooledItems.Add(obj);
        }
        Debug.Log("Pooling more! pool is at "+(pooledItems.Count+activeItem.Count+pendingItems.Count));
    }
    public void Init(IEvaluable parentEval) {
        //MarkPolledToActive();
        evaluable.eval = (t) => { return parentEval.eval(parentAgeAtBirth) + t * dir + curve * Mathf.Pow(t, 2); };
        Update();
    }
    public virtual void MarkActiveToPooled() {
        activeItem.Remove(this);
        pooledItems.Add(this);
        gameObject.SetActive(false);
        transform.localScale = Vector3.one*2;
        //splitTime = 1000;
    }
    public virtual void MarkPolledToActive() {
        dead = false;
        pooledItems.Remove(this);
        activeItem.Add(this);
        gameObject.SetActive(true);
        SoftEnable(true);
    }
    public virtual void MarkActiveToPending() {
        dead = true;
        SoftEnable(false);
        activeItem.Remove(this);
        pendingItems.Add(this);
        SpawnDeathAnim();
    }
    public virtual void MarkPendingToActive() {
        dead = false;
        SoftEnable(true);
        pendingItems.Remove(this);
        activeItem.Add(this);
    }
    public virtual void MarkPendingToPooled() {

        SoftEnable(true);
        gameObject.SetActive(false);
        pendingItems.Remove(this);
        pooledItems.Add(this);
    }
    public void Update() {
        TimeUpdate();
        if (dead) return;
        SetRotation();
        if (t>splitTime&&!didSplit) {
            didSplit = true;
            for(int x = 0; x < 8; x++) {
                float angle = ((float)x) / 8;
                BulletPool obj = BulletPool.GetObject();
                obj.spawnTime = spawnTime+splitTime;
                obj.dir = new Vector3(Mathf.Sin(angle * 2 * 3.141529f), -Mathf.Cos(angle * 2 * 3.141529f), 0);
                obj.parentAgeAtBirth = GameManager.time - spawnTime;
                obj.Init(evaluable);
                obj.transform.localScale *= 0.2f;
                obj.scheduledDeathTime = 4;
                obj.splitTime = 100;
            }
        }
        CheckCounts();
    }
    void SetRotation() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir + curve * Mathf.Pow(t, 2));
    }
    void CheckCounts() {
        int currentCount = pooledItems.Count + activeItem.Count + pendingItems.Count;
        if (currentCount != spawnCount) {
            Debug.LogError(" MISSSS MATCH!!!!!");
        }
    }
    public override void BeforeBirth() {
        base.BeforeBirth();
        MarkActiveToPooled();
    }
    public override void AfterNaturalDeath() {
        base.AfterNaturalDeath();
        MarkActiveToPending();
    }
    public override void Resurrect() {
        base.Resurrect();
        MarkPendingToActive();

    }
    public override void Kill(float killedTime) {
        base.Kill(killedTime);
        MarkActiveToPending();
    }
    public override void TotalCleanup() {
        MarkPendingToPooled();
    }
    private void SpawnDeathAnim() {
        GameObject go = Instantiate(Resources.Load("EnemyShotDeath")) as GameObject;
        go.name = "DeathEffect";
        go.layer = LayerMask.NameToLayer("EnemyBullet");
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.transform.localScale = transform.localScale*2;
        go.GetComponentInChildren<MeshRenderer>().material = new Material(mr.material);
        ProjectileDeathAnim pda = go.AddComponent<ProjectileDeathAnim>();
        pda.parentAgeAtBirth = GameManager.time - spawnTime;
        pda.spawnTime = GameManager.time;
        pda.Init(evaluable);
    }
    private void SoftEnable(bool enable) {
        mr.enabled = enable;
        sc.enabled = enable;

    }
}
