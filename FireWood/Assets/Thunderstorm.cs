using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunderstorm : MonoBehaviour
{
    public enum StormState { First = 0, Ingoing = 1, Peace = 2 };

    private StormState state = StormState.First;
    public float timeBeforeFirstStorm = 60;
    public float intervalBetweenStorms = 30;
    public float stormDuration = 10;
    public float thunderStormMultiplier = 2;

    private float counter = 0;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        switch (state)
        {
            case StormState.First:
                Transition(timeBeforeFirstStorm, StormState.Ingoing);
                break;
            case StormState.Ingoing:
                Transition(stormDuration, StormState.Peace);
                break;
            case StormState.Peace:
                Transition(intervalBetweenStorms, StormState.Ingoing);
                break;
        }
    }
    private void Transition(float time, StormState nextState)
    {
        if (counter < time) return;
        counter = 0;
        var oldState = state;
        state = nextState;
        if (state != oldState)
        {
            Debug.Log("Thunderstorm : " + state);
            ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(state);
        }
    }
}
