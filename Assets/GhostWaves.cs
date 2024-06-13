using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWaves : MonoBehaviour
{
    public static event Action<bool> Changed;
    public bool isWaveActive = false;
    [SerializeField]
    private float onTime = 10;
    [SerializeField]
    private float offTime = 60;

    private float counter = 0;
    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (isWaveActive && counter > onTime)
        {
            counter = 0;
            isWaveActive = false;
            Changed?.Invoke(isWaveActive);
            var player = ServiceManager.Instance.Get(Ext.DefaultComponent<PlayerLight>);
            if (!player.IsLit) Debug.Log("GAME OVER");
        }
        else if (!isWaveActive && counter > offTime)
        {
            counter = 0;
            isWaveActive = true;
            Changed?.Invoke(isWaveActive);
        }
    }
}
