using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : TimeObject
{
    public MeshRenderer mr;
    public Collider col;
    public void Init(IEvaluable parent) {
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * t; };
        col = GetComponent<Collider>();
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
        col.enabled = true;
    }
    public override void Kill(float gloablKillTime) {
        base.Kill(gloablKillTime);
        mr.enabled = false;
        col.enabled = false;
    }

}
