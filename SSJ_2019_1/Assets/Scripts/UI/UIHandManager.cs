﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHandManager : Parabola {
    public int selectionIndex = -1;
    public int handSize = 6;
    
    public List<Card> cardList = new List<Card>();
    public Card cardPrefab;

    public override int steps { get { return cardList.Count; } }

    internal void Init() {
        GenerateHand();
        GameManager.instance.OnNewTurn += NewTurn;
        GameManager.instance.OnTurnComplete += TurnComplete;
    }

    private void NewTurn() {
        GenerateHand();
    }

    private void GenerateHand() {
        for (int i = 0; i < handSize; i++) {
            ReceiveCard(CardResourceHandler.GetRandomCard());
        }
    }

    private void TurnComplete() {
        RemoveCards();
    }

    internal void Cleanup() {
        RemoveCards();
    } 

    // Hand Management
    public void ReceiveCards(IEnumerable<CardDefinition> defs) {
        foreach (CardDefinition def in defs) {
            ReceiveCard(def);
        }
    }
    public void ReceiveCard(CardDefinition def) {
        if (def == null || cardPrefab == null) return;

        Card c = Instantiate(cardPrefab, transform);
        c.Load(def);
        c.handIndex = cardList.Count;
        cardList.Add(c);

        int index = cardList.Count - 1;
        if (index - 1 != -1) {
            cardList[index - 1].SetRight(cardList[index].selectable);
            cardList[index].SetLeft(cardList[index - 1].selectable);
        }

        cardList[  0  ].SetLeft( cardList[index].selectable);
        cardList[index].SetRight(cardList[  0  ].selectable);
        

        width = (int)((steps * 150) * widthHiddenScalar);

        Reorder();
    }

    internal void Process()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            a_targetPosition,
            0.3f);
        for (int i = steps - 1; i >= 0; i--)
        {
            if (cardList[i].handIndex == selectionIndex && !hidden)
            {
                cardList[i].select(true);
            }
            else
            {
                cardList[i].select(false);
            }
            cardList[i].transform.SetSiblingIndex(cardList[i].handIndex);
        }
        if (!hidden && selectionIndex >= 0 && selectionIndex < steps) cardList[selectionIndex].transform.SetAsLastSibling();
    }

    internal static void CardTriggered(CardDefinition definition) {
        int slot = UIManager.skillManager.PlaceTab(definition, GameManager.instance.percentThroughTurn);
        if (slot == -1) return;
        CardEffect effect = new CardEffect();
        effect.Load(definition);
        UIManager.CETurnManager.AddEffect(UISkillBar.SlotToGT(slot), effect);
    }

    public void RemoveCards() {
        selectionIndex = -1;
        for (int i = steps - 1; i >= 0; i--)
        {
            // Discard Pile ?
            cardList[i].Cleanup();
            cardList.RemoveAt(i);
        }
    }
    
    private Vector2 a_targetPosition;
    private readonly Vector2 a_HIDDENPOSTITION = new Vector3(0f,-110f,0f);
    private readonly Vector2 a_SHOWPOSITION = new Vector3(0f, 30f, 0f);
    private float widthHiddenScalar = 1f;
    private const float HIDDENSCALARLOW = 0.1f;

    private bool _hidden;
    public bool hidden
    {
        get { return _hidden; }
        set
        {
            if (_hidden == value) return;
            _hidden = value;
            RetargetHidden();
        }
    }

    private void RetargetHidden() {
        a_targetPosition = hidden ? a_HIDDENPOSTITION : a_SHOWPOSITION;
        widthHiddenScalar = hidden ? HIDDENSCALARLOW : 1f;
        width = (int)((steps * 150) * widthHiddenScalar);
        selectionIndex = -1;
        Reorder();
    }
    //

    // Movement / ordering
    public void Reorder() {
        Card.Pose temp = new Card.Pose();
        for (int i = steps-1; i >= 0; i--) {
            if (cardList[i] == null) {
                cardList.RemoveAt(i);
                continue;
            }

            cardList[i].handIndex = i;

            temp = cardList[i].handPose;
            temp.position = GetPositionAtIndex(i);
            temp.rotation = Quaternion.LookRotation(Vector3.forward, GetNormalAtIndex(i));
            if(steps == 1) {
                temp.pivot = new Vector2(0.5f, 0.5f);
            } else {
                float range = (float)(i) / (float)(steps-1);
                temp.pivot = new Vector2 (1-range, .5f + (Mathf.Abs((range-.5f)*2) * .02f));
            }
            cardList[i].handPose = temp;
            
            cardList[i].Update();
        }
    }
}
