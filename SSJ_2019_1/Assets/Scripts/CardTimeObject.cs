using UnityEngine;

public class CardTimeObject : TimeObject {
    public CardEffect effect;

    public MeshRenderer mr;
    public Collider col;
    public void Init(IEvaluable parent, CardEffect details) {
        Load(details);
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * t; };
    }
    protected void Load(CardEffect details) {
        effect = details;
    }
    void Update() {
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
    public override void Kill(float globalKillTime) {
        base.Kill(globalKillTime);
        mr.enabled = false;
        col.enabled = false;
    }
}
