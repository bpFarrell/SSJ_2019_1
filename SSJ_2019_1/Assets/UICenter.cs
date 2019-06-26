using Rewired;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICenter : UISingletonBehaviour<UICenter>, ISelectHandler {
    public Selectable selectable;
    public void OnSelect(BaseEventData eventData) {
        UIManager.handManager.hidden = true;
    }
    public void Update() {
        if(EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        Player player = ReInput.players.GetPlayer(0);
        if (player.GetButtonDown("XButton")) {
            UIManager.handManager.ReceiveCard(CardResourceHandler.GetRandomCard());
        }
        if (player.GetButtonDown("YButton")) {
            UIManager.handManager.RemoveCards();
        }
    }
}
