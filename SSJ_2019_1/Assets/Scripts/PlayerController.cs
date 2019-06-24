using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITimeObject {
    public float moveSpeed = 1;
    public float moveTimeScalar = 1f;
    public float screenPadding = 1;
    List<Vector3> poses = new List<Vector3>();
    float poseSaveIntervals = 0.1f;
    int currentFrame { get { return Mathf.FloorToInt(GameManager.time / poseSaveIntervals); } }
    float currentFrameTime { get { return currentFrame * poseSaveIntervals; } }
    float percentThroughFrame { get { return (GameManager.time - currentFrameTime) / poseSaveIntervals; } }
    public float t { get; set; }
    public float spawnTime { get; set; }
    public float scheduledDeathTime { get; set; }
    public Vector3 startpos { get; set; }
    public IEvaluable evaluable { get; set; }
    public TimeState timeState { get; set; }
    public float parentAgeAtBirth { get; set; }

    float lastSpawn;
    float spawnRate = 1;
    private void Awake() {
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return GetPosAtTime(t); };
    }
    void Update() {

        Player player = ReInput.players.GetPlayer(0);
        Vector3 dir = new Vector3(player.GetAxis("MoveHori"), player.GetAxis("MoveVert"), 0);
        if (dir.magnitude > Mathf.Epsilon) {
            GameManager.time += dir.magnitude * Time.deltaTime * moveTimeScalar;
            CullBranch();
            transform.position += dir * moveSpeed;
            TryBounds();
        }
        TryRecordPos();
        TryShoot();
        if (player.GetButtonDown("Confirm")) {
            GameObject go = Instantiate(Resources.Load("PlayerShot")) as GameObject;
            PlayerShot ps = go.GetComponent<PlayerShot>();
            ps.spawnTime = GameManager.time;
            ps.dir = Vector3.right * 20;
            ps.scheduledDeathTime = 5;
            ps.Init(evaluable);
        }
    }
    void TryRecordPos() {
        float lastSnap = ((float)poses.Count) * poseSaveIntervals;
        if (lastSnap < GameManager.time) {
            poses.Add(transform.position);
        }
        if (lastSnap - poseSaveIntervals > GameManager.time) {
            transform.position = GetCurrentPos();
        }
        for (int i = 1; i < poses.Count; i++) {
            Debug.DrawLine(poses[i - 1], poses[i]);
        }
    }
    void CullBranch() {
        if (currentFrame >= poses.Count - 1) return;
        int length = poses.Count - currentFrame - 1;
        Debug.Log("Culling branch " + currentFrame + ", " + length);
        poses.RemoveRange(currentFrame + 1, length);
    }
    Vector3 GetCurrentPos() {
        Vector3 newPos = Vector3.Lerp(poses[currentFrame], poses[currentFrame + 1], percentThroughFrame);
        return newPos;
    }
    Vector3 GetPosAtTime(float time) {
        int currentFrame = Mathf.FloorToInt(time / poseSaveIntervals);
        float currentFrameTime = currentFrame * poseSaveIntervals;
        float percentThroughFrame = (time - currentFrameTime) / poseSaveIntervals;
        int nowFrame = Mathf.Min(currentFrame, poses.Count - 1);
        int nextFrame = Mathf.Min(currentFrame + 1, poses.Count - 1);
        return Vector3.Lerp(
            poses[nowFrame],
            poses[nextFrame],
            percentThroughFrame);
    }
    void TryShoot() {
        return;
        if (GameManager.time < lastSpawn) {
            lastSpawn -= spawnRate;
        }
        if (GameManager.time > lastSpawn + spawnRate) {
            lastSpawn += spawnRate;
            BulletPool obj = BulletPool.GetObject();
            obj.spawnTime = lastSpawn;
            obj.parentAgeAtBirth = lastSpawn;
            obj.splitTime = 9.9f;
            obj.scheduledDeathTime = 10;
            obj.dir = (Vector3.right) * 3;
            obj.Init(evaluable);

        }
    }
    void TryBounds() {
        Vector3 clampedPos = new Vector3();
        Rect rect = CamPost.screenRect;
        clampedPos.x = Mathf.Clamp(transform.position.x, 
            rect.xMin+screenPadding, 
            rect.xMax-screenPadding-5);
        clampedPos.y = Mathf.Clamp(transform.position.y, 
            rect.yMin+screenPadding, 
            rect.yMax-screenPadding);
        transform.position = clampedPos;
    }
    private void OnTriggerEnter(Collider other) {
        TimeObject to = other.GetComponent<TimeObject>();
        to.Kill(GameManager.time);
    }
}