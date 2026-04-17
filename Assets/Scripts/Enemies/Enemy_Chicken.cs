using UnityEngine;

namespace Enemies
{
    public class EnemyChicken : Enemy
    {
        [Header("Chicken details")]
        [SerializeField] private float aggroDuration;
        [SerializeField] private float detectionRange;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float speedIncreaseRate = 0.5f;

        private float aggroTimer;
        private float lastTimeAttacked;
        private float currentMoveSpeed;
        private bool isChasing;
        private bool canFlip = true;

        protected override void Update()
        {
            base.Update();

            aggroTimer -= Time.deltaTime;

            if (isDead) return;

            bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

            if (isPlayerDetected)
            {
                canMove = true;
                aggroTimer = aggroDuration;

                if (!isChasing && canAttack) Attack();

                if (isChasing) currentMoveSpeed += speedIncreaseRate * Time.deltaTime;
            }

            if (aggroTimer < 0)
            {
                canMove = false;
                isChasing = false;
                currentMoveSpeed = moveSpeed;
            }

            HandleMovement();

            if (isGrounded) HandleTurnAround();
        }
        
        protected override void HandleFlip(float xValue)
        {
            if ((!(xValue < transform.position.x) || !facingRight) &&
                (!(xValue > transform.position.x) || facingRight)) return;
            if (!canFlip) return;
            canFlip = false;
            
            Invoke(nameof(Flip), .3f);
        }

        protected override void Flip()
        {
            base.Flip();
            canFlip = true;
        }

        private void HandleTurnAround()
        {
            if (isGroundInFrontDetected && !isWallDetected) return;
            Flip();
            canMove = false;
            SetLinearVelocity(Vector2.zero);
        }

        private void Attack()
        {
            lastTimeAttacked = Time.time;
            isChasing = true;
            currentMoveSpeed = moveSpeed;
        }

        private void HandleMovement()
        {
            if (!canMove) return;
            if (!player) return;

            if (player) HandleFlip(player.transform.position.x);

            float speedToUse = isChasing ? currentMoveSpeed : moveSpeed;
            SetLinearVelocity(new Vector2(speedToUse * facingDir, rb.linearVelocity.y));
        }
    }
}
