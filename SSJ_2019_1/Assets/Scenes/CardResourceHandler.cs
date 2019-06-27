using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardResourceHandler : UISingletonBehaviour<CardResourceHandler> {
    [SerializeField]
    private List<CardDefinition> cardDefList = new List<CardDefinition>();

    public static CardDefinition GetRandomCard() {
        int index = Random.Range(0, instance.cardDefList.Count);
        return instance.cardDefList[index].Copy;
    }
}
