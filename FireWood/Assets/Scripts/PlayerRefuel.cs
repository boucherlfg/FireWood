using UnityEngine;

public class PlayerRefuel : MonoBehaviour
{
    [SerializeField]
    private LightScript _light;
    private PlayerLightFuel _player;
    private void Start()
    {
        _player = FindObjectOfType<PlayerLightFuel>(true);
    }
    private void Update()
    {
        if (_player.IsLitBy(_light))
        {
            _player.Refill();
        }

    }
}