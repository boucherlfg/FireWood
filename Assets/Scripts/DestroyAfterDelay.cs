using System.Collections;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour 
{
    [SerializeField]
    private float delay;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}