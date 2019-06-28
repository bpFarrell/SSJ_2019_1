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

    private Color baseColor;
    private Color hueColor;

    private float costCoeficient { get { return UISkillBar.costCoefficient; } }

    public void Load(CardDefinition def) {
        definition = def;
        switch (definition.type) {
            case CardDefinition.TYPE.POWER:
                tabColor.color = UIManager.instance.powerPallet.primary;
                handleColor.color = UIManager.instance.powerPallet.primary;
                baseColor = UIManager.instance.powerPallet.primary;
                hueColor = UIManager.instance.powerPallet.hue;
                break;
            case CardDefinition.TYPE.MOVEMENT:
                tabColor.color = UIManager.instance.movementPallet.primary;
                handleColor.color = UIManager.instance.movementPallet.primary;
                baseColor = UIManager.instance.movementPallet.primary;
                hueColor = UIManager.instance.movementPallet.hue;
                break;
            case CardDefinition.TYPE.UTILITY:
                tabColor.color = UIManager.instance.utilityPallet.primary;
                handleColor.color = UIManager.instance.utilityPallet.primary;
                baseColor = UIManager.instance.utilityPallet.primary;
                hueColor = UIManager.instance.utilityPallet.hue;
                break;
        }
        
        SetSize(def.cost);
    }

    private Color target;
    public bool highlit;

    private void Update() {
        if (highlit) {
            target = hueColor;
        } else {
            target = baseColor;
        }
        float val = (Mathf.Sin(Time.time*15f)+1)*0.5f;
        Color col = Color.Lerp(baseColor, target, val);
        tabColor.color = col;
        handleColor.color = col;
    }

    public void Cleanup() {
        Destroy(gameObject);
    }

    public void SetSize(int cost) {
        rectTransform.sizeDelta = new Vector2(cost * costCoeficient, rectTransform.sizeDelta.y);
    }
}
