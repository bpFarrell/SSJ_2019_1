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
    // Start is called before the first frame update

    /// <summary>
    /// Call this to update the continuous position.
    /// </summary>
    public virtual void TimeUpdate() {
        transform.position = evaluable.eval(t);
        CheckDeath();
        if (t < 0) {
            BeforeBirth();
        }
        if (dead && t < actualdDeathTime) {
            dead = false;
            Resurrect();
            actualdDeathTime = scheduledDeathTime;
        }
    }
    private void CheckDeath() {
        if (t > scheduledDeathTime && !dead) {
            AfterNaturalDeath();
        } else if (t < scheduledDeathTime && dead) {
            Resurrect();
        }
    }
    /// <summary>
    /// Flag this object has been killed prematurly
    /// </summary>
    public void Kill(float killedTime) {
        dead = true;
        actualdDeathTime = killedTime;
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

    }
    /// <summary>
    /// Transition to living from death (rewind)
    /// </summary>
    public virtual void Resurrect() {

    }
    /// <summary>
    /// Transition to living from prebirth
    /// </summary>
    public virtual void Birth() {

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