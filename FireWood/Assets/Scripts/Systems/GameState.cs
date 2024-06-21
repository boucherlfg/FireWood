using System;

public class GameState
{
    public event Action Changed;
    public Observable<int> Wood
    {
        get;
        private set;
    }

    public Observable<float> Volume
    {
        get;
        private set;
    }

    public Observable<float> Lighting
    {
        get;
        private set;
    }

    public GameState()
    {
        Wood = new(0);
        Wood.Changed += (a, b) => Invoke();

        Volume = new(1);
        Volume.Changed += (a, b) => Invoke();

        Lighting = new(1);
        Lighting.Changed += (a, b) => Invoke();
    }

    private void Invoke() => Changed?.Invoke();

    
}