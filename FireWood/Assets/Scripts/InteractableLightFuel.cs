using UnityEngine;

public class InteractableLightFuel : Interactable
{
    [SerializeField]
    private float woodAmount;
    [SerializeField]
    private float woodCapacity = 1;
    [SerializeField]
    private LightScript lightScript;

    private bool consumeOverTime = false;
    public bool ConsumeOverTime
    {
        get => consumeOverTime;
        set => consumeOverTime = value;
    }
    [SerializeField]
    private float timePerLog = 60;
    public override void Interact()
    {
        var gameState = ServiceManager.Instance.Get<GameState>();
        if (gameState.Wood <= 0) return;
        if (woodAmount >= woodCapacity) return;
        gameState.Wood.Value--;
        woodAmount = Mathf.Min(woodCapacity, woodAmount + 1);
    }

    private void Update()
    {
        woodAmount = Mathf.Max(0, woodAmount - Time.deltaTime / timePerLog);
        lightScript.SetRange01((1 - Mathf.Exp(-4*woodAmount / woodCapacity)));
    }
    public void Extinguish() => woodAmount = 0;
}