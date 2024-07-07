public class OnEndGameChanged : BaseEvent<EndGameArgs>
{
    
}

public class EndGameArgs
{
    public EndGameCondition.EndGameState state;
    public float completion;

    public EndGameArgs(EndGameCondition.EndGameState state, float completion)
    {
        this.state = state;
        this.completion = completion;
    }
}