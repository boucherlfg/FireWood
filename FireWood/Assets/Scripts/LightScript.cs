using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public static List<LightScript> lights = new();
    [SerializeField]
    private float maximumRange = 5;
    [SerializeField]
    private float flicker = 0.01f;
    [SerializeField]
    private Transform _lightRange;
    [SerializeField]
    private float currentFill = 1;
    [SerializeField]
    private bool exclude = false;
    public bool Exclude => exclude;

    public bool IsLit => CurrentRange > 0.000001;


    public float CurrentRange => currentFill * maximumRange / 2;

    protected void Start()
    {
        lights.Add(this);
    }
    private void OnDestroy()
    {
        lights.Remove(this);
    }
    public void Update()
    {
        _lightRange.localScale = 2 * CurrentRange * Vector3.one + flicker * Random.value * Vector3.one;
    }

    public void SetRange01(float value)
    {
        currentFill = value;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, CurrentRange);
    }
#endif

    public bool Covers(Vector2 position)
    {
        return Vector2.Distance(position, transform.position) < CurrentRange;
    }
}
