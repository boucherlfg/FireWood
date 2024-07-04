public class OnStormChangedEvent : BaseEvent<StormChangedArgs> { }

public class StormChangedArgs 
{
    public Thunderstorm.StormState lastState; 
    public Thunderstorm.StormState newState;
}