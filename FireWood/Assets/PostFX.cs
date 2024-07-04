using System.Collections;
using Unity.VisualScripting;
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

    private Thunderstorm storm;
    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Subscribe(HandleStormChange);
    }

    private void OnDestroy()
    {
        ServiceManager.Instance.Get<OnStormChangedEvent>().Unsubscribe(HandleStormChange);
    }

    void HandleStormChange(StormChangedArgs args) 
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        if (args.newState != Thunderstorm.StormState.Transition)
        {
            return;
        }
        if ((args.lastState & (Thunderstorm.StormState)1) == 0)
        {
            coroutine = StartCoroutine(StormCoroutine(stormIngoing));
        }
        else if (args.lastState == Thunderstorm.StormState.Ingoing)
        {
            coroutine = StartCoroutine(StormCoroutine(stormPeace));
        }

        IEnumerator StormCoroutine(Effects effect)
        {
            var rain = this.rain.emission;
            storm = storm ? storm : FindObjectOfType<Thunderstorm>();
            if (!volume.profile.TryGet(out ColorAdjustments colors)) goto end;

            var startRain = rain.rateOverTime.constant;
            var startExposure = colors.postExposure.value;
            var startSaturation = colors.saturation.value;

            var targetRain = effect.rainEmission;
            var targetExposure = effect.exposure;
            var targetSaturation = effect.saturation;

            var rainFloat = startRain;
            while (true)
            {
                var isExposureDone = ChangeExposure();
                var isSaturationDone = ChangeSaturation();
                var isRainDone = ChangeRain();

                if (isExposureDone && isSaturationDone) break;
                yield return null;
            }

            bool ChangeExposure()
            {
                var delta = targetExposure - startExposure;
                colors.postExposure.value += delta * Time.deltaTime / storm.transitionDuration;
                return false;
            }

            bool ChangeSaturation()
            {
                var delta = targetSaturation - startSaturation;
                colors.saturation.value += delta * Time.deltaTime / storm.transitionDuration;
                return false;
            }

            bool ChangeRain()
            {
                var delta = targetRain - startRain;
                rainFloat += delta * Time.deltaTime / storm.transitionDuration;
                rain.rateOverTime = (int)rainFloat;
                return false;
            }

            end:;
        }
    }
}
