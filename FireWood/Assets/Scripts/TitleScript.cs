using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    [SerializeField]
    float fadeInDuration = 3;
    [SerializeField]
    float stayDuration = 3;
    [SerializeField]
    float fadeOutDuration = 3;
    [SerializeField]
    float flicker = 0.1f;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Image image;
    OnTutorialChanged tutorial;

    Color baseColor;
    // Start is called before the first frame update
    void Start()
    {
        tutorial = ServiceManager.Instance.Get<OnTutorialChanged>();
        tutorial.Subscribe(HandleEndOfTutorial);
        baseColor = image.color;
    }

    private void Update()
    {
        image.color = new Color(baseColor.r - Random.value * flicker,
                                baseColor.g - Random.value * flicker,
                                baseColor.b - Random.value * flicker);
    }

    void HandleEndOfTutorial(StartGameTutorial.TutorialState state) 
    {
        if (state != StartGameTutorial.TutorialState.TutorialOver) return;
        tutorial.Unsubscribe(HandleEndOfTutorial);
        StartCoroutine(DisplayTitle());
    }

    IEnumerator DisplayTitle() 
    {
        while (canvasGroup.alpha < 1 - float.Epsilon)
        {
            canvasGroup.alpha += Time.deltaTime / fadeInDuration;
            yield return null;
        }
        yield return new WaitForSeconds(stayDuration);
        while (canvasGroup.alpha > float.Epsilon)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeOutDuration;
            yield return null;
        }
    }
}
