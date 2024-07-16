using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOutMessages : MonoBehaviour
{
    [SerializeField]
    private GameObject[] messages;
    // Start is called before the first frame update
    void Start()
    {
        ServiceManager.Instance.Get<OnEndGameChanged>().Subscribe(HandleEndGameChanged);
    }

    private void HandleEndGameChanged(EndGameArgs args)
    {
        if (args.state != EndGameCondition.EndGameState.Danger) return;
        if (args.completion > float.Epsilon) return;
        var randomMessange = messages.GetRandom();
        ServiceManager.Instance.Get<OnNotification>().Invoke(randomMessange);
    }

    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnEndGameChanged>().Unsubscribe(HandleEndGameChanged);
    }
}
