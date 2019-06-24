using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public enum GameState
{
    TITLE,
    CARD_SELECT,
    SIMULATE_PLAY,
    PLAY,
    END,
}
public delegate void StateChange(GameState old, GameState now);
public delegate void CardUse(/*CardPrefab*/);
public delegate void TurnComplete();
public class GameManager : MonoBehaviour {
    public static float time = 0;
    public float _time;
    public float turnLength = 5;
    public int turnNumber = 0;
    public static GameManager instance;
    public GameState state = GameState.TITLE;
    public StateChange OnStateChange;
    public CardUse OnCardInvoke;
    public TurnComplete OnTurnComplete;
    void Awake() {
        instance = this;
    }
    public static void ChangeState(GameState newState) {
        if (newState == instance.state) return;
        GameState old = instance.state;
        instance.state = newState;
        if (instance.OnStateChange != null) {
            instance.OnStateChange(old, newState);
        }
    }
    private void Update() {
        _time = time;
        Shader.SetGlobalFloat("_T", time);
        Player player = ReInput.players.GetPlayer(0);
        float rewind = player.GetAxis("Rewind");
        float fastf = player.GetAxis("Fastforward");
        time += Mathf.Pow(fastf,4) * Time.deltaTime*10;
        time -= Mathf.Pow(rewind,4) * Time.deltaTime*10;
        time = Mathf.Max((turnNumber-1) * turnLength, time);
        if (time > turnNumber * turnLength) {
            turnNumber++;
            Debug.Log("started turn " + turnNumber);
            if (OnTurnComplete != null)
                OnTurnComplete();
        }
        //time = Mathf.Clamp(time,12, 22);
    }
}