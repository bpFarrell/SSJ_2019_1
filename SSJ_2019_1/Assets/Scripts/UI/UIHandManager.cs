using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHandManager : MonoBehaviour {
    // Debug
    public int selectionIndex;
    
    //
    private List<Card> cardList = new List<Card>();
    public Card cardPrefab;

    private void Awake() {
        
    }

    public void ReceiveCard(CardDefinition def) {
        if (def == null || cardPrefab == null) return;

        Card c = Instantiate(cardPrefab, transform);
        c.load(def);
        c.handIndex = cardList.Count;
        cardList.Add(c);

        Reorder();
    }

    private void Reorder() {
        
    }
}
