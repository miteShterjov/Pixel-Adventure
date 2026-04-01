using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private ParticleSystem dustFx;

    private Rigidbody2D rb;
    private PlayerCollisionDetector collisionDetector;

    private Vector2 moveInput;
    private bool facingRight = true;
    private int facingDir = 1;
    private bool isWallJumping;

    public int FacingDir => facingDir;
    public bool FacingRight => facingRight;
    public Vector2 MoveInput => moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionDetector = GetComponent<PlayerCollisionDetector>();
    }

    public void HandleInput(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    public void SetWallJumpingState(bool state)
    {
        isWallJumping = state;
    }

    public void HandleMovement()
    {
        if (collisionDetector.IsWallDetected)
            return;

        if (isWallJumping)
            return;

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    public void HandleWallSlide(Vector2 wallSlideInput)
    {
        bool canWallSlide = collisionDetector.IsWallDetected && rb.linearVelocity.y < 0;
        float yModifer = wallSlideInput.y < 0 ? 1 : .05f;

        if (canWallSlide == false)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifer);
    }

    public void HandleFlip()
    {
        if (moveInput.x < 0 && facingRight || moveInput.x > 0 && !facingRight)
            Flip();
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        collisionDetector.SetFacingDir(facingDir);
    }

    public void PlayDustFx()
    {
        dustFx.Play();
    }
}
