using System;

public class GameState
{
    public event Action Changed;
    public Observable<int> Wood
    {
        get;
        private set;
    }

    public GameState()
    {
        Wood = new(0);
        Wood.Changed += (a, b) => Invoke();
    }

    private void Invoke() => Changed?.Invoke();

    
}