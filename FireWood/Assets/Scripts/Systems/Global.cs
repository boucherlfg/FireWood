using UnityEngine;

public class Global : MonoBehaviour
{
    [SerializeField]
    private GlobalConfig config;
    public GlobalConfig Current => config;
}