using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    private bool paused = false;
    public Slider volume;
    public Slider lighting;
    public GameObject menu;

    GameState gameState;
    InputSystem input;

    private void Start()
    {
        gameState = ServiceManager.Instance.Get<GameState>();
        input = ServiceManager.Instance.Get<InputSystem>();
        input.Paused += Input_Paused;
    }

    private void Input_Paused()
    {
        paused = !paused;
        if (paused) HandlePause();
        else HandleResume();
    }

    private void HandlePause()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        
        volume.value = gameState.Volume.Value;
        lighting.value = gameState.Lighting.Value;
    }

    public void HandleResume()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChangeVolume(float value) 
    {
        gameState.Volume.Value = value;
    }
    
    public void ChangeLighting(float value) 
    {
        gameState.Lighting.Value = value;
    }

    public void Restart()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }
}
