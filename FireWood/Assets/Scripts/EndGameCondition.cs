using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EndGameCondition : MonoBehaviour
{
    [SerializeField]
    private float bufferTime = 10;

    private float counter = 0;
    // Update is called once per frame
    void Update()
    {
        var player = ServiceManager.Instance.Get(Ext.DefaultComponent<PlayerScript>);

        if (!player.Light.IsLit)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }

        if (counter > bufferTime)
        {
            Debug.Log("GAME OVER");
            ServiceManager.Instance.Get<OnPlayerLost>().Invoke();
        }
    }
}
