using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour, ITimeObject {
    private static List<BulletPool> activeItem = new List<BulletPool>();
    private static List<BulletPool> pendingItems = new List<BulletPool>();
    private static List<BulletPool> pooledItems = new List<BulletPool>();
    private static GameObject holder;
    MeshRenderer mr;
    [SerializeField]
    public float t { get {
            return GameManager.time - spawnTime;
        }
        set { } }
    public float spawnTime { get; set; }
    public float deathTime { get; set; }
    public Vector3 startpos { get; set; }
    public IEvaluable evaluable { get; set; }
    public float parentAgeAtBirth;
    public float splitTime = 2;
    public bool didSplit = false;
    bool dead;

    public Vector3 dir;
    public Vector3 curve;
    void Awake() {
        mr = GetComponent<MeshRenderer>();
        evaluable = new IEvaluable();
        deathTime = 10;
    }
    public static BulletPool GetObject() {
        if (pooledItems.Count == 0) {
            PopulatePool(100);
        }
        BulletPool obj = pooledItems[0];
        obj.MarkPolledToActive();
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
        MarkPolledToActive();
        evaluable.eval = (t) => { return parentEval.eval(parentAgeAtBirth) + t * dir + curve * Mathf.Pow(t, 2); };
        Update();
    }
    public virtual void MarkActiveToPooled() {
        activeItem.Remove(this);
        pooledItems.Add(this);
        gameObject.SetActive(false);
        transform.localScale = Vector3.one;
        //splitTime = 1000;
    }
    public virtual void MarkPolledToActive() {
        dead = false;
        pooledItems.Remove(this);
        activeItem.Add(this);
        gameObject.SetActive(true);
    }
    public virtual void MarkActiveToPending() {
        dead = true;
        mr.enabled = false;
        activeItem.Remove(this);
        pooledItems.Add(this);
    }
    public virtual void MarkPendingToActive() {
        dead = false;
        mr.enabled = true;
        pooledItems.Remove(this);
        activeItem.Add(this);
    }
    public void Update() {
        if (t < 0) {
            MarkActiveToPooled();
            return;
        }
        CheckDeath();
        transform.position = evaluable.eval(t);

        if (t< splitTime&&didSplit) {
            didSplit = false;
        }
        if (t>splitTime&&!didSplit) {
            didSplit = true;
            return;
            for(int x = 0; x < 8; x++) {
                float angle = ((float)x) / 8;
                BulletPool obj = BulletPool.GetObject();
                obj.spawnTime = spawnTime+splitTime;
                obj.dir = new Vector3(Mathf.Sin(angle * 2 * 3.141529f), -Mathf.Cos(angle * 2 * 3.141529f), 0);
                obj.parentAgeAtBirth = GameManager.time - spawnTime;
                obj.Init(evaluable);
                obj.transform.localScale *= 0.2f;
            }
        }
    }
    void CheckDeath() {
        if (t > deathTime && !dead) {
            MarkActiveToPending();
        } else if (t < deathTime && dead) {
            MarkPendingToActive();
        }
        
    }
}
