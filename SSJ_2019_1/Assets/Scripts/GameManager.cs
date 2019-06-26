using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
[System.Flags]
public enum GameState
{
    /// <summary>
    /// Is on Title Screen
    /// </summary>
    TITLE           = 0x00,  
    /// <summary>
    /// Full controll
    /// </summary>
    CARD_SELECT     = 0x01,  
    /// <summary>
    /// Simulating on turn confirm
    /// </summary>
    SIMULATE_PLAY   = 0x02,  
    /// <summary>
    /// Animation for on death inside CARD_SELECT
    /// </summary>
    IS_DYING        = 0x04,  
    /// <summary>
    /// ?? Maybe a reason, I don't remember
    /// </summary>
    PLAY            = 0x08,
    /// <summary>
    /// Final animation
    /// </summary>
    END             = 0x10,
    /// <summary>
    /// Credits
    /// </summary>
    CREDITS         = 0x20,

    /// <summary>
    /// A flag when the state is in the menu
    /// </summary>
    FLAG_IS_MENU = TITLE|CREDITS,

}
public delegate void StateChange(GameState old, GameState now);
public delegate void CardUse(CardDefinition card);
public delegate void TurnComplete();
public class GameManager : MonoBehaviour {
    public static float time = 0;
    public float _time;
    public float turnLength = 5;
    public int turnNumber = 0;
    bool isRewindingForSimulation;
    public float turnStartTime {
        get { return ((float)turnLength) * (turnNumber-1); }
    }
    public float timeIntoTurn {
        get { return time - turnStartTime; }
    }
    public float percentThroughTurn {
        get { return timeIntoTurn / turnLength; }
    }
    public bool isAtEndOfTurn {
        get { return time == turnNumber * turnLength; }
    }
    public static GameManager instance;
    public GameState state = GameState.TITLE;
    public StateChange OnStateChange;
    public CardUse OnCardInvoke;
    public TurnComplete OnTurnComplete;
    public TurnComplete OnNewTurn;
    public float playerSpeed = 2;
    public float rewindSpeed = 4;
    public bool playTurn;
    void Awake() {
        instance = this;
    }
    private void Start() {
        ChangeState(GameState.CARD_SELECT);
    }
    public static void ChangeState(GameState newState) {
        if (newState == instance.state) return;
        GameState old = instance.state;
        instance.state = newState;
        Debug.Log("GameManger: changing state from: " + old + ", to " + newState);
        if (instance.OnStateChange != null) {
            instance.OnStateChange(old, newState);
        }
    }
    private void Update() {

        //if (time > turnNumber * turnLength) {
        //    turnNumber++;
        //    Debug.Log("started turn " + turnNumber);
        //    if (OnTurnComplete != null)
        //        OnTurnComplete();
        //    if (OnNewTurn != null)
        //        OnNewTurn();
        //}
        //time = Mathf.Clamp(time,12, 22);
        _time = time;
        Shader.SetGlobalFloat("_T", time);
        switch (state) {
            case GameState.TITLE:
                break;
            case GameState.CARD_SELECT:
                InCardSelect();
                break;
            case GameState.SIMULATE_PLAY:
                InSimulate();
                break;
            case GameState.IS_DYING:
                break;
            case GameState.PLAY:
                break;
            case GameState.END:
                break;
            case GameState.CREDITS:
                break;
            default:
                break;
        }
    }
    void PlayTurn() {
        ChangeState(GameState.SIMULATE_PLAY);
        //time = turnStartTime;

        isRewindingForSimulation = true;
    }
    private void InCardSelect() {
        Player player = ReInput.players.GetPlayer(0);
        float rewind = player.GetAxis("Rewind");
        float fastf = player.GetAxis("Fastforward");
        time += Mathf.Pow(fastf, 4) * Time.deltaTime * 10;
        time -= Mathf.Pow(rewind, 4) * Time.deltaTime * 10;
        time = Mathf.Clamp(time, (turnNumber - 1) * turnLength, turnNumber * turnLength);

        if (player.GetButton("Start")) {
            PlayTurn();
        }
    }
    private void InSimulate() {
        float tempTime;
        if (isRewindingForSimulation) {
            tempTime = time - Time.deltaTime * rewindSpeed;

            if (tempTime < turnStartTime) {
                time = turnStartTime;
                isRewindingForSimulation = false;
            } else {
                time = tempTime;
            }
        } else {
            tempTime = time + Time.deltaTime*playerSpeed;
            if (tempTime > turnStartTime + turnLength) {
                StartNewTurn();
            } else {
                time = tempTime;
            }
        }
    }
    private void StartNewTurn() {
        ChangeState(GameState.CARD_SELECT);
        turnNumber++;
        time = turnStartTime + Mathf.Epsilon;
        Debug.Log("started turn " + turnNumber);
        if (OnTurnComplete != null)
            OnTurnComplete();
        if (OnNewTurn != null)
            OnNewTurn();
    }
}