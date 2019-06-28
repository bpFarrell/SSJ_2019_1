using UnityEngine;

public class CardTimeObject : TimeObject {
    public CardEffect effect;

    public MeshRenderer mr;
    public Collider col;
    public bool isBeforeBirth;

    public void Init(IEvaluable parent, CardEffect effect) {
        Load(effect);
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + dir * t; };
        RotateThatBitch();
        if (t < 0) {
            Show(false);
            isBeforeBirth = true;
        }
    }
    protected void Load(CardEffect effect) {
        this.effect = effect;
    }
    void Update() {
        TimeUpdate();
        if (t > 0 && isBeforeBirth) {
            Show(true);
            isBeforeBirth = false;
        }
        if(t < 0 && !isBeforeBirth) {
            Show(false);
            isBeforeBirth = true;
        }
        if (dead) return;
    }
    void RotateThatBitch() {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
    }
    public override void BeforeBirth() {
        Destroy(gameObject);
    }
    public override void Resurrect() {
        Show(true);
    }
    public override void Kill(float globalKillTime) {
        base.Kill(globalKillTime);
        Show(false);
    }
    void Show(bool show) {
        mr.enabled = show;
        col.enabled = show;
    }
}
