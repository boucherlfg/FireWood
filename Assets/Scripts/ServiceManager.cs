using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceManager : Singleton<ServiceManager>
{
    private readonly List<object> _services = new();

    public T Get<T>(Func<T> generator =  null) where T : class
    {
        var srv = _services.Find(x => x is T);
        if (srv is null)
        {
            generator ??= DefaultCtor<T>;
            srv = generator();
            _services.Add(srv);
        }
        return srv as T;
    }

    private static T DefaultCtor<T>() where T : class
    {
        var type = typeof(T);
        var ctor = type.GetConstructor(new Type[] { });
        return (T)ctor.Invoke(new object[] { });
    }

    public static T DefaultComponent<T>() where T : Component
    {
        var comp = GameObject.FindObjectOfType<T>();
        if (!comp)
        {
            comp = new GameObject(typeof(T).Name).AddComponent<T>();
            GameObject.DontDestroyOnLoad(comp.gameObject);
        }
        return comp;
    }
}