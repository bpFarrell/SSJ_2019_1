using Rewired;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICenter : UISingletonBehaviour<UICenter>, ISelectHandler {
    public Selectable selectable;
    public void OnSelect(BaseEventData eventData) {
        UIManager.handManager.hidden = true;
        UIManager.skillManager.SetHighlight(null);
    }
    private void Start() {
        GameManager.instance.OnStateChange += StateChange;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    private void OnDisable() {
        GameManager.instance.OnStateChange -= StateChange;
    }
    public void Update() {
        Player player = ReInput.players.GetPlayer(0);
        if (EventSystem.current.currentSelectedGameObject == null && GameManager.instance.state == GameState.CARD_SELECT && player.GetAxis("DVertical") < 0) {
            EventSystem.current.SetSelectedGameObject(selectable.FindSelectableOnDown().gameObject);
        }
        // Temp
        //if (player.GetButtonDown("XButton")) {
        //    UIManager.handManager.ReceiveCard(CardResourceHandler.GetRandomCard());
        //}
        //if (player.GetButtonDown("YButton")) {
        //    UIManager.handManager.RemoveCards();
        //}
    }
    private void StateChange(GameState old, GameState now) {
        if (now == GameState.CARD_SELECT) {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        if (now != GameState.CARD_SELECT) {
            EventSystem.current.SetSelectedGameObject(null);
            UIManager.handManager.hidden = true;
            UIManager.skillManager.SetHighlight(null);
        }
    }
    public static void DeselectAll() {
        EventSystem.current.SetSelectedGameObject(instance.gameObject);
    }
}
