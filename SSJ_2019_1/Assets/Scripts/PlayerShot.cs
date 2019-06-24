using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : TimeObject
{
    public void Init(IEvaluable parent) {
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * t; };

    }   
    void Update()
    {
        TimeUpdate();
    }
    public override void BeforeBirth() {
        Destroy(gameObject);
    }

}
