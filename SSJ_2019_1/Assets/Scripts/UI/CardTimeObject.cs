using UnityEngine;

public class CardTimeObject : TimeObject {
    public CardEffect effect;

    public bool isBeforeBirth;
    public const float BASESHOTSPEEDSCALE = 6f;

    public virtual void Init(IEvaluable parent, CardEffect effect) {
        Load(effect);
    }
    protected void Load(CardEffect effect)
    {
        this.effect = effect;
    }
    public virtual void Update()
    {
        TimeUpdate();
        if (t > 0 && isBeforeBirth)
        {
            Show(true);
            isBeforeBirth = false;
        }
        if (t < 0 && !isBeforeBirth)
        {
            Show(false);
            isBeforeBirth = true;
        }
        if (dead) return;
    }
    public override void BeforeBirth()
    {
        Destroy(gameObject);
    }
    public override void Resurrect()
    {
        Show(true);
    }
    public override void Kill(float globalKillTime)
    {
        base.Kill(globalKillTime);
        Show(false);
    }
    public virtual void Show(bool show) {
        
    }
}
