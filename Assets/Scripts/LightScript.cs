using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : Interactable
{
    public static List<LightScript> lights = new();
    [SerializeField]
    private float lightOutDelay = 30;
    [SerializeField]
    private float woodAmount;
    [SerializeField]
    private float woodCapacity = 1;
    [SerializeField]
    private float maximumRange = 5;
    [SerializeField]
    private float flicker = 0.01f;
    [SerializeField]
    Animator _animator;
    [SerializeField]
    private Transform _lightRange;

    private PlayerLight _player;

    public bool IsLit => woodAmount > 0.000001;

    public void Extinguish() => woodAmount = 0;

    public float CurrentRange => (1 - Mathf.Exp(-woodAmount / woodCapacity)) * maximumRange / 2;

    protected void Start()
    {
        lights.Add(this);
        _player = FindObjectOfType<PlayerLight>(true);
    }
    private void OnDestroy()
    {
        lights.Remove(this);
    }
    public void Update()
    {
        if (_player.IsLitBy(this))
        {
            _player.Refill();
        }

        woodAmount = Mathf.Max(0, woodAmount - Time.deltaTime/lightOutDelay);
        _lightRange.localScale = 2 * CurrentRange * Vector3.one + flicker * Random.value * Vector3.one;
        _animator.gameObject.SetActive(IsLit);
    }

    public override void Interact()
    {
        var gameState = ServiceManager.Instance.Get<GameState>();
        if (gameState.Wood <= 0) return;
        if (woodAmount >= woodCapacity) return;
        gameState.Wood.Value--;
        woodAmount = Mathf.Min(woodCapacity, woodAmount + 1);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, CurrentRange);
    }

    internal bool Covers(Vector2 position)
    {
        return Vector2.Distance(position, transform.position) < CurrentRange;
    }
}
