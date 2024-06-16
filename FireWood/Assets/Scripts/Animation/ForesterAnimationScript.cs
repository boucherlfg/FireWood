using Pathfinding;
using UnityEngine;

public class ForesterAnimationScript : AnimationScript
{
    AIPath aiPath;
    protected override Vector2 Velocity
    {
        get => aiPath.velocity;
    }
    protected override void Start()
    {
        aiPath = GetComponent<AIPath>();
        base.Start();
    }
}