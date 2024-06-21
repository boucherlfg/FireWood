using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodScript : Interactable
{
    public delegate void WoodDelegate(WoodScript target, bool activated);
    public static event WoodDelegate Changed;
    public GameObject lootParticle;

    public void Refill()
    {
        gameObject.SetActive(true);
    }
    private void Start()
    {
        Changed?.Invoke(this, true);
    }
    private void OnDestroy()
    {
        Changed?.Invoke(this, false);
    }

    public override void Interact()
    {
        var gameState = ServiceManager.Instance.Get<GameState>();
        gameState.Wood.Value += 1;
        Instantiate(lootParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
