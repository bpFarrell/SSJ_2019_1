using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectTurnManager {
    public List<KeyValuePair<float, CardEffect>> projectileEffects = new List<KeyValuePair<float, CardEffect>>();
    public List<List<CardEffect>> playerEffects = new List<List<CardEffect>>();
    public List<List<CardEffect>> handEffects = new List<List<CardEffect>>();

    public List<CardTimeObject> timeObjectList = new List<CardTimeObject>();

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
        projectileEffects.Clear();
        playerEffects.Clear();
        handEffects.Clear();
        timeObjectList.Clear();
    }

    public void Process() {
        if (time < lastEvaluated + Mathf.Epsilon) {
            lastEvaluated -= evalStep;
        }
        if (time >= lastEvaluated + evalStep + Mathf.Epsilon) {
            lastEvaluated += evalStep;

            ProcessProjectiles();
        }
    }

    public void ProcessProjectiles() {
        for (int i = projectileEffects.Count - 1; i >= 0; i--) {
            if (projectileEffects[i].Value == null) {
                projectileEffects.RemoveAt(i);
                continue;
            }
            if (projectileEffects[i].Key != lastEvaluated) continue;

            EffectToTimeObject(projectileEffects[i].Key, projectileEffects[i].Value);
        }
    }

    public void EffectToTimeObject(float spawnTime, CardEffect effect) {
        int length = (int)effect.shotFrequency * effect.def.cost;
        float freqIncrement = (float)effect.def.cost / (float)length;
        for (int r = 0; r < length; r++) {
            float freqDelta =  (freqIncrement * r) * UISkillBar.tabTimeIncrement;

            float angleStart = -effect.shotSpread;
            float totalAngle = effect.shotSpread * 2;
            float angleIncrement = totalAngle / (effect.shotCount - 1);
            for (int i = 0; i < effect.shotCount; i++)
            {
                CardTimeObject obj = GameObject.Instantiate<CardTimeObject>(effect.def.projectilePrefab, GameManager.instance.transform);

                if (effect.shotCount > 1) {
                    float angle = angleStart + (angleIncrement * i);
                    obj.dir = new Vector3(
                        Mathf.Cos(Mathf.Deg2Rad * angle),
                        -Mathf.Sin(Mathf.Deg2Rad * angle),
                        0);
                } else {
                    obj.dir = Vector3.right;
                }

                obj.scheduledDeathTime = 5f;
                obj.spawnTime = freqDelta + spawnTime + ((float)i) * 0.05f;
                obj.prebirthSpawnTime = spawnTime + Mathf.Epsilon;
                obj.Init(GameManager.instance.player.evaluable, effect);
                timeObjectList.Add(obj);
            }
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

    internal void AddEffect(float v, CardEffect effect) {
        projectileEffects.Add(new KeyValuePair<float, CardEffect>(v, effect));
        EffectToTimeObject(v, effect);
    }
    internal void RemoveEffect(CardDefinition def) {
        for (int i = projectileEffects.Count - 1; i >= 0; i--) {
            if (projectileEffects[i].Value == null) {
                projectileEffects.RemoveAt(i);
                continue;
            }
            if (projectileEffects[i].Value.def != def) continue;

            for (int obj = timeObjectList.Count - 1; obj >= 0; obj--) {
                if(timeObjectList[obj] == null) {
                    timeObjectList.RemoveAt(obj);
                    continue;
                }
                if(timeObjectList[obj].effect == projectileEffects[i].Value)
                timeObjectList[obj].TotalCleanup();
            }

            projectileEffects.RemoveAt(i);
        }
    }
    
}
