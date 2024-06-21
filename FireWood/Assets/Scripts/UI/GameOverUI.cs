using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private bool isGameOver;
    public GameObject menu;
    private OnEndOfGame endOfGame;
    private InputSystem input;
    private void Start()
    {
        endOfGame = ServiceManager.Instance.Get<OnEndOfGame>();
        endOfGame.Subscribe(HandleEndOfGame);
        input = ServiceManager.Instance.Get<InputSystem>();
        input.Acted += Input_Acted;

    }
    private void OnDestroy()
    {
        input.Acted -= Input_Acted;
    }

    private void Input_Acted()
    {
        if (!isGameOver) return;
        SceneManager.LoadScene(gameObject.scene.name);
    }

    void HandleEndOfGame() 
    {
        menu.SetActive(true);
        isGameOver = true;
        endOfGame.Unsubscribe(HandleEndOfGame);
    }
}
