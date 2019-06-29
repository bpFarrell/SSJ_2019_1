using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTimeObject : CardTimeObject {
    public ParticleSystem particle;

    public override void Init(IEvaluable parent, CardEffect effect) {
        base.Init(parent, effect);
        evaluable = new IEvaluable();
        evaluable.eval = (t) => {
            return parent.eval(GameManager.time - 0.09f);
        };
        //particle.Pause(true);
        if (t < 0) {
            Show(false);
            isBeforeBirth = true;
        }
    }
    public override void Update() {
        base.Update();
        if (particle == null) return;
        //particle.time = GameManager.time;
        particle.Simulate(GameManager.time,true,true);
    }
    public override void Show(bool show) {
        particle.gameObject.SetActive(show);
    }
    public void OnTriggerEnter(Collider other) {
        TimeObject to = other.GetComponent<TimeObject>();
        if (to != null)
            to.Kill(GameManager.time);
    }
}
