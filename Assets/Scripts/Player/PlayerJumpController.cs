using System.Collections;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    [Header("Buffer & Coyote jump")]
    [SerializeField] private float bufferJumpWindow = .25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = .5f;
    private float coyoteJumpActivated = -1;

    [Header("Wall interactions")]
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;

    private Rigidbody2D rb;
    private PlayerCollisionDetector collisionDetector;
    private PlayerMovementController movementController;
    private PlayerAnimationController animationController;

    private float defaultGravityScale;
    private bool canDoubleJump;
    private bool isWallJumping;
    private bool isAirborne;

    public bool CanDoubleJump => canDoubleJump;
    public bool IsWallJumping => isWallJumping;
    public bool IsAirborne => isAirborne;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody2D component not found on Player.");
        collisionDetector = GetComponent<PlayerCollisionDetector>();
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        defaultGravityScale = rb.gravityScale;
    }

    public void UpdateAirbornStatus()
    {
        if (collisionDetector.IsGrounded && isAirborne)
            HandleLanding();

        if (!collisionDetector.IsGrounded && !isAirborne)
            BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirborne = true;

        if (rb.linearVelocity.y < 0)
            ActivateCoyoteJump();
    }

    private void HandleLanding()
    {
        movementController.PlayDustFx();
        isAirborne = false;
        canDoubleJump = true;
        AttemptBufferJump();
    }

    public void RequestBufferJump()
    {
        if (isAirborne)
            bufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }

    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;
    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1;

    public void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActivated + coyoteJumpWindow;

        if (collisionDetector.IsGrounded || coyoteJumpAvailable)
        {
            Jump();
        }
        else if (collisionDetector.IsWallDetected && !collisionDetector.IsGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }

        CancelCoyoteJump();
    }

    private void Jump()
    {
        movementController.PlayDustFx();
        AudioManager.instance.PlaySFX(3);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void DoubleJump()
    {
        movementController.PlayDustFx();
        AudioManager.instance.PlaySFX(3);

        StopCoroutine(WallJumpRoutine());
        isWallJumping = false;
        movementController.SetWallJumpingState(false);
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
    }

    private void WallJump()
    {
        movementController.PlayDustFx();
        AudioManager.instance.PlaySFX(12);

        canDoubleJump = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -movementController.FacingDir, wallJumpForce.y);

        movementController.Flip();

        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        movementController.SetWallJumpingState(true);

        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
        movementController.SetWallJumpingState(false);
    }

    public void ResetGravityScale()
    {
        rb.gravityScale = defaultGravityScale;
    }

    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }
}
