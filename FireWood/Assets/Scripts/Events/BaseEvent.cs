using System;

public abstract class BaseEvent
{
    private event Action Changed;
    public void Invoke()
    {
        Changed?.Invoke();
    }
    public void Subscribe(Action subscriber)
    {
        Changed += subscriber;
    }
    public void Unsubscribe(Action subscriber)
    {
        Changed -= subscriber;
    }
}
public abstract class BaseEvent<T>
{
    private event Action<T> Changed;
    public void Invoke(T args)
    {
        Changed?.Invoke(args);
    }
    public void Subscribe(Action<T> subscriber)
    {
        Changed += subscriber;
    }
    public void Unsubscribe(Action<T> subscriber)
    {
        Changed -= subscriber;
    }
}
