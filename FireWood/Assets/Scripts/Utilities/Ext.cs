using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Ext
{
    public static bool TryFind<T>(this IEnumerable<T> list, System.Func<T, bool> func, out T value)
    {
        value = default;
        foreach (var elem in list)
        {
            if (func(elem))
            {
                value = elem;
                return true;
            }
        }
        return false;
    }

    public static T FindMost<T>(this IEnumerable<T> list, System.Func<T, T, bool> AisMoreThanB)
    {
        if (list.Count() <= 0) return default;
        T most = list.ElementAt(0);
        foreach (var elem in list)
        {
            if (AisMoreThanB(elem, most)) most = elem;
        }
        return most;
    }

    public static bool Same(this Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b) < 0.0001f;
    }

    public static bool IsCollision(Vector2 position)
    {
        var hit = Physics2D.OverlapCircle(position, 0.3f);
        return hit && !hit.isTrigger;
    }

    public static void DoWhile(Func<bool> condition, Action action, int iterations = 10000)
    {
        for (int i = 0; i < iterations; i++)
        {
            if (!condition()) return;
            
            action();
        }
    }

    /// <summary>
    /// use reflection to get the value of a field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="origin"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetValue<T>(this object origin, string name)
    {
        var flag = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
        return (T)origin.GetType().GetField(name, flag).GetValue(origin);
    }

    /// <summary>
    /// use reflection to set the value of a field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="origin"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void SetValue<T>(this object origin, string name, T value)
    {
        var flag = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
        origin.GetType().GetField(name, flag).SetValue(origin, value);
    }

    public static T GetRandom<T>(this IEnumerable<T> source)
    {
        var index = UnityEngine.Random.Range(0, source.Count());
        return source.ElementAt(index);
    }
}