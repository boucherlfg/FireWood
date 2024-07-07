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

    private void HandleStormChanged(StormChangedArgs args)
    {
        if (args.newState != Storm.StormState.Transition) return;
        if ((args.lastState & (Storm.StormState)1) != 0) return;

        var randomMessange = messages.GetRandom();
        ServiceManager.Instance.Get<OnNotification>().Invoke(randomMessange);
    }

    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(HandleStormChanged);
    }
}
