﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour, ITimeObject {
    private static List<BulletPool> activeItem = new List<BulletPool>();
    private static List<BulletPool> pooledItems = new List<BulletPool>();
    private static GameObject holder;
    MeshRenderer mr;
    public float t { get {
            return GameManager.time - spawnTime;
        }
        set { } }
    public float spawnTime { get; set; }
    public float deathTime { get; set; }
    public Vector3 startpos { get; set; }
    public IEvaluable evaluable { get; set; }

    bool dead;

    public Vector3 dir;
    public Vector3 curve;
    void Awake() {
        mr = GetComponent<MeshRenderer>();
        evaluable = new IEvaluable();
    }
    public static BulletPool GetObject() {
        if (pooledItems.Count == 0) {
            PopulatePool(100);
        }
        BulletPool obj = pooledItems[0];
        pooledItems.RemoveAt(0);
        activeItem.Add(obj);
        obj.gameObject.SetActive(true);
        return obj;
    }
    public static void PopulatePool(int count) {
        if (holder == null)
            holder = new GameObject("poolHolder");
        for (int x = 0; x < count; x++) {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.parent = holder.transform;
            go.SetActive(false);
            BulletPool obj = go.AddComponent<BulletPool>();
            pooledItems.Add(obj);
        }
        Debug.Log("Pooling more! pool is at "+(pooledItems.Count+activeItem.Count));
    }
    public void Init(IEvaluable parentEval) {
        evaluable.eval = (t) => { return parentEval.eval(spawnTime) + t * dir + curve * Mathf.Pow(t, 2); };
    }
    public virtual void MarkComplete() {
        activeItem.Remove(this);
        pooledItems.Add(this);
        gameObject.SetActive(false);
    }
    public virtual void MarkActive() {

    }
    public void Update() {
        if (dead && t > 0) {
            mr.enabled = true;
            dead = false;
        } else if (!dead && t <= 0) {
            mr.enabled = false;
            dead = true;
        }
        transform.position = evaluable.eval(t);
        //if (Time.time > killAt)
            //MarkComplete();
    }
}
