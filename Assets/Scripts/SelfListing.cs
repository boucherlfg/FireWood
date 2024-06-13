using System.Collections.Generic;
using UnityEngine;

public class SelfListing<T> : MonoBehaviour where T : SelfListing<T>
{
    private static List<T> _list = new();
    public static List<T> List => _list;

    protected virtual void Start()
    {
        _list.Add(this as T);
    }

    protected virtual void OnDestroy()
    {
        _list.Remove(this as T);
    }
}