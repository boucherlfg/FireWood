using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    private void Start()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Subscribe(LogStormChange);
    }
    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(LogStormChange);
    }

    void LogStormChange(StormState state) => Debug.Log(state);

    public enum StormState : int { First = 0, Ingoing = 1, Peace = 2, TransitionToIngoing = 3, TransitionToPeace = 4 };

    private StormState state = StormState.First;

    [SerializeField] private float minPeaceTime = 60;
    [SerializeField] private float maxPeaceTime = 120;

    [SerializeField] private float minIngoingTime = 15;
    [SerializeField] private float maxIngoingTime = 45;
    
    [SerializeField] private float firstTime = 60;
    [SerializeField] private float transitionTime = 10;
    [SerializeField] private float stormMultiplier = 2;

    private float currentPeaceTime = 0;
    private float currentIngoingTime = 0;

    public float StateProgress => counter / CurrentTime;

    public StormState State => state;
    public float CurrentTime => state switch
    {
        StormState.First => First,
        StormState.Ingoing => Ingoing,
        StormState.Peace => Peace,
        _ => Transition
    };
    public float First => firstTime;
    public float Ingoing => currentIngoingTime;
    public float Peace => currentPeaceTime;
    public float Transition => transitionTime;
    public float Multiplier => stormMultiplier;

    private float counter = 0;
    
    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        switch (state)
        {
            case StormState.First:
                DoFirst();
                break;
            case StormState.Ingoing:
                DoIngoing();
                break;
            case StormState.TransitionToIngoing:
                DoTransitionToIngoing();
                break;
            case StormState.TransitionToPeace:
                DoTransitionToPeace();
                break;
            case StormState.Peace:
                DoPeace();
                break;
        }
    }
    void DoFirst() 
    {
        counter += Time.deltaTime;
        if (counter < First) return;
        counter = 0;

        state = StormState.TransitionToIngoing;
        ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(state);
    }
    void DoTransitionToIngoing()
    {
        counter += Time.deltaTime;
        if (counter < Transition) return;
        counter = 0;
        currentIngoingTime = Random.Range(minIngoingTime, maxIngoingTime);

        state = StormState.Ingoing;
        ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(state);
    }
    void DoIngoing()
    {
        counter += Time.deltaTime;
        if (counter < Ingoing) return;
        counter = 0;

        state = StormState.TransitionToPeace;
        ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(state);

    }
    void DoTransitionToPeace() 
    {
        counter += Time.deltaTime;
        if (counter < Transition) return;
        counter = 0;
        currentPeaceTime = Random.Range(minPeaceTime, maxPeaceTime);

        state = StormState.Peace;
        ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(state);
    }
    void DoPeace() 
    {
        counter += Time.deltaTime;
        if (counter < Peace) return;
        counter = 0;

        state = StormState.TransitionToIngoing;
        ServiceManager.Instance.Get<OnStormChangedEvent>().Invoke(state);
    }

}
