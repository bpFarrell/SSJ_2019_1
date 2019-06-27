using System.Collections.Generic;
using UnityEngine;

public class CardEffectTurnManager {
    public List<KeyValuePair<float, CardEffect>> projectileEffects = new List<KeyValuePair<float, CardEffect>>();
    public List<List<CardEffect>> playerEffects = new List<List<CardEffect>>();
    public List<List<CardEffect>> handEffects = new List<List<CardEffect>>();

    public CardEffectTurnManager() { }

    private float time { get { return GameManager.time; } }
    private float lastEvaluated = 0f;
    public float evalStep { get { return GameManager.instance.turnLength / 10; } }

    public void Init() {
        GameManager.instance.OnTurnComplete += TurnComplete;
        GameManager.instance.OnNewTurn += NewTurn;
    }

    public void Cleanup() {
        GameManager.instance.OnTurnComplete -= TurnComplete;
        GameManager.instance.OnNewTurn -= NewTurn;
    }

    public void Process() {
        if (time < lastEvaluated) {
            lastEvaluated -= evalStep;
        }
        if (time >= lastEvaluated + evalStep) {
            lastEvaluated += evalStep;

            ProcessProjectiles();
        }
    }

    public void ProcessProjectiles() {
        for (int i = 0; i < projectileEffects.Count; i++) {
            if (projectileEffects[i].Value == null) continue;
            if (projectileEffects[i].Key != lastEvaluated) continue;

            EffectToTimeObject(projectileEffects[i].Value);
        }
    }

    public void EffectToTimeObject(CardEffect effect) {
       for (int i = 0; i < effect.shotCount; i++) {
            float angle = (effect.shotSpread / effect.shotCount) * i;
            CardTimeObject obj = GameObject.Instantiate<CardTimeObject>(effect.def.projectilePrefab, GameManager.instance.transform);
            obj.dir = new Vector3(
                -Mathf.Sin(angle),
                -Mathf.Cos(angle),
                0) * 2;
            obj.scheduledDeathTime = 5f;
            obj.spawnTime = lastEvaluated - 0.1f;
            obj.Init(GameManager.instance.player.evaluable, effect);
        }
    }

    public void Processplayer() {

    }

    public void Processhand() {

    }

    private void NewTurn() {
        
    }

    public void TurnComplete() {

    }
}
