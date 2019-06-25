using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandManager : Parabola<UIHandManager> {
    public int selectionIndex = -1;
    
    public List<Card> cardList = new List<Card>();
    public Card cardPrefab;

    public override int steps { get { return cardList.Count; } }

    private void Awake() {
        
    }

    // Hand Management
    public void ReceiveCards(CardDefinition[] defs) {
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

        int i = cardList.Count - 1;
        if (i - 1 >= 0)
        {
            cardList[i - 1].SetRight(cardList[i].selectable);
            cardList[i].SetLeft(cardList[i - 1].selectable);
        }

        width = (int)((steps * 150) * widthHiddenScalar);

        Reorder();
    }
    public void RemoveCards() {

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

    private void Update() {
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            a_targetPosition,
            0.3f);
        for (int i = steps - 1; i >= 0; i--) {
            if (cardList[i].handIndex == selectionIndex && !hidden) {
                cardList[i].select(true);
            } else {
                cardList[i].select(false);
            }
            cardList[i].transform.SetSiblingIndex(cardList[i].handIndex);
        }
        if(!hidden && selectionIndex >= 0 && selectionIndex < steps) cardList[selectionIndex].transform.SetAsLastSibling();
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
