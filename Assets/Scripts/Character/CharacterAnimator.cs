using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    // animation parameters
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die1 = Animator.StringToHash("Die");
    
    [SerializeField] private Animator animator;

    public void PlayAttacking()
    {
        animator.SetBool(Attack, true);
    }

    public void StopAttacking()
    {
        animator.SetBool(Attack, false);  
    }

    public void PlayMoving(float blendVal)
    {
        if (!animator.GetBool(Attack))
        {
            animator.SetFloat(Move, blendVal); 
        }
    }

    public void PlayDeath()
    {
        animator.SetBool(Die1, true);
    }
}