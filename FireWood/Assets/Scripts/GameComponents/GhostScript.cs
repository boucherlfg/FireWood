using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    private float baseAlpha;
    private SpriteRenderer _renderer;
    private Coroutine coroutine;
    private PlayerScript player;
    private ForesterScript forester;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        baseAlpha = _renderer.color.a;
    }
    // Update is called once per frame
    void Update()
    {
        player = player ? player : FindObjectOfType<PlayerScript>();
        forester = forester ? forester : FindObjectOfType<ForesterScript>();

        var isLit = LightScript.lights.Any(x => Vector2.Distance(transform.position, x.transform.position) < x.CurrentRange);
        if (Vector2.Distance(player.transform.position, transform.position) < player.Light.CurrentRange || isLit
         || Vector2.Distance(forester.transform.position, transform.position) < forester.CurrentRange)
        {
            coroutine = StartCoroutine(Disappear());
        }
        else if (Vector2.Distance(player.transform.position, transform.position) > player.Light.CurrentRange * 4 && !isLit 
            && Vector2.Distance(forester.transform.position, transform.position) > forester.CurrentRange * 2)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            var color = _renderer.color;
            color.a = baseAlpha;
            _renderer.color = color;

        }
    }

    IEnumerator Disappear()
    {
        var disappearingSpeed = Random.Range(5, 20);
        while(_renderer.color.a > float.Epsilon)
        {
            var color = _renderer.color;
            color.a = Mathf.Max(color.a - Time.deltaTime / disappearingSpeed, 0);
            _renderer.color = color;
            yield return null;
        }
    }
}
