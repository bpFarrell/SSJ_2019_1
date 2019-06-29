using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCardTimeObject : CardTimeObject { 
    public MeshRenderer mr;
    public Collider col;

    public override void Init(IEvaluable parent, CardEffect effect) {
        base.Init(parent, effect);
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * (effect.shotSpeed * BASESHOTSPEEDSCALE) * t; };
        RotateThatBitch();
        if (t < 0)
        {
            Show(false);
            isBeforeBirth = true;
        }
    }
    void RotateThatBitch()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }
    public override void Show(bool show) {
        mr.enabled = show;
        col.enabled = show;
    }
}
