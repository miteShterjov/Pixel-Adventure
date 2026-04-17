using UnityEngine;

namespace Enemies
{
    public class EnemySnail : Enemy
    {
        [Header("Snail details")]
        [SerializeField] private EnemySnailBody bodyPrefab;
        [SerializeField] private float maxSpeed = 10;
        
        private bool hasBody = true;
        private static readonly int WallHitParam = Animator.StringToHash("wallHit");
        private static readonly int HitParam = Animator.StringToHash("hit");

        protected override void Update()
        {
            base.Update();

            if (isDead) return;

            HandleMovement();

            if (isGrounded) HandleTurnAround();
        }

        public override void Die()
        {
            if (hasBody)
            {
                canMove = false;
                hasBody = false;
                anim.SetTrigger(HitParam);

                rb.linearVelocity = Vector2.zero;
                idleDuration = 0;
            }
            
            else if (!canMove && !hasBody)
            {
                anim.SetTrigger(HitParam);
                canMove = true;
                moveSpeed = maxSpeed;
            }
            else base.Die();
        }

        protected override void Flip()
        {
            base.Flip();

            if (!hasBody) anim.SetTrigger(WallHitParam);
        }
        
        private void HandleTurnAround()
        {
            bool canFlipFromLedge = !isGroundInFrontDetected && hasBody;

            if (!canFlipFromLedge && !isWallDetected) return;
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }

        private void HandleMovement()
        {
            if (idleTimer > 0) return;

            if (!canMove) return;

            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
        }

        private void CreateBody()
        {
            EnemySnailBody newBody = Instantiate(bodyPrefab, transform.position, Quaternion.identity);


            if (Random.Range(0, 100) < 50) deathRotationDirection = deathRotationDirection * -1;

            newBody.SetupBody(deathImpactSpeed, deathRotationSpeed * deathRotationDirection,facingDir);

            Destroy(newBody.gameObject, 10);
        }
    }
}
