using UnityEngine;

public class WoodDisplay : MonoBehaviour
{
    private GameState _gameState;
    [SerializeField]
    private TMPro.TMP_Text label;

    private void Start()
    {
        _gameState = ServiceManager.Instance.Get<GameState>();
        _gameState.Wood.Changed += Wood_Changed;
        Wood_Changed();
    }

    private void Wood_Changed()
    {
        label.text = "" + _gameState.Wood.Value;
    }
}