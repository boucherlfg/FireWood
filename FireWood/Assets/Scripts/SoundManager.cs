using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    GameState gameState;
    private void Start()
    {
        gameState = ServiceManager.Instance.Get<GameState>();
        gameState.Volume.Changed += Volume_Changed;
    }

    private void Volume_Changed(float oldValue, float newValue)
    {
        AudioListener.volume = newValue;
    }
}
