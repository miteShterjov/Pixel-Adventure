using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Player Visuals")]
    [SerializeField] private AnimatorOverrideController[] animators;
    [SerializeField] private int skinId;

    private static readonly int xVelocity = Animator.StringToHash("xVelocity");
    private static readonly int yVelocity = Animator.StringToHash("yVelocity");
    private static readonly int AnimGroundedParam = Animator.StringToHash("isGrounded");
    private static readonly int AnimWallDetectedParam = Animator.StringToHash("isWallDetected");
    private static readonly int AnimKnockedParam = Animator.StringToHash("isKnocked");


    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    public void UpdateAnimations()
    {
        anim.SetFloat(xVelocity, rb.linearVelocity.x);
        anim.SetFloat(yVelocity, rb.linearVelocity.y);
    }

    public void UpdateGroundedAnimation(bool isGrounded)
    {
        anim.SetBool(AnimGroundedParam, isGrounded);
    }

    public void UpdateWallDetectedAnimation(bool isWallDetected)
    {
        anim.SetBool(AnimWallDetectedParam, isWallDetected);
    }

    public void SetKnockedAnimation(bool isKnocked)
    {
        anim.SetBool(AnimKnockedParam, isKnocked);
    }

    public void UpdateSkin()
    {
        SkinManager skinManager = SkinManager.instance;

        if (skinManager == null)
            return;

        anim.runtimeAnimatorController = animators[skinManager.choosenSkinId];
    }
}
