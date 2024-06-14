using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField]
    private float lightOutDelay = 60;
    [SerializeField]
    private float woodAmount;
    [SerializeField]
    private float woodCapacity = 1;
    [SerializeField]
    private float maximumRange = 5;
    [SerializeField]
    private float flicker = 0.01f;
    [SerializeField]
    private Transform _lightRange;

    public bool IsLit => woodAmount >= 0.00001f;


    public bool IsLitBy(LightScript light)
    {
        return Vector2.Distance(transform.position, light.transform.position) < light.CurrentRange;
    }
    public void Refill()
    {
        woodAmount = Mathf.Min(woodCapacity, woodAmount + Time.deltaTime);
    }
    public void Extinguish()
    {
        woodAmount = 0;
    }
    public float CurrentRange 
    { 
        get {
            var debug = (1 - Mathf.Exp(-woodAmount / woodCapacity)) * maximumRange / 2;
            return debug;
        }
    }
    private void Update()
    {
        woodAmount = Mathf.Max(0, woodAmount - Time.deltaTime / lightOutDelay);
        _lightRange.localScale = 2 * CurrentRange * Vector3.one + flicker * Random.value * Vector3.one;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, CurrentRange);
    }
#endif
}
