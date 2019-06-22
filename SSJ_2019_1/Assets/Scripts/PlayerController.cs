using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITimeObject
{
    public float moveSpeed = 1;
    public float moveTimeScalar = 0.1f;
    List<Vector3> poses = new List<Vector3>();
    float poseSaveIntervals = 0.1f;
    int currentFrame { get { return Mathf.FloorToInt(GameManager.time / poseSaveIntervals); } }
    float currentFrameTime { get { return currentFrame * poseSaveIntervals; } }
    float percentThroughFrame { get { return (GameManager.time - currentFrameTime) / poseSaveIntervals; } }
    public float t { get; set; }
    public float spawnTime { get; set; }
    public float deathTime { get; set; }
    public Vector3 startpos { get; set; }
    public IEvaluable evaluable { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Player player = ReInput.players.GetPlayer(0);
        Vector3 dir = new Vector3(player.GetAxis("MoveHori"), player.GetAxis("MoveVert"), 0);
        if (dir.magnitude > Mathf.Epsilon) {
            GameManager.time += dir.magnitude * moveTimeScalar;
            CullBranch();
        }
        transform.position += dir*moveSpeed;
        TryRecordPos();
    }
    void TryRecordPos() {
        float lastSnap = ((float)poses.Count) * poseSaveIntervals;
        if (lastSnap < GameManager.time) {
            Debug.Log("Snapping! " + poses.Count);
            poses.Add(transform.position);
        }
        if(lastSnap - poseSaveIntervals > GameManager.time) {
            transform.position = GetCurrentPos();
        }
        for (int i = 1; i < poses.Count; i++) {
            Debug.DrawLine(poses[i - 1],poses[i]);
        }
    }
    void CullBranch() {
        if (currentFrame >= poses.Count - 1) return;
        int length = poses.Count - currentFrame-1;
        Debug.Log("Culling branch " + currentFrame + ", " + length);
        poses.RemoveRange(currentFrame + 1, length);
    }
    Vector3 GetCurrentPos() {
        Vector3 newPos = Vector3.Lerp(poses[currentFrame], poses[currentFrame + 1], percentThroughFrame);
        return newPos;
    }

}
