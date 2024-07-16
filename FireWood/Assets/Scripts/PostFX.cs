using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostFX : MonoBehaviour
{
    [System.Serializable]
    public struct Effects
    {
        public int rainEmission;
        public float saturation;
        public float exposure;
    }

    public SpriteRenderer darkness;
    public Volume volume;
    public ParticleSystem rain;

    public Effects stormPeace;
    public Effects stormIngoing;
    private Effects? currentStormEffect;

    public Color forestrerRoams;
    public Color foresterFollows;
    private Color startForesterColor;
    private Color targetForesterColor;
    private float foresterEffectTime = 0.5f;
    private float foresterEffectProgress = 0;

    private Storm storm;
    
    private int startRain;
    private float startExposure;
    private float startSaturation;
    private int targetRain;
    private float targetExposure;
    private float targetSaturation;

    // Start is called before the first frame update
    void Start()
    {
        startForesterColor = forestrerRoams;
        targetForesterColor = forestrerRoams;

        ServiceManager.Instance.Get<OnStormChangedEvent>().Subscribe(HandleStormChange);
        ServiceManager.Instance.Get<OnEndGameChanged>().Subscribe(HandleEndGameChanged);
        ServiceManager.Instance.Get<OnForesterStateChanged>().Subscribe(HandleForesterStateChanged);
    }

    private void HandleForesterStateChanged(ForesterScript.ForesterState state)
    {
        if(!volume.profile.TryGet(out ColorAdjustments colors)) return;

        var newForesterColor = state switch {
            ForesterScript.ForesterState.FollowPlayer => foresterFollows,
            _ => forestrerRoams
        };

        if(Vector4.Distance(newForesterColor, targetForesterColor) < 0.001f) return;
        
        colors.colorFilter.value = targetForesterColor;
        startForesterColor = targetForesterColor;
        targetForesterColor = newForesterColor;
        foresterEffectProgress = 0;
    }

    private void HandleEndGameChanged(EndGameArgs args)
    {
        volume.profile.TryGet(out Vignette vignette);
        vignette.intensity.value = args.completion;
    }

    private void Update()
    {
        if (!volume.profile.TryGet(out ColorAdjustments colors)) return;

        UpdateStormEffect(colors);
        UpdateForesterEffect(colors);
    }

    void UpdateStormEffect(ColorAdjustments colors) 
    {
        if (!currentStormEffect.HasValue) return;

        var rain = this.rain.emission;
        storm = storm ? storm : FindObjectOfType<Storm>();

        colors.postExposure.value = Mathf.Lerp(startExposure, targetExposure, storm.StateProgress);
        colors.saturation.value = Mathf.Lerp(startSaturation, targetSaturation, storm.StateProgress);
        rain.rateOverTime = Mathf.Lerp(startRain, targetRain, storm.StateProgress);
        
    }
   
    void UpdateForesterEffect(ColorAdjustments colors) 
    {
        if(foresterEffectProgress >= 1) return;
        foresterEffectProgress = Mathf.Min(1f, foresterEffectProgress + Time.deltaTime / foresterEffectTime);
        
        colors.colorFilter.value = Color.Lerp(startForesterColor, targetForesterColor, foresterEffectProgress);
    }

    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnEndGameChanged>().Unsubscribe(HandleEndGameChanged);
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(HandleStormChange);
        ServiceManager.Instance.Get<OnForesterStateChanged>().Unsubscribe(HandleForesterStateChanged);
    }

    void HandleStormChange(Storm.StormState state) 
    {
        if (!volume.profile.TryGet(out ColorAdjustments colors)) return;
        var rain = this.rain.emission;
        storm = storm ? storm : FindObjectOfType<Storm>();

        Effects? newStormEffect = state switch
        {
            Storm.StormState.TransitionToIngoing => stormIngoing,
            Storm.StormState.TransitionToPeace => stormPeace,
            _ => null
        };

        if (currentStormEffect.HasValue)
        {
            startRain = currentStormEffect.Value.rainEmission;
            startExposure = currentStormEffect.Value.exposure;
            startSaturation = currentStormEffect.Value.saturation;

            colors.saturation.value = targetSaturation;
            colors.postExposure.value = targetExposure;
            rain.rateOverTime = targetRain;
        }

        if(newStormEffect.HasValue) 
        {
            targetRain = newStormEffect.Value.rainEmission;
            targetExposure = newStormEffect.Value.exposure;
            targetSaturation = newStormEffect.Value.saturation;
        }

        currentStormEffect = newStormEffect;

    }
}
