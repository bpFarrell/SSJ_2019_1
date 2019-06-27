using UnityEngine;

public class CardTimeObject : TimeObject {
    public CardEffect effect;

    public MeshRenderer mr;
    public Collider col;
    public void Init(IEvaluable parent, CardEffect effect) {
        Load(effect);
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * t; };
        RotateThatBitch();
    }
    protected void Load(CardEffect effect) {
        this.effect = effect;
    }
    void Update() {
        TimeUpdate();
        if (dead) return;
    }
    void RotateThatBitch() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }
    public override void BeforeBirth() {
        Destroy(gameObject);
    }
    public override void Resurrect() {
        mr.enabled = true;
        col.enabled = true;
    }
    public override void Kill(float globalKillTime) {
        base.Kill(globalKillTime);
        mr.enabled = false;
        col.enabled = false;
    }
}
