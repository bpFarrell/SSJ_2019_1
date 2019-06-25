using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : TimeObject
{
    public MeshRenderer mr;
    public void Init(IEvaluable parent) {
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * t; };
        //mr = GetComponent<MeshRenderer>();
    }   
    void Update()
    {
        TimeUpdate();
        if (dead) return;
    }
    public override void BeforeBirth() {
        Destroy(gameObject);
    }
    public override void Resurrect() {
        mr.enabled = true;
    }

}
