using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    [SerializeField]
    Animator _animator;
    [SerializeField]
    LightScript _light;
    private void Update()
    {
        _animator.gameObject.SetActive(_light.IsLit);
    }
}