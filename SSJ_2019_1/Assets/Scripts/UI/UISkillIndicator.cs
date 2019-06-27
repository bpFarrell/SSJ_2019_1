using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillIndicator : UIMonoBehaviour {
    private void Update() {
        rectTransform.anchoredPosition = new Vector2(GameManager.instance.percentThroughTurn * 1440f, rectTransform.anchoredPosition.y);
        transform.SetAsLastSibling();
    }
}
