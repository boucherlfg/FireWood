using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private LightScript _light;
    [SerializeField]
    private PlayerLightFuel _lightFuel;
    [SerializeField]
    private ActScript _actScript;

    public LightScript Light => _light;
    public PlayerLightFuel LightFuel => _lightFuel;
    public ActScript ActScript => _actScript;

    private void Start()
    {
        var gameState = ServiceManager.Instance.Get<GameState>();
        gameState.Wood.Value = 0;
    }
}