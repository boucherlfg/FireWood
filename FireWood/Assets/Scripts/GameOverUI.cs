using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private bool isGameOver;
    public GameObject menu;
    private OnEndOfGame endOfGame;
    private void Start()
    {
        endOfGame = ServiceManager.Instance.Get<OnEndOfGame>();
        endOfGame.Subscribe(HandleEndOfGame);
    }
    private void Update()
    {
        if (!isGameOver) return;

        if (!Input.GetKey(KeyCode.Space)) return;

        SceneManager.LoadScene(gameObject.scene.name);
    }

    void HandleEndOfGame() 
    {
        menu.SetActive(true);
        isGameOver = true;
        endOfGame.Unsubscribe(HandleEndOfGame);
    }
}
