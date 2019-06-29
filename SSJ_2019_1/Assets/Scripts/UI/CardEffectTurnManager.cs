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
        playerEffects.Add(new List<CardEffect>());
        handEffects.Add(new List<CardEffect>());
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

            if (projectileEffects[i].Value.def.cClass == CardDefinition.CLASS.ATTACK) {
                EffectToTimeObject(projectileEffects[i].Key, projectileEffects[i].Value);
            }
            if (projectileEffects[i].Value.def.cClass == CardDefinition.CLASS.PET)
            {
                EffectToPetTimeObject(projectileEffects[i].Key, projectileEffects[i].Value);
            }
        }
    }

    private void EffectToPetTimeObject(float spawnTime, CardEffect effect) {
        CardTimeObject obj = GameObject.Instantiate<CardTimeObject>(effect.def.projectilePrefab, GameManager.instance.transform);

        obj.scheduledDeathTime = effect.duration * GameManager.instance.turnLength;
        obj.spawnTime = spawnTime;
        obj.prebirthSpawnTime = spawnTime - Mathf.Epsilon;
        obj.parentAgeAtBirth = GameManager.instance.player.t;
        obj.Init(GameManager.instance.player.evaluable, effect);
        timeObjectList.Add(obj);
    }

    public void EffectToTimeObject(float spawnTime, CardEffect effect) {
        List<CardEffect> effectStack = handEffects[GameManager.GlobalTimeToTurn(spawnTime)];
        float shotSpeed = effect.shotSpeed;
        float shotFrequency = effect.shotFrequency;
        int shotCount = effect.shotCount;
        float shotSpread = effect.shotSpread;
        
        for (int i = 0; i < effectStack.Count; i++) {
            //shotSpeed += effectStack[i].shotSpeed
            shotFrequency += effectStack[i].shotFrequencyModifier;
            shotCount += effectStack[i].shotCountModifier;
            shotSpread += effectStack[i].shotSpreadModifier;
        }
        for (int i = 0; i < effectStack.Count; i++) {
            //shotSpeed += effectStack[i].shotSpeed
            shotFrequency *= effectStack[i].shotFrequencyScalar;
            shotCount = (int)((float)shotCount * effectStack[i].shotCountScalar);
            shotSpread *= effectStack[i].shotSpreadScalar;
        }

        int length = (int)shotFrequency * effect.def.cost;
        float freqIncrement = (float)effect.def.cost / (float)length;
        for (int r = 0; r < length; r++)
        {
            float freqDelta = (freqIncrement * r) * UISkillBar.tabTimeIncrement;

            float angleStart = -shotSpread;
            float totalAngle = shotSpread * 2;
            float angleIncrement = totalAngle / (shotCount - 1);
            for (int i = 0; i < effect.shotCount; i++)
            {
                CardTimeObject obj = GameObject.Instantiate<CardTimeObject>(effect.def.projectilePrefab, GameManager.instance.transform);

                if (shotCount > 1)
                {
                    float angle = angleStart + (angleIncrement * i);
                    obj.dir = new Vector3(
                        Mathf.Cos(Mathf.Deg2Rad * angle),
                        -Mathf.Sin(Mathf.Deg2Rad * angle),
                        0);
                }
                else
                {
                    obj.dir = Vector3.right;
                }

                obj.scheduledDeathTime = effect.duration * GameManager.instance.turnLength;
                obj.spawnTime = freqDelta + spawnTime + ((float)i) * 0.05f;
                obj.prebirthSpawnTime = spawnTime - Mathf.Epsilon;
                obj.parentAgeAtBirth = GameManager.instance.player.t;
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
        playerEffects.Add(new List<CardEffect>());
        handEffects.Add(new List<CardEffect>());
    }

    public void TurnComplete() {

    }

    internal void AddEffect(float v, CardEffect effect) {
        switch (effect.def.cardType) {
            case CardDefinition.CARDTYPE.PROJECTILE:
                projectileEffects.Add(new KeyValuePair<float, CardEffect>(v, effect));
                //if (effect.def.cClass == CardDefinition.CLASS.ATTACK)
                //{
                //    EffectToTimeObject(v, effect);
                //}
                //if (effect.def.cClass == CardDefinition.CLASS.PET)
                //{
                //    EffectToPetTimeObject(v, effect);
                //}
                break;
            case CardDefinition.CARDTYPE.HAND:
                handEffects[GameManager.GlobalTimeToTurn(v)].Add(effect);
                break;
            case CardDefinition.CARDTYPE.PLAYER:
                playerEffects[GameManager.GlobalTimeToTurn(v)].Add(effect);
                break;
            default:
                break;
        }
    }
    internal void RemoveEffect(CardDefinition def) {
        switch (def.cardType) {
            case CardDefinition.CARDTYPE.PROJECTILE:
                RemoveProjectile(def);
                break;
            case CardDefinition.CARDTYPE.HAND:
                RemoveHandBuff(def);
                break;
            case CardDefinition.CARDTYPE.PLAYER:
                RemovePlayerBuff(def);
                break;
            default:
                break;
        }
    }

    private void RemovePlayerBuff(CardDefinition def)
    {
        for (int turn = playerEffects.Count - 1; turn >= 0; turn--) {
            for (int index = playerEffects[turn].Count - 1; index >= 0; index--) {
                if (playerEffects[turn][index] == null || playerEffects[turn][index].def == def)
                {
                    playerEffects[turn].RemoveAt(index);
                    continue;
                }
            }
        }
    }

    private void RemoveHandBuff(CardDefinition def)
    {
        for (int turn = handEffects.Count - 1; turn >= 0; turn--) {
            for (int index = handEffects[turn].Count - 1; index >= 0; index--) {
                if(handEffects[turn][index] == null || handEffects[turn][index].def == def) {
                    handEffects[turn].RemoveAt(index);
                    continue;
                }
            }
        }
    }

    internal void RemoveProjectile(CardDefinition def) {
        for (int i = projectileEffects.Count - 1; i >= 0; i--)
        {
            if (projectileEffects[i].Value == null)
            {
                projectileEffects.RemoveAt(i);
                continue;
            }
            if (projectileEffects[i].Value.def != def) continue;

            for (int obj = timeObjectList.Count - 1; obj >= 0; obj--)
            {
                if (timeObjectList[obj] == null)
                {
                    timeObjectList.RemoveAt(obj);
                    continue;
                }
                if (timeObjectList[obj].effect == projectileEffects[i].Value)
                    timeObjectList[obj].TotalCleanup();
            }

            projectileEffects.RemoveAt(i);
        }
    }
}
