using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTab : UIMonoBehaviour {
    // Visual representation Only!!!
    public CardDefinition definition;

    [SerializeField]
    private Image tabColor;
    [SerializeField]
    private Image handleColor;

    private float costCoeficient { get { return UISkillBar.costCoefficient; } }

    public void Load(CardDefinition def) {
        definition = def;
        switch (definition.type) {
            case CardDefinition.TYPE.POWER:
                tabColor.color = UIManager.instance.powerPallet.primary;
                handleColor.color = UIManager.instance.powerPallet.primary;
                break;
            case CardDefinition.TYPE.MOVEMENT:
                tabColor.color = UIManager.instance.movementPallet.primary;
                handleColor.color = UIManager.instance.movementPallet.primary;
                break;
            case CardDefinition.TYPE.UTILITY:
                tabColor.color = UIManager.instance.utilityPallet.primary;
                handleColor.color = UIManager.instance.utilityPallet.primary;
                break;
        }
        SetSize(def.cost);
    }

    public void Cleanup() {
        Destroy(gameObject);
    }

    public void SetSize(int cost) {
        rectTransform.sizeDelta = new Vector2(cost * costCoeficient, rectTransform.sizeDelta.y);
    }
}
