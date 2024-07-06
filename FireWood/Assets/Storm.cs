using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    public enum StormState { First = 0, Ingoing = 1, Peace = 2, Transition = 3 };

    private StormState lastState;
    private StormState state = StormState.First;
    public float timeBeforeFirstStorm = 60;
    public float intervalBetweenStorms = 30;
    public float stormDuration = 10;
    public float transitionDuration = 3;
    public float thunderStormMultiplier = 2;
    
    private float counter = 0;
    private void OnValidate()
    {
        transitionDuration = Mathf.Min(transitionDuration, Mathf.Min(timeBeforeFirstStorm, intervalBetweenStorms, stormDuration));
    }
    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        switch (state)
        {
            case StormState.First:
                Transition(timeBeforeFirstStorm, StormState.Transition);
                break;
            case StormState.Ingoing:
                Transition(stormDuration, StormState.Transition);
                break;
            case StormState.Transition:
                if ((lastState & (StormState)1) == 0) Transition(transitionDuration, StormState.Ingoing);
                else if (lastState == StormState.Ingoing) Transition(transitionDuration, StormState.Peace);
                break;
            case StormState.Peace:
                Transition(intervalBetweenStorms, StormState.Transition);
                break;
        }
    }
    private void Transition(float time, StormState nextState)
    {
        if (counter < time) return;
        counter = 0;
        lastState = state;
        state = nextState;
        if (state != lastState)
        {
            Debug.Log("Thunderstorm : " + state);
            ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(new() { lastState = lastState, newState = state });
        }
    }
}
