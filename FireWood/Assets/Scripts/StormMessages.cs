using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormMessages : MonoBehaviour
{
    [SerializeField]
    private GameObject[] messages;
    // Start is called before the first frame update
    void Start()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Subscribe(HandleStormChanged);
    }

    private void HandleStormChanged(Storm.StormState state)
    {
        if (state < Storm.StormState.TransitionToIngoing) return;
        if (state == Storm.StormState.TransitionToPeace) return;

        var randomMessange = messages.GetRandom();
        ServiceManager.Instance.Get<OnNotification>().Invoke(randomMessange);
    }

    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(HandleStormChanged);
    }
}
