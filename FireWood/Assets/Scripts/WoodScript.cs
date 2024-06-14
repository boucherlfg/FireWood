using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodScript : Interactable
{
    public static List<WoodScript> woodPiles = new();
    public GameObject lootParticle;

    public void Refill()
    {
        gameObject.SetActive(true);
    }
    private void Start()
    {
        woodPiles.Add(this);
    }

    public override void Interact()
    {
        var gameState = ServiceManager.Instance.Get<GameState>();
        gameState.Wood.Value += 1;
        Instantiate(lootParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
