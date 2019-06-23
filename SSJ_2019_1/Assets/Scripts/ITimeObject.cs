using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TimeState {
    Prebirth,
    Alive,
    EarlyDead,
    Dead
}
public interface ITimeObject 
{
    TimeState timeState { get; set; }
    float t { get; set; }
    float spawnTime { get; set; }
    float deathTime { get; set; }
    Vector3 startpos { get; set; }
    IEvaluable evaluable { get; set; }
    float parentAgeAtBirth { get; set; }
}
