using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeObject 
{
    float t { get; set; }
    float spawnTime { get; set; }
    float deathTime { get; set; }
    Vector3 startpos { get; set; }
    IEvaluable evaluable { get; set; }
}
