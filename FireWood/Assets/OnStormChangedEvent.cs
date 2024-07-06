public class OnStormChangedEvent : BaseEvent<StormChangedArgs> { }

public class StormChangedArgs 
{
    public Storm.StormState lastState; 
    public Storm.StormState newState;
}