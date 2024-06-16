using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameTutorial : MonoBehaviour
{
    private OnTutorialChanged tutorialChanged;
    private OnNotification notifications;
    private GameState gameState;

    [SerializeField]
    private string moveToWoodMessage;
    [SerializeField]
    private string takeWoodMessage;
    [SerializeField]
    private string lightFireMessage;
    [SerializeField]
    private string tutorialOverMessage;

    [SerializeField]
    private WoodScript wood;
    [SerializeField]
    private LightScript fire;
    [SerializeField]
    private PlayerScript player;

    private TutorialState state;
    public enum TutorialState
    {
        MoveToWood = 0,
        TakeWood = 1,
        LightFire = 2,
        TutorialOver = 3
    }

    private IEnumerator Start()
    {
        tutorialChanged = ServiceManager.Instance.Get<OnTutorialChanged>();
        notifications = ServiceManager.Instance.Get<OnNotification>();
        gameState = ServiceManager.Instance.Get<GameState>();
        yield return null;
        notifications.Invoke(moveToWoodMessage);
        tutorialChanged.Invoke(TutorialState.MoveToWood);
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case TutorialState.MoveToWood:
                MoveToWood();
                break;
            case TutorialState.TakeWood:
                TakeWood();
                break;
            case TutorialState.LightFire:
                LightFire();
                break;
        }
    }

    void SetState(TutorialState state) 
    {
        this.state = state;
        tutorialChanged.Invoke(state);
    }
    void MoveToWood()
    {
        if (Vector2.Distance(player.transform.position, wood.transform.position) < player.ActScript.Range) 
        {
            notifications.Invoke(takeWoodMessage);
            SetState(TutorialState.TakeWood);
        }
    }

    void TakeWood()
    {
        if (gameState.Wood.Value > 0)
        {
            notifications.Invoke(lightFireMessage);
            SetState(TutorialState.LightFire);
        }
    }

    void LightFire()
    {
        if (gameState.Wood.Value < 1)
        {
            notifications.Invoke(tutorialOverMessage);
            SetState(TutorialState.TutorialOver);

        }
    }

}
