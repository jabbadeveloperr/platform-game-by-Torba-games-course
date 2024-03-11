using UnityEngine;

public class PlayerAnimatorController
{
    private Animator animator;
    private Rigidbody2D rb;

    public PlayerAnimatorController(Animator animator, Rigidbody2D rb)
    {
        this.animator = animator;
        this.rb = rb;
    }

    public void ChangeAnimation(bool isMoving, bool isGrounded)
    {
        if (isMoving && isGrounded)
            SetAnimation(AnimationState.Run);
        else if (isGrounded)
            SetAnimation(AnimationState.Idle);
        else
            SetAnimation(rb.velocity.y > Constants.ZeroFloat ? AnimationState.Up : AnimationState.Down);
    }

    private void SetAnimation(AnimationState state)
    {
        animator.SetTrigger($"to{state}");
    } 
}