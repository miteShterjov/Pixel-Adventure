using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public int FacingDir { get; private set; } = 1;
        public Vector2 MoveInput => moveInput;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private ParticleSystem dustFx;

        private Rigidbody2D rb;
        private PlayerCollisionDetector collisionDetector;
        private Vector2 moveInput;
        private bool isWallJumping;
        private bool FacingRight { get; set; } = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            collisionDetector = GetComponent<PlayerCollisionDetector>();
        }

        public void HandleInput(Vector2 input) => moveInput = input;
        
        public void SetWallJumpingState(bool state) => isWallJumping = state;
        
        public void HandleMovement()
        {
            if (collisionDetector.IsWallDetected) return;

            if (isWallJumping) return;

            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }

        public void HandleWallSlide(Vector2 wallSlideInput)
        {
            bool canWallSlide = collisionDetector.IsWallDetected && rb.linearVelocity.y < 0;
            float yModifer = wallSlideInput.y < 0 ? 1 : .05f;

            if (!canWallSlide) return;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifer);
        }

        public void HandleFlip()
        {
            if (moveInput.x < 0 && FacingRight || moveInput.x > 0 && !FacingRight) Flip();
        }

        public void Flip()
        {
            FacingDir = FacingDir * -1;
            transform.Rotate(0, 180, 0);
            FacingRight = !FacingRight;
            collisionDetector.SetFacingDir(FacingDir);
        }

        public void PlayDustFx() => dustFx.Play();
    }
}
