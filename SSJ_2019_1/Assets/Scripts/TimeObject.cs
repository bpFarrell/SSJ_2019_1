using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour, ITimeObject {
    public Vector3 dir;
    /// <summary>
    /// Age (Local Time)
    /// </summary>
    public float t { get { return GameManager.time - spawnTime; } set { } }
    /// <summary>
    /// Birth time (Gloabl Time)
    /// </summary>
    public float spawnTime { get; set; }
    private float _scheduledDeathTime;
    private float _actualdDeathTime;
    /// <summary>
    /// death timer
    /// </summary>
    public float scheduledDeathTime {
        get { return _scheduledDeathTime; }
        set { _actualdDeathTime = _scheduledDeathTime = value; } }
    /// <summary>
    /// time of death smaller than scheduled if has died from non old age
    /// </summary>
    public float actualdDeathTime {
        get { return _actualdDeathTime; }
        set { _actualdDeathTime = Mathf.Min(value, _scheduledDeathTime); } }
    /// <summary>
    /// Method to describe position(t)
    /// </summary>
    public IEvaluable evaluable { get; set; }
    /// <summary>
    /// shoudl be used, but is not :/
    /// </summary>
    public TimeState timeState { get; set; }
    /// <summary>
    /// The age of your parent at birth (need for time heirarcys greater than 3)
    /// </summary>
    public float parentAgeAtBirth { get; set; }
    /// <summary>
    /// Are you currently dead at this point in perceived time
    /// </summary>
    public bool dead;

    /// <summary>
    /// Call this to update the continuous position.
    /// </summary>
    public virtual void TimeUpdate() {
        transform.position = evaluable.eval(t);
        CheckDeath();
        if (t < 0) {
            BeforeBirth();
        }
    }
    private void CheckDeath() {
        if (t > scheduledDeathTime && !dead) {
            dead = true;
            AfterNaturalDeath();
        } else if (t < actualdDeathTime && dead) {
            dead = false;
            Resurrect();
            actualdDeathTime = scheduledDeathTime;
        }
    }
    /// <summary>
    /// Flag this object has been killed prematurly
    /// </summary>
    public virtual void Kill(float gloablKillTime) {
        dead = true;
        actualdDeathTime = gloablKillTime - spawnTime;
        GameManager.instance.OnTurnComplete += OnTurnComplete;
    }
    /// <summary>
    /// Transition from alive to before birth (rewind)
    /// </summary>
    public virtual void BeforeBirth() {

    }
    /// <summary>
    /// Transition from Alive to dead via death timer
    /// </summary>
    public virtual void AfterNaturalDeath() {

        GameManager.instance.OnTurnComplete += OnTurnComplete;

    }
    /// <summary>
    /// Transition to living from death (rewind)
    /// </summary>
    public virtual void Resurrect() {
        GameManager.instance.OnTurnComplete -= OnTurnComplete;
        timeState = TimeState.Alive;
    }
    /// <summary>
    /// Transition to living from prebirth
    /// </summary>
    public virtual void Birth() {

    }
    private void OnTurnComplete() {
        if (timeState != TimeState.PendingCleanup) {
            Debug.Log("Setting pending cleanup on " + gameObject.name);
            timeState = TimeState.PendingCleanup;
        }else if (timeState == TimeState.PendingCleanup) {
            Debug.Log("Actually cleaning up " + gameObject.name);
            GameManager.instance.OnTurnComplete -= OnTurnComplete;
            TotalCleanup();
        }
    }
    public virtual void TotalCleanup() {
        Destroy(gameObject);
    }
    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(evaluable.eval(0),0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(evaluable.eval(scheduledDeathTime), 0.5f);
        for (float x = 0.1f; x < scheduledDeathTime; x += 0.1f) {
            float m = x % 1;
            Gizmos.color = new Color(0,m,m);
            Gizmos.DrawLine(evaluable.eval(x), evaluable.eval(x - 0.1f));
        }
    }
}