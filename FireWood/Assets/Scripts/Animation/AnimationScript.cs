using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationScript : MonoBehaviour
{
    public const string IdleUp = nameof(IdleUp),
                        IdleLeft = nameof(IdleLeft),
                        IdleDown = nameof(IdleDown),
                        IdleRight = nameof(IdleRight),
                        WalkUp = nameof(WalkUp),
                        WalkLeft = nameof(WalkLeft),
                        WalkDown = nameof(WalkDown),
                        WalkRight = nameof(WalkRight);

    [SerializeField]
    private Animator _animator;
    public Animator Animator => _animator;
    private Vector2 lastVelocity;
    public Vector2 LastVelocity => lastVelocity;
    protected abstract Vector2 Velocity { get; }
    protected virtual void Start()
    {
        if(!_animator) _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        Vector2 velocity = Velocity;

        if (velocity.magnitude <= 0.1f)
        {
            if (lastVelocity.x < -0.1f) _animator.Play(IdleLeft);
            else if (lastVelocity.x > 0.1f) _animator.Play(IdleRight);
            else if (lastVelocity.y < -0.1f) _animator.Play(IdleDown);
            else if (lastVelocity.y > 0.1f) _animator.Play(IdleUp);
        }
        else
        {
            if (velocity.x < -0.1f) _animator.Play(WalkLeft);
            else if (velocity.x > 0.1f) _animator.Play(WalkRight);
            else if (velocity.y < -0.1f) _animator.Play(WalkDown);
            else if (velocity.y > 0.1f) _animator.Play(WalkUp);
            lastVelocity = velocity;
        }
    }
}
