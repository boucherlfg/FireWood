using UnityEngine;

[CreateAssetMenu(menuName = "Felix/Global Config")]
public class GlobalConfig : ScriptableObject
{
    public float steeringTimeout = 10;
    public float avoidDistance = 1;
    public float avoidWeight = 1;
    public float chaseWeight = 1;
}