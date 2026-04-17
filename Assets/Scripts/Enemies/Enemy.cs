using Managers;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Header("General info")]
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected float idleDuration = 1.5f;
        [SerializeField] private bool isFacingRight;
        [Header("Death details")]
        [SerializeField] protected float deathImpactSpeed = 5;
        [SerializeField] protected float deathRotationSpeed = 150;
        [Header("Basic collision")]
        [SerializeField] protected float groundCheckDistance = 1.1f;
        [SerializeField] protected float wallCheckDistance = .7f;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected float playerDetectionDistance = 15;
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected Transform groundCheck;
        
        protected SpriteRenderer sr;
        protected Transform player;
        protected Animator anim;
        protected Rigidbody2D rb;
        protected bool isPlayerDetected;
        protected bool isGrounded;
        protected bool isWallDetected;
        protected bool isGroundInFrontDetected;
        protected int facingDir = -1;
        protected bool facingRight;
        protected int deathRotationDirection = 1;
        protected bool isDead;
        protected float idleTimer;
        protected bool canMove = true;

        private Collider2D[] colliders;
        
        private static readonly int HitTrigger = Animator.StringToHash("hit");
        private static readonly int XVelocityTrigger = Animator.StringToHash("xVelocity");

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            colliders = GetComponentsInChildren<Collider2D>();

            if (isFacingRight && facingDir == -1) Flip();
        }

        protected virtual void Start()
        {
            if (sr.flipX && !facingRight)
            {
                sr.flipX = false;
                Flip();
            }

            PlayerManager.OnPlayerRespawn += UpdatePlayerReference;
        }

        private void UpdatePlayerReference()
        {
            if (!player) player = PlayerManager.Instance.player.transform;
        }

        protected virtual void Update()
        {
            HandleCollision();
            HandleAnimator();

            idleTimer -= Time.deltaTime;

            if (isDead) HandleDeathRotation();
        }

        public virtual void Die()
        {
            EnableColliders(false);

            anim.SetTrigger(HitTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, deathImpactSpeed);
            isDead = true;

            if (Random.Range(0, 100) < 50) deathRotationDirection = deathRotationDirection * -1;

            PlayerManager.OnPlayerRespawn -= UpdatePlayerReference;
            Destroy(gameObject, 10);
        }
        
        [ContextMenu("Change Facing Direction")]
        public void FlipDefaultFacingDirections() => sr.flipX = !sr.flipX;

        protected void EnableColliders(bool enable)
        {
            foreach (Collider2D eachCollider in colliders)
            {
                eachCollider.enabled = enable;
            }
        }
        
        protected virtual void HandleFlip(float xValue)
        {
            if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight) Flip();
        }

        protected virtual void Flip()
        {
            facingDir = facingDir * -1;
            transform.Rotate(0, 180, 0);
            facingRight = !facingRight;
        }
        
        protected virtual void HandleAnimator() => anim.SetFloat(XVelocityTrigger, rb.linearVelocity.x);
        
        protected virtual void HandleCollision()
        {
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
            isGroundInFrontDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
            isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
            isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectionDistance, whatIsPlayer);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDetectionDistance * facingDir), transform.position.y));
        }
        protected void SetLinearVelocity(float xVelocity, float yVelocity) => rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    
        protected void SetLinearVelocity(Vector2 velocity) => rb.linearVelocity = velocity;
        
        private void HandleDeathRotation() => transform.Rotate(0, 0, (deathRotationSpeed * deathRotationDirection) * Time.deltaTime);
    }
}
