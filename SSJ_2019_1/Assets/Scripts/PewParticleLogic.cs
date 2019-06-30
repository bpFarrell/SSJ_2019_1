using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewParticleLogic : TimeObject {
    public GameObject bg;
    public GameObject fg;
    public Vector3 offset;
    public float rotAmount;
    public float scalar = 1;
    public bool toTheRight;
    // Start is called before the first frame update
    void Start() {
        rotAmount = Random.Range(-100, 100);
        offset = Random.insideUnitSphere + (toTheRight?Vector3.right:Vector3.zero) * 2.5f;
    }

    public void Init(IEvaluable parent) {
        evaluable = new IEvaluable();
        evaluable.eval = (t) => { return parent.eval(spawnTime) + offset + dir * t; };
        scheduledDeathTime = 0.2f;
    }
    void Update() {
        fg.transform.eulerAngles = Vector3.forward * (t * rotAmount + rotAmount * -0.5f);
        bg.transform.eulerAngles = Vector3.forward * (t * -rotAmount + rotAmount * 0.5f);
        transform.localScale = Vector3.one * t * 10 * scalar;
        TimeUpdate();
    }
    public override void AfterNaturalDeath() {
        bg.SetActive(false);
        fg.SetActive(false);
    }
    public override void Resurrect() {
        bg.SetActive(true);
        fg.SetActive(true);
    }
    public override void BeforeBirth() {
        Destroy(gameObject);
    }
    public static PewParticleLogic PlacePew(IEvaluable parent, float birthTime, float? prebirthSpawnTime) {
        PewParticleLogic ppl = Instantiate(Resources.Load<PewParticleLogic>("Pew")) as PewParticleLogic;
        ppl.Init(parent);
        ppl.spawnTime = birthTime;
        ppl.prebirthSpawnTime = prebirthSpawnTime;
        return ppl;
    }

    public static PewParticleLogic PlaceBoom(IEvaluable parent, float birthTime, float? prebirthSpawnTime) {
        PewParticleLogic ppl = Instantiate(Resources.Load<PewParticleLogic>("Boom")) as PewParticleLogic;
        ppl.Init(parent);
        ppl.spawnTime = birthTime;
        ppl.prebirthSpawnTime = prebirthSpawnTime;
        ppl.scheduledDeathTime = 0.5f;
        return ppl;

    }
}
