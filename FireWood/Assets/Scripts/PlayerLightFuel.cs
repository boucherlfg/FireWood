using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightFuel : MonoBehaviour
{
    private Thunderstorm storm;
    [SerializeField]
    private LightScript _light;
    [SerializeField]
    private float woodAmount;
    [SerializeField]
    private float woodCapacity = 1;
    [SerializeField]
    private float timePerLog = 60;

    private float thunderstormMultiplier = 1;

    private bool consumeOverTime = false;
    public bool ConsumeOverTime
    {
        get => consumeOverTime;
        set => consumeOverTime = value;
    }
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
    private void Start()
    {
        storm = FindObjectOfType<Thunderstorm>();
        ServiceManager.Instance.Get<OnStormChangedEvent>().Subscribe(HandleStormChanged);
    }

    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(HandleStormChanged);
    }

    private void Update()
    {
        if (!consumeOverTime) return;
        woodAmount = Mathf.Max(0, woodAmount - thunderstormMultiplier * Time.deltaTime / timePerLog);

        _light.SetRange01((1 - Mathf.Exp(-4 * woodAmount / woodCapacity)));
    }

    private void HandleStormChanged(Thunderstorm.StormState state)
    {
        thunderstormMultiplier = state switch
        {
            Thunderstorm.StormState.Ingoing => storm.thunderStormMultiplier,
            _ => 1,
        };
    }
}
