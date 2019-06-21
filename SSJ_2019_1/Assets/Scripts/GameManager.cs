using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state = GameState.TITLE;
    public StateChange OnStateChange;
    public CardUse OnCardInvoke;
    void Awake()
    {
        instance = this;
    }
    public static void ChangeState(GameState newState)
    {
        if (newState == instance.state) return;
        GameState old = instance.state;
        instance.state = newState;
        if (instance.OnStateChange != null)
        {
            instance.OnStateChange(old,newState);
        }
    }

}
