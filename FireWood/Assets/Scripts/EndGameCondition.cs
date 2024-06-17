using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EndGameCondition : MonoBehaviour
{
    private PlayerScript player;
    public enum EndGameState
    {
        Safe,
        Danger,
        Dead,
        End
    }
    private EndGameState state;
    [SerializeField]
    private float bufferTime = 10;

    private float counter = 0;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EndGameState.Safe:
                Safe();
                break;
            case EndGameState.Danger:
                Danger();
                break;
            case EndGameState.Dead:
                Dead();
                break;
        }
    }

    void Safe()
    {
        player = player ? player : FindObjectOfType<PlayerScript>();
        counter = 0;
        if (!player.Light.IsLit) state = EndGameState.Danger;
    }
    void Danger()
    {
        player = player ? player : FindObjectOfType<PlayerScript>();
        counter += Time.deltaTime;
        if (player.Light.IsLit) state = EndGameState.Safe;
        else if (counter >= bufferTime) state = EndGameState.Dead;
    }
    void Dead()
    {
        player = player ? player : FindObjectOfType<PlayerScript>();
        player.gameObject.SetActive(false);
        ServiceManager.Instance.Get<OnEndOfGame>().Invoke();
        state = EndGameState.End;
    }
}
