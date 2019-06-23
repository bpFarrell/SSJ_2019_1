using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour, ITimeObject {
    public Vector3 dir;

    public float t { get { return GameManager.time - spawnTime; } set { } }
    public float spawnTime { get; set; }
    public float deathTime { get; set; }
    public Vector3 startpos { get; set; }
    public IEvaluable evaluable { get; set; }
    public TimeState timeState { get ; set; }
    public float parentAgeAtBirth { get; set; }
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
    }
    private void CheckDeath() {
        if (t > deathTime && !dead) {
            AfterNaturalDeath();
        } else if (t < deathTime && dead) {
            Resurrect();
        }
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
}
