using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletSpam : TimeObject
{
    float lastSpawn;
    float spawnRate = 2;
    public int hp = 20;
    public Vector3 startPos;
    public float mag=1;
    public float freq=1;
    bool alternate;
    public GameObject renderObjects;
    Stack<KeyValuePair<float, float>> damageStack = new Stack<KeyValuePair<float, float>>();
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
    }

    // Update is called once per frame
    void Update()
    {

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
            obj.curve = Vector3.zero;
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
            obj.SetMat(1);
            obj.Init(evaluable);
        }
    }
    public override void Resurrect() {
        renderObjects.SetActive(true);
    }
    private void CheckDamageRewind() {
        if (damageStack.Count == 0) return;
        if (GameManager.time < damageStack.Peek().Key) {
            hp += (int)damageStack.Pop().Value;
        }
    }
    private void OnTriggerEnter(Collider other) {
        //if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")) return;
        TimeObject to = other.GetComponent<TimeObject>();
        to.Kill(GameManager.time);
        hp--;
        damageStack.Push(new KeyValuePair<float, float>(GameManager.time, 1));
        if (hp <= 0) {
            Debug.Log("Killed the boss!");
            if(GameManager.instance.state== GameState.SIMULATE_PLAY)
                GameManager.ChangeState(GameState.END);
            Kill(GameManager.time);
            renderObjects.SetActive(false);
        }
    }
    void OnNewTurn() {
        if(GameManager.instance.turnNumber%3>0)
            SpawnDangerZone(Rand.GetRange(0,5));
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