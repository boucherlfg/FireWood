
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
    private EndGameState _state;
    private EndGameState State
    {
        get => _state;
        set
        {
            var oldState = _state;
            _state = value;
            if (oldState != _state)
            {
                ServiceManager.Instance.Get<OnEndGameChanged>().Invoke(new EndGameArgs(_state, counter / bufferTime));
            }
        }
    }
    [SerializeField]
    private float bufferTime = 10;

    private float counter = 0;
    // Update is called once per frame
    void Update()
    {
        switch (State)
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
        ServiceManager.Instance.Get<OnEndGameChanged>().Invoke(new(_state, 0));
        counter = 0;
        if (!player.Light.IsLit) State = EndGameState.Danger;
    }
    void Danger()
    {
        player = player ? player : FindObjectOfType<PlayerScript>();
        counter += Time.deltaTime;
        ServiceManager.Instance.Get<OnEndGameChanged>().Invoke(new(_state, counter / bufferTime));

        if (player.Light.IsLit) State = EndGameState.Safe;
        else if (counter >= bufferTime) State = EndGameState.Dead;
    }
    void Dead()
    {
        player = player ? player : FindObjectOfType<PlayerScript>();
        player.gameObject.SetActive(false);
        ServiceManager.Instance.Get<OnEndOfGame>().Invoke();
        State = EndGameState.End;
    }
}
