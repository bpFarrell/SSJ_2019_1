using System;
using System.Collections.Generic;
using UnityEngine;

public class UISkillBar : UIMonoBehaviour {
    [SerializeField]
    private SkillTab tabPrefab;

    private const float TOTALWIDTH = 1440;
    public const int TOTALSLOTS = 10;
    public static float costCoefficient { get { return TOTALWIDTH / TOTALSLOTS; } }
    public static float tabTimeIncrement { get { return (GameManager.instance.turnLength / TOTALSLOTS); }
    }

    public SkillTab[] tabList = new SkillTab[TOTALSLOTS];

    private static float GetWidthFromCost(int cost) {
        return costCoefficient * cost;
    }

    internal void Init() {
        GameManager.instance.OnCardInvoke += CardInvoked;
        GameManager.instance.OnTurnComplete += TurnComplete;
        GameManager.instance.OnNewTurn += NewTurn;
    }

    private void TurnComplete() {
        for (int i = tabList.Length - 1; i >= 0; i--) {
            if (tabList[i] == null) continue;
            tabList[i].Cleanup();
            tabList[i] = null;
        }
    }

    private void NewTurn() {
        
    }

    private void CardInvoked(CardDefinition def)
    {
        throw new NotImplementedException();
    }


    public int PlaceTab(CardDefinition def) {
        return PlaceTab(def, 0f);
    }

    internal void SetHighlight(CardDefinition definition) {
        for (int i = 0; i < TOTALSLOTS; i++) {
            if (tabList[i] == null) continue;
            if (definition == tabList[i].definition){
                tabList[i].highlit = true;
            } else {
                tabList[i].highlit = false;
            }
        }
    }

    public int PlaceTab(CardDefinition def, float localTime) {
        for (int i = 0; i < tabList.Length; i++) {
            if (tabList[i] == null) continue;
            if (tabList[i].definition.GetHashCode() == def.GetHashCode()) {
                return -1;
            }
        }
        int slot = nextAvailableIndex(def.cost, LocalTimeToSlot(localTime));
        if (slot == -1) {
            return -1;
        }
        SkillTab tab = Instantiate(tabPrefab, transform);
        tab.Load(def);
        MoveToSlot(tab, slot);
        // Timey Whimy Stuff
        //GameManager.time = SlotToGT(slot) - 0.2f;

        return slot;
    }

    public void RemoveTab(CardDefinition def) {
        for (int i = 0; i < TOTALSLOTS; i++) {
            if (tabList[i] == null) continue;
            if (tabList[i].definition == def) {
                tabList[i].Cleanup();
            }
        }
    }

    public static float SlotToGT(int slot) {
        return (slot * (GameManager.instance.turnLength / GameManager.instance.turnStepCount)) + ((GameManager.instance.turnNumber-1) * GameManager.instance.turnLength);
    }

    public static int LocalTimeToSlot(float localTime) {
        return (int)Mathf.Floor(localTime * TOTALSLOTS);
    }

    public void MoveToSlot(SkillTab tab, int index) {
        tab.rectTransform.anchoredPosition = new Vector2(index * costCoefficient, tab.rectTransform.anchoredPosition.y);
        for (int i = 0; i < tab.definition.cost; i++) {
            tabList[index+i] = tab;
        }
    }

    internal void Cleanup() {
        GameManager.instance.OnCardInvoke -= CardInvoked;
        GameManager.instance.OnTurnComplete -= TurnComplete;
        GameManager.instance.OnNewTurn -= NewTurn;
    }

    internal void Process() {
        
    }

    public int nextAvailableIndex(int cost) {
        return nextAvailableIndex(cost, 0);
    }
    public int nextAvailableIndex(int cost, int start) {
        start = Mathf.Clamp(start, 0, TOTALSLOTS);
        int startNull = -1;
        for (int index = start; index < TOTALSLOTS; index++)
        {
            if (tabList[index] != null)
            {
                startNull = -1;
                continue;
            }
            if (cost == 1) return index;
            if (startNull == -1)
            {
                startNull = index;
                continue;
            }
            if (index - startNull + 1 == cost)
            {
                return startNull;
            }
        }
        return -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Vector3 first = (Vector3)(MainCanvas.transform.localScale * rectTransform.rect.min) + rectTransform.position;
        first += Vector3.up * (rectTransform.rect.size.y / 2);
        for (int i = 0; i < TOTALSLOTS; i++) {
            if(tabList[i] != null)
                Gizmos.DrawSphere(first + (MainCanvas.transform.localScale.x * (Vector3.right * costCoefficient * i)), 20f);
        }
    }

    internal bool cardSelected(CardDefinition definition) {
        for (int i = 0; i < TOTALSLOTS; i++) {
            if (tabList[i] == null) continue;
            if (tabList[i].definition == definition) return true;
        }
        return false;
    }
}