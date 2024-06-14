using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private float nominalHintEmission = 20;
    [SerializeField]
    private ParticleSystem hint;
    public abstract void Interact();
    public void Hint(bool isOn)
    {
        if (isOn && hint.emission.rateOverTime.constant < 5) OnHover(nominalHintEmission);
        else if (!isOn && hint.emission.rateOverTime.constant > 5) OnHover(0);
    }
    protected virtual void OnHover(float value) 
    {
        if (GetType() == typeof(WoodScript))
        {
            Debug.Log("");
        }
        var emission = hint.emission;
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = value;
        emission.rateOverTime = rateOverTime;
    }
}