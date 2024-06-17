using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialElement : MonoBehaviour
{
    [Serializable]
    public struct TutorialRule
    {
        public StartGameTutorial.TutorialState state;
        public UnityEvent action;
    }
    public List<TutorialRule> rules;
    private OnTutorialChanged tutorialChanged;
    private void Start()
    {
        tutorialChanged = ServiceManager.Instance.Get<OnTutorialChanged>();
        tutorialChanged.Subscribe(OnTutorialChanged);
    }
    private void OnDestroy()
    {
        tutorialChanged.Unsubscribe(OnTutorialChanged);
    }

    private void OnTutorialChanged(StartGameTutorial.TutorialState state) 
    {
        rules.ForEach(rule =>
        {
            if (rule.state == state)
            {
                rule.action?.Invoke();
            }
        });
    }
}