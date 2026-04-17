using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyRadish : Enemy
    {
        [Header("Radish details")]
        [SerializeField] private float flyForce;
        [SerializeField] private float walkDuration = 2;
        [Space]
        [SerializeField] private Material flashMaterial;

        private float xOriginalPosition;
        private float walkTimer;
        private float minFlyDistance;
        private RaycastHit2D groundBelowDetected;
        private bool isFlying;
        private Material originalMaterial;

        private static readonly int AnimFlyParam = Animator.StringToHash("isFlying");

        protected override void Start()
        {
            base.Start();

            originalMaterial = sr.material;
            xOriginalPosition = transform.position.x;
            isFlying = true;
            minFlyDistance = Physics2D.Raycast(transform.position, Vector2.down, float.MaxValue, whatIsGround).distance;
        }

        protected override void Update()
        {
            base.Update();

            if (isDead) return;

            walkTimer -= Time.deltaTime;

            if (isFlying) HandleFlying();
            else
            {
                float xDifference = Mathf.Abs(transform.position.x - xOriginalPosition);

                if (walkTimer < 0 && xDifference < .1f)
                {
                    rb.gravityScale = 1;
                    isFlying = true;
                }

                HandleMovement();
                HandleTurnAround();
            }
        }

        private void HandleFlying()
        {
            if (groundBelowDetected.distance < minFlyDistance) rb.linearVelocity = new Vector2(0, flyForce);
        }

        private void HandleTurnAround()
        {
            if (!isGrounded) return;
            if (isGroundInFrontDetected && !isWallDetected) return;

            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }

        private void HandleMovement()
        {
            if (!isGrounded) return;
            if (idleTimer > 0) return;

            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
        }

        protected override void HandleAnimator()
        {
            base.HandleAnimator();
            anim.SetBool(AnimFlyParam, isFlying);
        }

        protected override void HandleCollision()
        {
            base.HandleCollision();
            groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, float.MaxValue, whatIsGround);
        }

        public override void Die()
        {
            if (isFlying)
            {
                StartCoroutine(FlashFxCo());
                isFlying = false;
                walkTimer = walkDuration;
                rb.gravityScale = 3;
            }
            else base.Die();
        }

        private IEnumerator FlashFxCo()
        {
            const float flashTimer = 0.1f;
        
            sr.material = flashMaterial;
            yield return new WaitForSeconds(flashTimer);
            sr.material = originalMaterial;
        }
    }
}
