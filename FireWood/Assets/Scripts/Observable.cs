using System;
using static UnityEngine.Rendering.DebugUI;

public class Observable<T>
{
    public event Action Changed;
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            Changed?.Invoke();
        }
    }

    public Observable(T value)
    {
        _value = value;
    }


    public static implicit operator T(Observable<T> observable)
    {
        return observable.Value;
    }
}