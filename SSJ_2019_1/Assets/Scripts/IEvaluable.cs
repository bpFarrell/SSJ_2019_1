using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Vector3 EvalMethod(float t);
public class IEvaluable 
{
    public EvalMethod eval;
}
