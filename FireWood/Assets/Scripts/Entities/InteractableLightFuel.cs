using UnityEngine;

public class InteractableLightFuel : Interactable
{
    private Storm storm;
    [SerializeField]
    private GameObject refuelParticle;
    [SerializeField]
    private float woodAmount;
    [SerializeField]
    private float woodCapacity = 1;
    [SerializeField]
    private LightScript lightScript;

    private float thunderstormMultiplier = 1;

    private bool consumeOverTime = false;
    public bool ConsumeOverTime
    {
        get => consumeOverTime;
        set => consumeOverTime = value;
    }
    [SerializeField]
    private float timePerLog = 60;
    
    private void Start()
    {
        storm = FindObjectOfType<Storm>();
        ServiceManager.Instance.Get<OnStormChangedEvent>().Subscribe(HandleStormChanged);
    }
   
    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(HandleStormChanged);
    }
   
    private void Update()
    {
        woodAmount = Mathf.Max(0, woodAmount - thunderstormMultiplier * Time.deltaTime / timePerLog);
        lightScript.SetRange01((1 - Mathf.Exp(-4*woodAmount / woodCapacity)));
    }

    public override void Interact()
    {
        var gameState = ServiceManager.Instance.Get<GameState>();
        if (gameState.Wood <= 0) return;
        if (woodAmount >= woodCapacity) return;

        Instantiate(refuelParticle, transform.position, Quaternion.identity);
        gameState.Wood.Value--;
        woodAmount = Mathf.Min(woodCapacity, woodAmount + 1);
    }

    private void HandleStormChanged(Storm.StormState state)
    {
        thunderstormMultiplier = state switch
        {
            Storm.StormState.Ingoing => storm.Multiplier,
            _ => 1,
        };
    }
   
    public void Extinguish() => woodAmount = 0;
}