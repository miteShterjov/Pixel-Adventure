using System.Collections;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerJumpController : MonoBehaviour
    {
        public bool IsWallJumping { get; private set; }
        public bool IsAirborne { get; private set; }
        public void EnableDoubleJump() => CanDoubleJump = true;
        
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

        private float defaultGravityScale;

        private bool CanDoubleJump { get; set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (!rb) Debug.LogError("Rigidbody2D component not found on Player.");
            collisionDetector = GetComponent<PlayerCollisionDetector>();
            movementController = GetComponent<PlayerMovementController>();
        }

        private void Start()
        {
            defaultGravityScale = rb.gravityScale;
        }

        public void UpdateAirborneStatus()
        {
            if (collisionDetector.IsGrounded && IsAirborne) HandleLanding();

            if (!collisionDetector.IsGrounded && !IsAirborne) BecomeAirborne();
        }

        private void BecomeAirborne()
        {
            IsAirborne = true;

            if (rb.linearVelocity.y < 0)
                ActivateCoyoteJump();
        }

        private void HandleLanding()
        {
            movementController.PlayDustFx();
            IsAirborne = false;
            CanDoubleJump = true;
            AttemptBufferJump();
        }

        public void RequestBufferJump()
        {
            if (IsAirborne)
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
            else if (CanDoubleJump)
            {
                DoubleJump();
            }

            CancelCoyoteJump();
        }

        private void Jump()
        {
            movementController.PlayDustFx();
            AudioManager.Instance.PlaySfx(3);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        private void DoubleJump()
        {
            movementController.PlayDustFx();
            AudioManager.Instance.PlaySfx(3);

            StopCoroutine(WallJumpRoutine());
            IsWallJumping = false;
            movementController.SetWallJumpingState(false);
            CanDoubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        }

        private void WallJump()
        {
            movementController.PlayDustFx();
            AudioManager.Instance.PlaySfx(12);

            CanDoubleJump = true;
            rb.linearVelocity = new Vector2(wallJumpForce.x * -movementController.FacingDir, wallJumpForce.y);

            movementController.Flip();

            StopAllCoroutines();
            StartCoroutine(WallJumpRoutine());
        }

        private IEnumerator WallJumpRoutine()
        {
            IsWallJumping = true;
            movementController.SetWallJumpingState(true);

            yield return new WaitForSeconds(wallJumpDuration);

            IsWallJumping = false;
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
}
