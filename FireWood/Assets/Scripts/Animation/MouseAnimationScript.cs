using Pathfinding;
using System.Collections;
using UnityEngine;

public class MouseAnimationScript : AnimationScript
{
    private const string DisappearLeft = nameof(DisappearLeft);
    private const string DisappearRight = nameof(DisappearRight);
    private const string DisappearUp = nameof(DisappearUp);
    private const string DisappearDown = nameof(DisappearDown);
    [SerializeField]
    MouseScript mouseScript;
    [SerializeField]
    AIPath aiPath;
    private bool stopAnimation = false;
    protected override Vector2 Velocity
    {
        get => aiPath.velocity;
    }

    protected override void Update()
    {
        if (stopAnimation) return;
        if (mouseScript.ShouldDisappear)
        {
            if (LastVelocity.x < -0.01f)
            {
                stopAnimation = true;
                Animator.Play(DisappearLeft);
            }
            else if (LastVelocity.x > 0.01f)
            {
                stopAnimation = true;
                Animator.Play(DisappearRight);
            }
            else if (LastVelocity.y < -0.01f)
            {
                stopAnimation = true;
                Animator.Play(DisappearDown);
            }
            else
            {
                stopAnimation = true;
                Animator.Play(DisappearUp);
            }
        }
        base.Update();
    }

    public void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
