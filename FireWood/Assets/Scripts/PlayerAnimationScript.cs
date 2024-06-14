using UnityEngine;

public class PlayerAnimationScript : AnimationScript
{
    private Rigidbody2D _rigidBody;
    protected override Vector2 Velocity => _rigidBody.velocity;
    protected override void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>(); 
        base.Start();
    }
}
