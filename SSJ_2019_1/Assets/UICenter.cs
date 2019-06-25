using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICenter : UISingletonBehaviour<UICenter>, ISelectHandler {
    public Selectable selectable;
    public void OnSelect(BaseEventData eventData) {
        UIHandManager.instance.hidden = true;
    }
}
