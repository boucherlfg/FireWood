using System;
using static UnityEngine.Rendering.DebugUI;

public class Observable<T>
{
    public delegate void ChangeDelegate(T oldValue, T newValue);
    public event ChangeDelegate Changed;
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            var oldValue = _value;
            _value = value;
            if (_value == null ^ oldValue == null)
            {
                Changed?.Invoke(oldValue, _value);
            }
            else if(_value != null && !_value.Equals(oldValue)) 
            {
                Changed?.Invoke(oldValue, _value);
            }
            else if (oldValue != null && !oldValue.Equals(_value))
            {
                Changed?.Invoke(oldValue, _value);
            }
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